using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Grid;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Custom_Windows
{
    public class EditorUI
    {
        private GridDataSO _gridData;
        private BuildingConfigsData _buildingConfigsData;
        private GridEditor _gridEditor;
        private BasicBuildingConfig _selectedBuildingConfig;
        private Vector2 _scrollPosition;

        public EditorUI(GridDataSO gridData, BuildingConfigsData buildingConfigsData, GridEditor gridEditor)
        {
            _gridData = gridData;
            _buildingConfigsData = buildingConfigsData;
            _gridEditor = gridEditor;
        }

        public bool DisplayErrors()
        {
            if (_gridData == null)
            {
                SirenixEditorGUI.ErrorMessageBox("Please assign a GridDataAsset.");
                return true;
            }

            if (_buildingConfigsData == null)
            {
                SirenixEditorGUI.ErrorMessageBox("Please assign BuildingConfigsData.");
                return true;
            }

            return false;
        }

        public void DrawSettingsPanel()
        {
            GUIStyle centeredBoldStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18
            };

            float quarterOfWindowWidth = EditorGUILayout.GetControlRect().width * 0.25f;
            float threeQuarterOfWindowWidth = EditorGUILayout.GetControlRect().width * 0.75f;

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(quarterOfWindowWidth));
            GUILayout.Label("Grid Settings", centeredBoldStyle);
            GUILayout.Label("Grid Size", EditorStyles.boldLabel);
            _gridData.gridSize =
                EditorGUILayout.Vector2IntField("", _gridData.gridSize, GUILayout.MaxWidth(quarterOfWindowWidth));
            _gridData.gridSize = Vector2Int.Max(_gridData.gridSize, new Vector2Int(_gridEditor.MinGridSize, _gridEditor.MinGridSize));
            _gridData.gridSize = Vector2Int.Min(_gridData.gridSize, new Vector2Int(_gridEditor.MaxGridSize, _gridEditor.MaxGridSize));

            GUILayout.Space(10);
            GUILayout.Label("Object Settings", centeredBoldStyle);

            GUILayout.Space(20);
            GUILayout.Label("Object Type", EditorStyles.boldLabel);

            if (_buildingConfigsData.buildingConfigs != null && _buildingConfigsData.buildingConfigs.Count > 0)
            {
                var buildingNames = _buildingConfigsData.buildingConfigs.ConvertAll(b => b.buildingName).ToArray();
                int selectedIndex = _selectedBuildingConfig != null
                    ? _buildingConfigsData.buildingConfigs.IndexOf(_selectedBuildingConfig)
                    : -1;

                selectedIndex = Mathf.Clamp(EditorGUILayout.Popup("", selectedIndex, buildingNames,
                    GUILayout.Width(quarterOfWindowWidth)), 0, _buildingConfigsData.buildingConfigs.Count - 1);

                _selectedBuildingConfig = _buildingConfigsData.buildingConfigs[selectedIndex];
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
                _gridData.ExportToJson();
            }

            if (GUILayout.Button("Clear Grid"))
            {
                _gridEditor.ClearGrid();
            }

            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(threeQuarterOfWindowWidth));

            DrawGrid();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        public void DrawGrid()
        {
            if (_gridData.gridSize == null) return;

            float gridWidth = EditorGUILayout.GetControlRect().width * 0.7f;
            float gridHeight = EditorGUILayout.GetControlRect().height * 0.85f;

            float cellSize = Mathf.Min(gridWidth / _gridData.gridSize.x, gridHeight / _gridData.gridSize.y);

            float paddedGridWidth = cellSize * _gridData.gridSize.x;
            float paddedGridHeight = cellSize * _gridData.gridSize.y;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Rect gridRect =
                EditorGUILayout.GetControlRect(GUILayout.Width(paddedGridWidth), GUILayout.Height(paddedGridHeight));

            for (int y = _gridData.gridSize.y - 1; y >= 0; y--)
            {
                for (int x = 0; x < _gridData.gridSize.x; x++)
                {
                    Rect cellRect = new Rect(gridRect.x + x * cellSize,
                        gridRect.y + (_gridData.gridSize.y - 1 - y) * cellSize, cellSize, cellSize);

                    var objInCell = _gridData.gridObjects.Find(obj =>
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
                            if (_gridEditor.CanPlaceBuilding(_selectedBuildingConfig, new Vector2Int(x, y)))
                            {
                                _gridEditor.PlaceBuilding(_selectedBuildingConfig, new Vector3Int(x, y, 0));
                            }
                        }
                        else if (Event.current.button == 1)
                        {
                            _gridEditor.RemoveBuildingAtPosition(new Vector2Int(x, y));
                        }

                        Event.current.Use();
                    }
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
