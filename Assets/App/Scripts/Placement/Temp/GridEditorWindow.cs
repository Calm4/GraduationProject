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

    [InlineEditor, VerticalGroup]
    public GridDataAsset gridDataAsset;

    private int _gridWidth = 10;
    private int _gridHeight = 10;
    private int _gridMinSize = 1;
    private int _gridMaxSize = 50;
    private int _objectMinSize = 1;
    private int _objectMaxSize = 50;
    private bool[,] grid;

    private Vector2Int currentSize = new Vector2Int(1, 1); 

    private void OnEnable()
    {
        InitializeGrid(new Vector2Int(_gridWidth, _gridHeight));
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
        
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        _gridWidth = EditorGUILayout.IntField("Grid Width", _gridWidth);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        _gridHeight = EditorGUILayout.IntField("Grid Height", _gridHeight);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        _gridWidth = Mathf.Clamp(_gridWidth, _gridMinSize, _gridMaxSize);
        _gridHeight = Mathf.Clamp(_gridHeight, _gridMinSize, _gridMaxSize);

        if (grid == null || _gridWidth != grid.GetLength(0) || _gridHeight != grid.GetLength(1))
        {
            InitializeGrid(new Vector2Int(_gridWidth, _gridHeight));
        }

        currentSize = EditorGUILayout.Vector2IntField("Object Size", currentSize);
        currentSize = Vector2Int.Max(currentSize, new Vector2Int(_objectMinSize, _objectMinSize));
        currentSize = Vector2Int.Min(currentSize, new Vector2Int(_objectMaxSize, _objectMaxSize));

        float scrollViewHeight = position.height - 150; 
        if (scrollViewHeight < 300)
        {
            scrollViewHeight = 300; 
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(scrollViewHeight));
        DrawGrid();
        GUILayout.EndScrollView();

        GUILayout.BeginVertical(GUILayout.MinHeight(300));
        GUILayout.EndVertical();
    }



    private void InitializeGrid(Vector2Int newSize)
    {
        grid = new bool[newSize.x, newSize.y];
        
        gridDataAsset.ClearGrid();
        
        foreach (var obj in gridDataAsset.gridObjects)
        {
            if (obj.position.x < newSize.x && obj.position.y < newSize.y)
            {
                MarkOccupiedCells(obj.position, obj.size, true);
            }
        }
    }

    private void DrawGrid()
{
    GUILayout.Label("Grid", EditorStyles.boldLabel);

    float cellSize = Mathf.Min(position.width / _gridWidth, (position.height - 150) / _gridHeight); 
    
    GUILayout.BeginVertical();
    GUILayout.FlexibleSpace();

    for (int y = _gridHeight - 1; y >= 0; y--)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); 

        for (int x = 0; x < _gridWidth; x++)
        {
            bool isOccupied = grid[x, y];
            Color color = isOccupied ? Color.red : Color.green;
            
            Rect cellRect = GUILayoutUtility.GetRect(cellSize, cellSize);
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

        GUILayout.FlexibleSpace(); 
        GUILayout.EndHorizontal();
    }

    GUILayout.FlexibleSpace();
    GUILayout.EndVertical();
}


    private void ToggleObjectPlacement(Vector3Int position, Vector2Int size)
    {
        if (!grid[position.x, position.y])
        {
            var newObject = new GridObjectData("Building", position, size);
            gridDataAsset.gridObjects.Add(newObject);
            MarkOccupiedCells(position, size, true);
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

                if (posX >= _gridWidth || posY >= _gridHeight || grid[posX, posY])
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
}
