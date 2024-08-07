using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.CustomWindows
{
    public class CreateMapWindow : OdinEditorWindow
    {
        [FormerlySerializedAs("gridDataAsset")] [InlineEditor(Expanded = true), VerticalGroup("Grid Data")]
        public GridDataSO gridDataSo;

        [SerializeField, HideInInspector] private BuildingConfigsData buildingConfigsData; // поменять на List<BasicBuildingConfig>

        private const int GridMinSize = 1;
        private const int GridMaxSize = 50;
        private bool[,] _grid;

        private BasicBuildingConfig _selectedBuildingConfig;
        private Vector2 _scrollPosition;

        [MenuItem("Tools/Create Map \ud83d\uddfa\ufe0f")]
        private static void OpenWindow()
        {
            GetWindow<CreateMapWindow>().Show();
        }

        protected override void OnEnable()
        {
            if (gridDataSo == null) return;
            
            InitializeGrid(new Vector2Int(gridDataSo.gridSize.x, gridDataSo.gridSize.y));

            foreach (var obj in gridDataSo.gridObjects)
            {
                if (obj.position.x < gridDataSo.gridSize.x && obj.position.y < gridDataSo.gridSize.y)
                {
                    MarkOccupiedCells(obj.buildingConfig, obj.position, true);
                }
            }
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
            gridDataSo.gridSize =
                EditorGUILayout.Vector2IntField("", gridDataSo.gridSize, GUILayout.MaxWidth(quarterOfWindowWidth));
            gridDataSo.gridSize = Vector2Int.Max(gridDataSo.gridSize, new Vector2Int(GridMinSize, GridMinSize));
            gridDataSo.gridSize = Vector2Int.Min(gridDataSo.gridSize, new Vector2Int(GridMaxSize, GridMaxSize));

            GUILayout.Space(10);
            GUILayout.Label("Object Settings", centeredBoldStyle);

            GUILayout.Space(20);
            GUILayout.Label("Object Type", EditorStyles.boldLabel);

            if (buildingConfigsData.buildingConfigs != null && buildingConfigsData.buildingConfigs.Count > 0)
            {
                var buildingNames = buildingConfigsData.buildingConfigs.ConvertAll(b => b.buildingName).ToArray();
                int selectedIndex = _selectedBuildingConfig != null
                    ? buildingConfigsData.buildingConfigs.IndexOf(_selectedBuildingConfig)
                    : -1;

                selectedIndex = Mathf.Clamp(EditorGUILayout.Popup("", selectedIndex, buildingNames,
                    GUILayout.Width(quarterOfWindowWidth)), 0, buildingConfigsData.buildingConfigs.Count - 1);

                _selectedBuildingConfig = buildingConfigsData.buildingConfigs[selectedIndex];
            }
            else
            {
                EditorGUILayout.LabelField("No Building Configs Available");
            }

            GUILayout.Space(20);

            if (_selectedBuildingConfig != null)
            {
                GUILayout.Label("Object Size", EditorStyles.boldLabel);
                GUILayout.Space(10);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Vector2IntField("", _selectedBuildingConfig.size, GUILayout.Width(quarterOfWindowWidth));
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
            else
            {
                EditorGUILayout.LabelField("No Building Config Selected");
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Export to JSON"))
            {
                gridDataSo.ExportToJson();
            }

            if (GUILayout.Button("Clear Grid"))
            {
                ClearGrid();
            }

            GUILayout.EndVertical();


            GUILayout.BeginVertical(GUILayout.Width(threeShadesOfWindowWidth));

            if (_grid == null || gridDataSo.gridSize.x != _grid.GetLength(0) ||
                gridDataSo.gridSize.y != _grid.GetLength(1))
            {
                InitializeGrid(gridDataSo.gridSize);
            }

            float scrollViewHeight = Mathf.Max(position.height * 0.9f, 300);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(threeShadesOfWindowWidth),
                GUILayout.Height(scrollViewHeight));

            DrawGrid();

            GUILayout.EndScrollView();


            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void DrawGrid()
        {
            float gridWidth = position.width * 0.7f;
            float gridHeight = position.height * 0.85f;

            float cellSize = Mathf.Min(gridWidth / gridDataSo.gridSize.x, gridHeight / gridDataSo.gridSize.y);

            float paddedGridWidth = cellSize * gridDataSo.gridSize.x;
            float paddedGridHeight = cellSize * gridDataSo.gridSize.y;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Rect gridRect =
                EditorGUILayout.GetControlRect(GUILayout.Width(paddedGridWidth), GUILayout.Height(paddedGridHeight));

            for (int y = gridDataSo.gridSize.y - 1; y >= 0; y--)
            {
                for (int x = 0; x < gridDataSo.gridSize.x; x++)
                {
                    Rect cellRect = new Rect(gridRect.x + x * cellSize,
                        gridRect.y + (gridDataSo.gridSize.y - 1 - y) * cellSize, cellSize, cellSize);

                    var objInCell = gridDataSo.gridObjects.Find(obj =>
                        obj.position.x <= x && x < obj.position.x + obj.buildingConfig.size.x &&
                        obj.position.y <= y && y < obj.position.y + obj.buildingConfig.size.y);

                    if (objInCell != null)
                    {
                        if (objInCell.buildingConfig.sprite != null)
                        {
                            EditorGUI.DrawTextureTransparent(cellRect, objInCell.buildingConfig.sprite.texture,
                                ScaleMode.ScaleToFit);
                        }
                    }
                    else
                    {
                        EditorGUI.DrawRect(cellRect, Color.green);

                        EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, cellSize, 1), Color.black);
                        EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y + cellSize - 1, cellSize, 1), Color.black);
                        EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, 1, cellSize), Color.black);
                        EditorGUI.DrawRect(new Rect(cellRect.x + cellSize - 1, cellRect.y, 1, cellSize), Color.black);
                    }

                    if (Event.current.type == EventType.MouseDown && cellRect.Contains(Event.current.mousePosition))
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

                        Event.current.Use();
                    }
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void InitializeGrid(Vector2Int newSize)
        {
            _grid = new bool[newSize.x, newSize.y];

            foreach (var obj in gridDataSo.gridObjects)
            {
                if (obj.position.x < newSize.x && obj.position.y < newSize.y)
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
            var objToRemove = gridDataSo.gridObjects.Find(obj =>
            {
                return obj.position.x <= position.x && position.x < obj.position.x + obj.buildingConfig.size.x &&
                       obj.position.y <= position.y && position.y < obj.position.y + obj.buildingConfig.size.y;
            });

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
            InitializeGrid(new Vector2Int(gridDataSo.gridSize.x, gridDataSo.gridSize.y));
        }
    }
}