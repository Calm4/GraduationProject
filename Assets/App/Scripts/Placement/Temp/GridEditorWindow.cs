using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;

public class GridEditorWindow : OdinEditorWindow
{
    [MenuItem("Tools/Grid Editor")]
    private static void OpenWindow()
    {
        GetWindow<GridEditorWindow>().Show();
    }

    [InlineEditor(Expanded = true), VerticalGroup("Grid Data")]
    public GridDataAsset gridDataAsset;

    private int _gridMinSize = 1;
    private int _gridMaxSize = 50;
    private int _objectMinSize = 1;
    private int _objectMaxSize = 50;
    private bool[,] grid;

    private Vector2Int currentSize = new Vector2Int(1, 1);

    private void OnEnable()
    {
        if (gridDataAsset == null)
        {
            Debug.LogError("GridDataAsset is not assigned. Please assign it in the inspector.");
            return;
        }

        InitializeGrid(new Vector2Int(gridDataAsset.gridSize.x, gridDataAsset.gridSize.y));

        foreach (var obj in gridDataAsset.gridObjects)
        {
            if (obj.position.x < gridDataAsset.gridSize.x && obj.position.y < gridDataAsset.gridSize.y)
            {
                MarkOccupiedCells(obj.position, obj.size, true);
            }
        }
    }

    private Vector2 scrollPosition;

    protected override void OnImGUI()
    {
        base.OnImGUI();

        if (gridDataAsset == null)
        {
            SirenixEditorGUI.ErrorMessageBox("Please assign a GridDataAsset.");
            return;
        }

        GUIStyle centeredBoldStyle = new GUIStyle(EditorStyles.boldLabel);
        centeredBoldStyle.alignment = TextAnchor.MiddleCenter; // Выравнивание текста по центру
        centeredBoldStyle.fontSize = 18;
        
        /*// Create a GUIStyle for the green background (for the grid section)
        GUIStyle greenBackgroundStyle = new GUIStyle();
        Texture2D greenBackgroundTexture = new Texture2D(1, 1);
        greenBackgroundTexture.SetPixels(new Color[] { Color.white });
        greenBackgroundTexture.Apply();
        greenBackgroundStyle.normal.background = greenBackgroundTexture;

        // Draw green background for the bottom section (grid)
        GUI.BeginGroup(new Rect(0, 150, position.width, position.height - 150));
        GUI.Box(new Rect(0, 0, position.width, position.height - 150), GUIContent.none, greenBackgroundStyle);
        GUI.EndGroup();*/

        // Draw the rest of the layout
        GUILayout.BeginHorizontal();

        // Input fields section (30% of the width)
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.25f));

        GUILayout.BeginVertical();
        // Grid Size input field
        GUILayout.Label("Grid Settings", centeredBoldStyle);
        GUILayout.Label("Grid Size", EditorStyles.boldLabel);
        gridDataAsset.gridSize = EditorGUILayout.Vector2IntField("", gridDataAsset.gridSize);
        gridDataAsset.gridSize = Vector2Int.Max(gridDataAsset.gridSize, new Vector2Int(_gridMinSize, _gridMinSize));
        gridDataAsset.gridSize = Vector2Int.Min(gridDataAsset.gridSize, new Vector2Int(_gridMaxSize, _gridMaxSize));
        
        // Object size controls
        GUILayout.Space(10);
        GUILayout.Label("Object Settings", centeredBoldStyle);
        GUILayout.Label("Object Size", EditorStyles.boldLabel);
        currentSize = EditorGUILayout.Vector2IntField("", currentSize);
        currentSize = Vector2Int.Max(currentSize, new Vector2Int(_objectMinSize, _objectMinSize));
        currentSize = Vector2Int.Min(currentSize, new Vector2Int(_objectMaxSize, _objectMaxSize));

        // Button section
        GUILayout.Space(20); // Space before buttons
        GUILayout.BeginVertical();;
        
        if (GUILayout.Button("Export to JSON"))
        {
            gridDataAsset.ExportToJson();
        }
        
        if (GUILayout.Button("Clear Grid"))
        {
            ClearGrid();
        }


        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.EndVertical();

        // Grid section (70% of the width)
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.7f));

        // Initialize the grid if necessary
        if (grid == null || gridDataAsset.gridSize.x != grid.GetLength(0) ||
            gridDataAsset.gridSize.y != grid.GetLength(1))
        {
            InitializeGrid(gridDataAsset.gridSize);
        }

        // Calculate the height of the scroll view
        float scrollViewHeight = Mathf.Max(position.height - 150, 300); // Ensure minimum height
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width * 0.7f),
            GUILayout.Height(scrollViewHeight));

        // Draw the grid with proper alignment
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        DrawGrid();
        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    private void DrawGrid()
    {
        float cellSize = Mathf.Min(position.width * 0.7f / gridDataAsset.gridSize.x,
            (position.height - 150) / gridDataAsset.gridSize.y);

        GUILayout.BeginVertical();
        for (int y = gridDataAsset.gridSize.y - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < gridDataAsset.gridSize.x; x++)
            {
                bool isOccupied = grid[x, y];
                Color color = isOccupied ? Color.red : Color.green;

                // Создаем прямоугольник для ячейки
                Rect cellRect = GUILayoutUtility.GetRect(cellSize, cellSize);
                EditorGUI.DrawRect(cellRect, color);

                // Рисуем границы ячейки
                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, cellSize, 1), Color.black);
                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y + cellSize - 1, cellSize, 1), Color.black);
                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, 1, cellSize), Color.black);
                EditorGUI.DrawRect(new Rect(cellRect.x + cellSize - 1, cellRect.y, 1, cellSize), Color.black);

                // Обработка кликов мыши
                if (Event.current.type == EventType.MouseDown && cellRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.button == 0)
                    {
                        if (CanPlaceObject(x, y, currentSize))
                        {
                            ToggleObjectPlacement(new Vector3Int(x, y, 0), currentSize);
                        }
                    }
                    else if (Event.current.button == 1)
                    {
                        RemoveObjectAtPosition(new Vector3Int(x, y, 0));
                    }

                    Event.current.Use();
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }



    private void InitializeGrid(Vector2Int newSize)
    {
        grid = new bool[newSize.x, newSize.y];

        foreach (var obj in gridDataAsset.gridObjects)
        {
            if (obj.position.x < newSize.x && obj.position.y < newSize.y)
            {
                MarkOccupiedCells(obj.position, obj.size, true);
            }
        }
    }


    private void ToggleObjectPlacement(Vector3Int position, Vector2Int size)
    {
        if (!grid[position.x, position.y])
        {
            var newObject = new GridObjectData("Building", position, size);
            gridDataAsset.gridObjects.Add(newObject);
            MarkOccupiedCells(position, size, true);
            Repaint(); // Перерисовать окно
        }
    }


    private void RemoveObjectAtPosition(Vector3Int position)
    {
        var objToRemove = gridDataAsset.gridObjects.Find(obj => obj.position == position);
        if (objToRemove != null)
        {
            MarkOccupiedCells(objToRemove.position, objToRemove.size, false);
            gridDataAsset.gridObjects.Remove(objToRemove);
        }
    }

    private bool CanPlaceObject(int x, int y, Vector2Int size)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                int posX = x + i;
                int posY = y + j;

                if (posX >= gridDataAsset.gridSize.x || posY >= gridDataAsset.gridSize.y || grid[posX, posY])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void MarkOccupiedCells(Vector3Int position, Vector2Int size, bool isOccupied)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                grid[position.x + x, position.y + y] = isOccupied;
            }
        }
    }

    private void ClearGrid()
    {
        gridDataAsset.ClearGrid();
        InitializeGrid(new Vector2Int(gridDataAsset.gridSize.x, gridDataAsset.gridSize.y));
    }
}