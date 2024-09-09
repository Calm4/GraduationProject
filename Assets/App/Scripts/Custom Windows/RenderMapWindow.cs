using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Custom_Windows
{
    public class RenderMapWindow
    {
        private GridMapWindow _gridMapWindow;
        private GridDataSO _gridDataSO;
        private BuildingsDataBase _buildingsDataBase;
        private BasicBuildingConfig _selectedBuildingConfig;
        private Vector2 _scrollPosition;
        private readonly CreateMapWindow _mapCreateWindow;

        public RenderMapWindow(CreateMapWindow mapCreateWindow, GridMapWindow gridMapWindow, GridDataSO gridDataSO, BuildingsDataBase buildingsDataBase)
        {
            _mapCreateWindow = mapCreateWindow;
            _gridMapWindow = gridMapWindow;
            _gridDataSO = gridDataSO;
            _buildingsDataBase = buildingsDataBase;
        }

        public void UpdateGrid(GridDataSO gridDataSO, BuildingsDataBase buildingsDataBase)
        {
            _gridDataSO = gridDataSO;
            _buildingsDataBase = buildingsDataBase;
            _gridMapWindow.InitializeGrid(gridDataSO.gridSize, gridDataSO.gridObjects);
        }

        public void Draw()
        {
            if (_gridDataSO == null)
            {
                SirenixEditorGUI.ErrorMessageBox("Please assign a GridDataAsset.");
                return;
            }

            if (_buildingsDataBase == null)
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
            var centeredBoldStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18
            };

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(_mapCreateWindow.position.width * 0.25f));
            GUILayout.Label("Grid Settings", centeredBoldStyle);
            GUILayout.Label("Grid Size", EditorStyles.boldLabel);

            _gridDataSO.gridSize = EditorGUILayout.Vector2IntField("", _gridDataSO.gridSize);
            _gridDataSO.gridSize = Vector2Int.Max(_gridDataSO.gridSize, new Vector2Int(CreateMapWindow.GridMinSize, CreateMapWindow.GridMinSize));
            _gridDataSO.gridSize = Vector2Int.Min(_gridDataSO.gridSize, new Vector2Int(CreateMapWindow.GridMaxSize, CreateMapWindow.GridMaxSize));

            GUILayout.Space(10);
            GUILayout.Label("Object Settings", centeredBoldStyle);
            GUILayout.Space(20);
        }

        private void DrawObjectSettings()
        {
            GUILayout.Label("Object Type", EditorStyles.boldLabel);

            if (_buildingsDataBase.buildingConfigs != null && _buildingsDataBase.buildingConfigs.Count > 0)
            {
                var buildingNames = _buildingsDataBase.buildingConfigs.ConvertAll(b => b.buildingName).ToArray();
                var selectedIndex = _selectedBuildingConfig != null
                    ? _buildingsDataBase.buildingConfigs.IndexOf(_selectedBuildingConfig)
                    : -1;

                selectedIndex = Mathf.Clamp(EditorGUILayout.Popup("", selectedIndex, buildingNames), 0, _buildingsDataBase.buildingConfigs.Count - 1);
                _selectedBuildingConfig = _buildingsDataBase.buildingConfigs[selectedIndex];
                _mapCreateWindow.UpdateGridData();
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
            if (GUILayout.Button("Export to JSON")) _gridDataSO.ExportToJson();
            if (GUILayout.Button("Clear Grid")) _mapCreateWindow.ClearGrid();

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
            if (_gridMapWindow == null || !_gridMapWindow.IsGridSizeValid(_gridDataSO.gridSize))
            {
                _gridMapWindow = new GridMapWindow(_gridDataSO.gridSize);
                _gridMapWindow.InitializeGrid(_gridDataSO.gridSize, _gridDataSO.gridObjects);
            }

            var scrollViewHeight = Mathf.Max(_mapCreateWindow.position.height * 0.9f, 300);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(_mapCreateWindow.position.width * 0.75f), GUILayout.Height(scrollViewHeight));

            float cellSize = CalculateCellSize();
            DrawGridCells(cellSize);

            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        private void DrawGridCells(float cellSize)
        {
            Rect gridRect = EditorGUILayout.GetControlRect(GUILayout.Width(cellSize * _gridDataSO.gridSize.x), GUILayout.Height(cellSize * _gridDataSO.gridSize.y));

            for (int y = _gridDataSO.gridSize.y - 1; y >= 0; y--)
            {
                for (int x = 0; x < _gridDataSO.gridSize.x; x++)
                {
                    Rect cellRect = new Rect(gridRect.x + x * cellSize, gridRect.y + (_gridDataSO.gridSize.y - 1 - y) * cellSize, cellSize, cellSize);
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
            var objInCell = _gridDataSO.gridObjects.Find(obj => IsObjectInCell(obj, x, y));

            if (objInCell != null)
            {
                if (objInCell.position.x == x && objInCell.position.y == y)
                {
                    Rect objectRect = new Rect(
                        cellRect.x,
                        cellRect.y - (objInCell.buildingConfig.size.y - 1) * cellRect.height,
                        cellRect.width * objInCell.buildingConfig.size.x,
                        cellRect.height * objInCell.buildingConfig.size.y
                    );

                    EditorGUI.DrawTextureTransparent(objectRect, objInCell.buildingConfig.sprite.texture, ScaleMode.StretchToFill);
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
                if (_gridMapWindow.CanPlaceObject(_selectedBuildingConfig, new Vector2Int(x, y), _gridDataSO.gridSize))
                {
                    ToggleObjectPlacement(_selectedBuildingConfig, new Vector3Int(x, y, 0));
                }
            }
            else if (Event.current.button == 1)
            {
                RemoveObjectAtAnyPosition(new Vector2Int(x, y));
            }
        }

        private bool IsObjectInCell(GridObjectData obj, int x, int y)
            => obj.position.x <= x && x < obj.position.x + obj.buildingConfig.size.x &&
               obj.position.y <= y && y < obj.position.y + obj.buildingConfig.size.y;

        private void ToggleObjectPlacement(BasicBuildingConfig buildingConfig, Vector3Int position)
        {
            if (_gridMapWindow.CanPlaceObject(buildingConfig, new Vector2Int(position.x, position.y), _gridDataSO.gridSize))
            {
                RemoveObjectAtAnyPosition(new Vector2Int(position.x, position.y));
                var newObject = new GridObjectData(buildingConfig, position);
                _gridDataSO.gridObjects.Add(newObject);
                _gridMapWindow.MarkOccupiedCells(buildingConfig, position, true);
                _mapCreateWindow.Repaint();
            }
        }

        private void RemoveObjectAtAnyPosition(Vector2Int position)
        {
            var objToRemove = _gridDataSO.gridObjects.Find(obj => IsObjectInCell(obj, position.x, position.y));
            if (objToRemove != null)
            {
                _gridMapWindow.MarkOccupiedCells(objToRemove.buildingConfig, objToRemove.position, false);
                _gridDataSO.gridObjects.Remove(objToRemove);
                _mapCreateWindow.Repaint();
            }
        }

        private float CalculateCellSize()
            => Mathf.Min(_mapCreateWindow.position.width * 0.7f / _gridDataSO.gridSize.x, _mapCreateWindow.position.height * 0.85f / _gridDataSO.gridSize.y);
    }
}
