using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Custom_Windows
{
    public class CreateMapWindow : OdinEditorWindow
    {
        [FormerlySerializedAs("gridDataAsset")]
        [InlineEditor(Expanded = true), VerticalGroup("Grid Data")]
        public GridDataSO gridDataSo;

        [SerializeField, HideInInspector]
        private BuildingConfigsData buildingConfigsData;

        private const int GridMinSize = 1;
        private const int GridMaxSize = 50;
        private bool[,] _grid;

        private BasicBuildingConfig _selectedBuildingConfig;
        private Vector2 _scrollPosition;

        [MenuItem("Tools/Create Map \ud83d\uddfa\ufe0f")]
        private static void OpenWindow() => GetWindow<CreateMapWindow>().Show();

        protected override void OnEnable()
        {
            if (gridDataSo == null) return;

            InitializeGrid(gridDataSo.gridSize);
            InitializeOccupiedCells();
        }


        protected override void OnImGUI()
        {
            base.OnImGUI();

            if (gridDataSo == null)
            {
                SirenixEditorGUI.ErrorMessageBox("Please assign a GridDataAsset.");
                return;
            }

            if (buildingConfigsData == null)
            {
                SirenixEditorGUI.ErrorMessageBox("Please assign BuildingConfigsData.");
                return;
            }
            
            DrawGridSettings();
            DrawObjectSettings();
            DrawGrid();
            
        }

        private void DrawGridSettings()
        {
            GUIStyle centeredBoldStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18
            };

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(position.width * 0.25f));
            GUILayout.Label("Grid Settings", centeredBoldStyle);
            GUILayout.Label("Grid Size", EditorStyles.boldLabel);

            gridDataSo.gridSize = EditorGUILayout.Vector2IntField("", gridDataSo.gridSize);
            gridDataSo.gridSize = Vector2Int.Max(gridDataSo.gridSize, new Vector2Int(GridMinSize, GridMinSize));
            gridDataSo.gridSize = Vector2Int.Min(gridDataSo.gridSize, new Vector2Int(GridMaxSize, GridMaxSize));

            GUILayout.Space(10);
            GUILayout.Label("Object Settings", centeredBoldStyle);
            GUILayout.Space(20);

            
        }

        private void DrawObjectSettings()
        {
            GUILayout.Label("Object Type", EditorStyles.boldLabel);

            if (buildingConfigsData.buildingConfigs != null && buildingConfigsData.buildingConfigs.Count > 0)
            {
                var buildingNames = buildingConfigsData.buildingConfigs.ConvertAll(b => b.buildingName).ToArray();
                int selectedIndex = _selectedBuildingConfig != null
                    ? buildingConfigsData.buildingConfigs.IndexOf(_selectedBuildingConfig)
                    : -1;

                selectedIndex = Mathf.Clamp(EditorGUILayout.Popup("", selectedIndex, buildingNames), 0, buildingConfigsData.buildingConfigs.Count - 1);
                _selectedBuildingConfig = buildingConfigsData.buildingConfigs[selectedIndex];
            }
            else
            {
                EditorGUILayout.LabelField("No Building Configs Available");
            }

            GUILayout.Space(20);

            if (_selectedBuildingConfig != null)
            {
                DrawBuildingConfigDetails();
            }
            else
            {
                EditorGUILayout.LabelField("No Building Config Selected");
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Export to JSON")) gridDataSo.ExportToJson();
            if (GUILayout.Button("Clear Grid")) ClearGrid();

            GUILayout.EndVertical();
        }

        private void DrawBuildingConfigDetails()
        {
            GUILayout.Label("Object Size", EditorStyles.boldLabel);
            GUILayout.Space(10);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector2IntField("", _selectedBuildingConfig.size);
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(10);

            if (_selectedBuildingConfig.sprite != null)
            {
                GUILayout.Label("Object Picture", EditorStyles.boldLabel);
                GUILayout.Space(10);

                Rect iconRect = EditorGUILayout.GetControlRect(GUILayout.Width(100), GUILayout.Height(100));
                EditorGUI.DrawTextureTransparent(iconRect, _selectedBuildingConfig.sprite.texture, ScaleMode.ScaleToFit);
            }
        }

        private void DrawGrid()
        {
            if (_grid == null || !IsGridSizeValid(gridDataSo.gridSize))
            {
                InitializeGrid(gridDataSo.gridSize);
            }

            float scrollViewHeight = Mathf.Max(position.height * 0.9f, 300);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width * 0.75f), GUILayout.Height(scrollViewHeight));

            float cellSize = CalculateCellSize();
            DrawGridCells(cellSize);

            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        private void DrawGridCells(float cellSize)
        {
            Rect gridRect = EditorGUILayout.GetControlRect(GUILayout.Width(cellSize * gridDataSo.gridSize.x), GUILayout.Height(cellSize * gridDataSo.gridSize.y));

            for (int y = gridDataSo.gridSize.y - 1; y >= 0; y--)
            {
                for (int x = 0; x < gridDataSo.gridSize.x; x++)
                {
                    Rect cellRect = new Rect(gridRect.x + x * cellSize, gridRect.y + (gridDataSo.gridSize.y - 1 - y) * cellSize, cellSize, cellSize);
                    DrawCell(cellRect, x, y);

                    if (Event.current.type == EventType.MouseDown && cellRect.Contains(Event.current.mousePosition))
                    {
                        HandleCellClick(x, y);
                        Event.current.Use();
                    }
                }
            }
        }

        private void DrawCell(Rect cellRect, int x, int y)
        {
            var objInCell = gridDataSo.gridObjects.Find(obj => IsObjectInCell(obj, x, y));

            if (objInCell != null)
            {
                if (objInCell.buildingConfig.sprite != null)
                {
                    EditorGUI.DrawTextureTransparent(cellRect, objInCell.buildingConfig.sprite.texture, ScaleMode.ScaleToFit);
                }
            }
            else
            {
                EditorGUI.DrawRect(cellRect, Color.green);
                DrawCellBorders(cellRect);
            }
        }

        private void DrawCellBorders(Rect cellRect)
        {
            EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, cellRect.width, 1), Color.black);
            EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y + cellRect.height - 1, cellRect.width, 1), Color.black);
            EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, 1, cellRect.height), Color.black);
            EditorGUI.DrawRect(new Rect(cellRect.x + cellRect.width - 1, cellRect.y, 1, cellRect.height), Color.black);
        }

        private void HandleCellClick(int x, int y)
        {
            if (Event.current.button == 0 && _selectedBuildingConfig != null)
            {
                if (CanPlaceObject(_selectedBuildingConfig, new Vector2Int(x, y)))
                {
                    ToggleObjectPlacement(_selectedBuildingConfig, new Vector3Int(x, y, 0));
                }
            }
            else if (Event.current.button == 1)
            {
                RemoveObjectAtAnyPosition(new Vector2Int(x, y));
            }
        }

        private bool IsWithinGrid(Vector2Int position, Vector2Int gridSize)
            => position.x < gridSize.x && position.y < gridSize.y;

        private bool IsGridSizeValid(Vector2Int newSize)
            => newSize.x != _grid.GetLength(0) || newSize.y != _grid.GetLength(1);

        private float CalculateCellSize()
            => Mathf.Min(position.width * 0.7f / gridDataSo.gridSize.x, position.height * 0.85f / gridDataSo.gridSize.y);

        private bool IsObjectInCell(GridObjectData obj, int x, int y)
            => obj.position.x <= x && x < obj.position.x + obj.buildingConfig.size.x &&
               obj.position.y <= y && y < obj.position.y + obj.buildingConfig.size.y;

        private void InitializeGrid(Vector2Int newSize)
        {
            _grid = new bool[newSize.x, newSize.y];
            foreach (var obj in gridDataSo.gridObjects)
            {
                if (IsWithinGrid((Vector2Int)obj.position, newSize))
                {
                    MarkOccupiedCells(obj.buildingConfig, obj.position, true);
                }
            }
        }

        private void InitializeOccupiedCells()
        {
            foreach (var obj in gridDataSo.gridObjects)
            {
                if (IsWithinGrid((Vector2Int)obj.position, gridDataSo.gridSize))
                {
                    MarkOccupiedCells(obj.buildingConfig, obj.position, true);
                }
            }
        }

        
        private void ToggleObjectPlacement(BasicBuildingConfig buildingConfig, Vector3Int position)
        {
            if (CanPlaceObject(buildingConfig, new Vector2Int(position.x, position.y)))
            {
                RemoveObjectAtAnyPosition(new Vector2Int(position.x, position.y));
                var newObject = new GridObjectData(buildingConfig, position);
                gridDataSo.gridObjects.Add(newObject);
                MarkOccupiedCells(buildingConfig, position, true);
                Repaint();
            }
        }

        private void RemoveObjectAtAnyPosition(Vector2Int position)
        {
            var objToRemove = gridDataSo.gridObjects.Find(obj => IsObjectInCell(obj, position.x, position.y));
            if (objToRemove != null)
            {
                MarkOccupiedCells(objToRemove.buildingConfig, objToRemove.position, false);
                gridDataSo.gridObjects.Remove(objToRemove);
                Repaint();
            }
        }

        private bool CanPlaceObject(BasicBuildingConfig buildingConfig, Vector2Int position)
        {
            for (int i = 0; i < buildingConfig.size.x; i++)
            {
                for (int j = 0; j < buildingConfig.size.y; j++)
                {
                    int posX = position.x + i;
                    int posY = position.y + j;

                    if (posX >= gridDataSo.gridSize.x || posY >= gridDataSo.gridSize.y || _grid[posX, posY])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void MarkOccupiedCells(BasicBuildingConfig buildingConfig, Vector3Int position, bool isOccupied)
        {
            for (int x = 0; x < buildingConfig.size.x; x++)
            {
                for (int y = 0; y < buildingConfig.size.y; y++)
                {
                    int gridX = position.x + x;
                    int gridY = position.y + y;

                    if (gridX >= 0 && gridX < _grid.GetLength(0) && gridY >= 0 && gridY < _grid.GetLength(1))
                    {
                        _grid[gridX, gridY] = isOccupied;
                    }
                }
            }
        }

        private void ClearGrid()
        {
            gridDataSo.ClearGrid();
            InitializeGrid(gridDataSo.gridSize);
        }
    }
}
