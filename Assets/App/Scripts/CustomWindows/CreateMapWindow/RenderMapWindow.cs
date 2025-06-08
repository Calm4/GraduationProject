#if UNITY_EDITOR
using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Buildings.UI;
using App.Scripts.Grid;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.CustomWindows.CreateMapWindow
{
    public class RenderMapWindow
    {
        private GridMapWindow _gridMapWindow;
        private GridDataSO _gridDataSO;
        private BuildingsDataBaseBySectionsSO _buildingsDataBaseBySections;
        private Building _selectedBuilding;
        private BuildingType _selectedBuildingType;
        private Vector2 _scrollPosition;
        private readonly CreateMapWindow _mapCreateWindow;

        public RenderMapWindow(CreateMapWindow mapCreateWindow, GridMapWindow gridMapWindow, GridDataSO gridDataSO, BuildingsDataBaseBySectionsSO buildingsDataBaseBySections)
        {
            _mapCreateWindow = mapCreateWindow;
            _gridMapWindow = gridMapWindow;
            _gridDataSO = gridDataSO;
            _buildingsDataBaseBySections = buildingsDataBaseBySections;
        }

        public void UpdateGrid(GridDataSO gridDataSO, BuildingsDataBaseBySectionsSO buildingsDataBaseBySections)
        {
            _gridDataSO = gridDataSO;
            _buildingsDataBaseBySections = buildingsDataBaseBySections;
            _gridMapWindow.InitializeGrid(gridDataSO.GridSize, gridDataSO.gridObjects);
        }

        public void Draw()
        {
            if (_gridDataSO == null)
            {
                SirenixEditorGUI.ErrorMessageBox("Please assign a GridDataAsset.");
                return;
            }

            if (_buildingsDataBaseBySections == null)
            {
                SirenixEditorGUI.ErrorMessageBox("Please assign BuildingsDataBaseBySections.");
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

            _gridDataSO.GridSize = EditorGUILayout.Vector2IntField("", _gridDataSO.GridSize);
            _gridDataSO.GridSize = Vector2Int.Max(_gridDataSO.GridSize, new Vector2Int(CreateMapWindow.GridMinSize, CreateMapWindow.GridMinSize));
            _gridDataSO.GridSize = Vector2Int.Min(_gridDataSO.GridSize, new Vector2Int(CreateMapWindow.GridMaxSize, CreateMapWindow.GridMaxSize));

            GUILayout.Space(10);
            GUILayout.Label("Object Settings", centeredBoldStyle);
            GUILayout.Space(20);
        }

        private void DrawObjectSettings()
        {
            GUILayout.Label("Object Type", EditorStyles.boldLabel);

            _selectedBuildingType = (BuildingType)EditorGUILayout.EnumPopup("Select Section", _selectedBuildingType);

            if (_buildingsDataBaseBySections.BuildingsDataBaseBySections.ContainsKey(_selectedBuildingType))
            {
                var buildings = _buildingsDataBaseBySections.BuildingsDataBaseBySections[_selectedBuildingType];
                if (buildings.Count > 0)
                {
                    var buildingNames = buildings.ConvertAll(b => b.BuildingConfig.buildingName).ToArray();
                    var selectedIndex = _selectedBuilding != null
                        ? buildings.IndexOf(_selectedBuilding)
                        : -1;

                    selectedIndex = Mathf.Clamp(EditorGUILayout.Popup("", selectedIndex, buildingNames), 0, buildings.Count - 1);
                    _selectedBuilding = buildings[selectedIndex];
                    _mapCreateWindow.UpdateGridData();
                }
                else
                {
                    EditorGUILayout.LabelField("No Building Configs Available in this Section");
                }
            }
            else
            {
                EditorGUILayout.LabelField("No Building Configs Available");
            }

            GUILayout.Space(20);

            if (_selectedBuilding != null)
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
            EditorGUILayout.Vector2IntField("", _selectedBuilding.BuildingConfig.size);
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(10);

            if (_selectedBuilding.BuildingConfig.sprite != null)
            {
                GUILayout.Label("Object Picture", EditorStyles.boldLabel);
                GUILayout.Space(10);

                Rect iconRect = EditorGUILayout.GetControlRect(GUILayout.Width(100), GUILayout.Height(100));
                EditorGUI.DrawTextureTransparent(iconRect, _selectedBuilding.BuildingConfig.sprite.texture, ScaleMode.ScaleToFit);
            }
        }

        private void DrawGrid()
        {
            if (_gridMapWindow == null || !_gridMapWindow.IsGridSizeValid(_gridDataSO.GridSize))
            {
                _gridMapWindow = new GridMapWindow(_gridDataSO.GridSize);
                _gridMapWindow.InitializeGrid(_gridDataSO.GridSize, _gridDataSO.gridObjects);
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
            Rect gridRect = EditorGUILayout.GetControlRect(GUILayout.Width(cellSize * _gridDataSO.GridSize.x), GUILayout.Height(cellSize * _gridDataSO.GridSize.y));

            for (int y = _gridDataSO.GridSize.y - 1; y >= 0; y--)
            {
                for (int x = 0; x < _gridDataSO.GridSize.x; x++)
                {
                    Rect cellRect = new Rect(gridRect.x + x * cellSize, gridRect.y + (_gridDataSO.GridSize.y - 1 - y) * cellSize, cellSize, cellSize);
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
                if (objInCell.Position.x == x && objInCell.Position.y == y)
                {
                    Rect objectRect = new Rect(
                        cellRect.x,
                        cellRect.y - (objInCell.Building.BuildingConfig.size.y - 1) * cellRect.height,
                        cellRect.width * objInCell.Building.BuildingConfig.size.x,
                        cellRect.height * objInCell.Building.BuildingConfig.size.y
                    );

                    EditorGUI.DrawTextureTransparent(objectRect, objInCell.Building.BuildingConfig.sprite.texture, ScaleMode.StretchToFill);
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
            if (Event.current.button == 0 && _selectedBuilding != null)
            {
                if (_gridMapWindow.CanPlaceObject(_selectedBuilding, new Vector2Int(x, y), _gridDataSO.GridSize))
                {
                    ToggleObjectPlacement(_selectedBuilding, new Vector3Int(x, y, 0));
                }
            }
            else if (Event.current.button == 1)
            {
                RemoveObjectAtAnyPosition(new Vector2Int(x, y));
            }
        }

        private bool IsObjectInCell(GridObjectData obj, int x, int y)
            => obj.Position.x <= x && x < obj.Position.x + obj.Building.BuildingConfig.size.x &&
               obj.Position.y <= y && y < obj.Position.y + obj.Building.BuildingConfig.size.y;

        private void ToggleObjectPlacement(Building building, Vector3Int position)
        {
            if (_gridMapWindow.CanPlaceObject(building, new Vector2Int(position.x, position.y), _gridDataSO.GridSize))
            {
                RemoveObjectAtAnyPosition(new Vector2Int(position.x, position.y));
                var newObject = new GridObjectData(building, position);
                _gridDataSO.gridObjects.Add(newObject);
                _gridMapWindow.MarkOccupiedCells(building, position, true);
                _mapCreateWindow.Repaint();
            }
        }

        private void RemoveObjectAtAnyPosition(Vector2Int position)
        {
            var objToRemove = _gridDataSO.gridObjects.Find(obj => IsObjectInCell(obj, position.x, position.y));
            if (objToRemove != null)
            {
                _gridMapWindow.MarkOccupiedCells(objToRemove.Building, objToRemove.Position, false);
                _gridDataSO.gridObjects.Remove(objToRemove);
                _mapCreateWindow.Repaint();
            }
        }

        private float CalculateCellSize()
            => Mathf.Min(_mapCreateWindow.position.width * 0.7f / _gridDataSO.GridSize.x, _mapCreateWindow.position.height * 0.85f / _gridDataSO.GridSize.y);
    }
}
#endif