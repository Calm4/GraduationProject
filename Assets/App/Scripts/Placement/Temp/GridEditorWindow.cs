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

        GUIStyle centeredBoldStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 18
        };
        
        float quarterOfWindowWidth = position.width * 0.25f;
        float threeShadesOfWindowWidth = position.width * 0.75f;

        GUILayout.BeginHorizontal();
        
        GUILayout.BeginVertical(GUILayout.Width(quarterOfWindowWidth));

        GUILayout.Label("Grid Settings", centeredBoldStyle);
        GUILayout.Label("Grid Size", EditorStyles.boldLabel);
        gridDataAsset.gridSize = EditorGUILayout.Vector2IntField("", gridDataAsset.gridSize, GUILayout.MaxWidth(quarterOfWindowWidth));
        gridDataAsset.gridSize = Vector2Int.Max(gridDataAsset.gridSize, new Vector2Int(_gridMinSize, _gridMinSize));
        gridDataAsset.gridSize = Vector2Int.Min(gridDataAsset.gridSize, new Vector2Int(_gridMaxSize, _gridMaxSize));

        GUILayout.Space(10);
        GUILayout.Label("Object Settings", centeredBoldStyle);
        GUILayout.Label("Object Size", EditorStyles.boldLabel);
        currentSize = EditorGUILayout.Vector2IntField("", currentSize, GUILayout.MaxWidth(quarterOfWindowWidth));
        currentSize = Vector2Int.Max(currentSize, new Vector2Int(_objectMinSize, _objectMinSize));
        currentSize = Vector2Int.Min(currentSize, new Vector2Int(_objectMaxSize, _objectMaxSize));

        GUILayout.Space(20); // Space before buttons
        if (GUILayout.Button("Export to JSON"))
        {
            gridDataAsset.ExportToJson();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            ClearGrid();
        }

        GUILayout.EndVertical();

        GUILayout.BeginVertical(GUILayout.Width(threeShadesOfWindowWidth));

        if (grid == null || gridDataAsset.gridSize.x != grid.GetLength(0) ||
            gridDataAsset.gridSize.y != grid.GetLength(1))
        {
            InitializeGrid(gridDataAsset.gridSize);
        }

        float scrollViewHeight = Mathf.Max(position.height * 0.9f, 300);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(threeShadesOfWindowWidth), GUILayout.Height(scrollViewHeight));

        DrawGrid();

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void DrawGrid()
    {
        float gridWidth = position.width * 0.7f;
        float gridHeight = position.height * 0.85f;

        float cellSize = Mathf.Min(gridWidth / gridDataAsset.gridSize.x, gridHeight / gridDataAsset.gridSize.y);

        float paddedGridWidth = cellSize * gridDataAsset.gridSize.x;
        float paddedGridHeight = cellSize * gridDataAsset.gridSize.y;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Centering the grid horizontally
        Rect gridRect = EditorGUILayout.GetControlRect(GUILayout.Width(paddedGridWidth), GUILayout.Height(paddedGridHeight));

        for (int y = gridDataAsset.gridSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridDataAsset.gridSize.x; x++)
            {
                bool isOccupied = grid[x, y];
                Color color = isOccupied ? Color.red : Color.green;

                Rect cellRect = new Rect(gridRect.x + x * cellSize, gridRect.y + (gridDataAsset.gridSize.y - 1 - y) * cellSize, cellSize, cellSize);
                EditorGUI.DrawRect(cellRect, color);

                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, cellSize, 1), Color.black);
                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y + cellSize - 1, cellSize, 1), Color.black);
                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, 1, cellSize), Color.black);
                EditorGUI.DrawRect(new Rect(cellRect.x + cellSize - 1, cellRect.y, 1, cellSize), Color.black);

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
        }
        GUILayout.FlexibleSpace(); 
        GUILayout.EndHorizontal();
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
            Repaint();
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
