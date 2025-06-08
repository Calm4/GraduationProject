#if UNITY_EDITOR
using App.Scripts.Buildings.UI;
using App.Scripts.Grid;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.CustomWindows.CreateMapWindow
{
    public class CreateMapWindow : OdinEditorWindow
    {
        public const int GridMinSize = 1;
        public const int GridMaxSize = 50;

        [InlineEditor(Expanded = true), VerticalGroup("Grid Data")]
        public GridDataSO gridDataSo;

        [SerializeField, HideInInspector]
        private BuildingsDataBaseBySectionsSO buildingsDataBaseBySections;

        private GridMapWindow _gridMapWindow;
        private RenderMapWindow _renderer;
        private Vector2Int _previousGridSize;

        [MenuItem("Tools/Create Map 🗺️")]
        private static void OpenWindow() => GetWindow<CreateMapWindow>().Show();

        protected override void OnEnable()
        {
            base.OnEnable();
            InitializeRenderer();

            if (gridDataSo != null)
            {
                _previousGridSize = gridDataSo.GridSize;
            }
        }

        private void InitializeRenderer()
        {
            if (gridDataSo == null || buildingsDataBaseBySections == null) return;

            _gridMapWindow ??= new GridMapWindow(gridDataSo.GridSize);

            if (_renderer == null)
            {
                _renderer = new RenderMapWindow(this, _gridMapWindow, gridDataSo, buildingsDataBaseBySections);
            }
            else
            {
                _renderer.UpdateGrid(gridDataSo, buildingsDataBaseBySections);
            }
        }

        protected override void OnImGUI()
        {
            base.OnImGUI();

            if (_renderer == null)
            {
                InitializeRenderer();
            }

            if (gridDataSo == null)
            {
                Debug.LogWarning("PLEASE INITIALIZE DATA");
                return;
            }

            if (gridDataSo.GridSize != _previousGridSize)
            {
                ClearGrid();
                _previousGridSize = gridDataSo.GridSize;
            }

            _renderer?.Draw();
        }

        public void UpdateGridData()
        {
            if (gridDataSo == null || buildingsDataBaseBySections == null) return;

            _renderer?.UpdateGrid(gridDataSo, buildingsDataBaseBySections);
            Repaint();
        }

        public void ClearGrid()
        {
            gridDataSo.ClearGrid();
            _gridMapWindow.InitializeGrid(gridDataSo.GridSize, gridDataSo.gridObjects);
            Repaint();
        }
    }
}
#endif   
    
