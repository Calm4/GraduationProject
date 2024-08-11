using App.Scripts.Buildings;
using App.Scripts.Grid;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Custom_Windows
{
    public class CreateMapWindow : OdinEditorWindow
    {
        public const int GridMinSize = 1;
        public const int GridMaxSize = 50;

        [FormerlySerializedAs("gridDataAsset")]
        [InlineEditor(Expanded = true), VerticalGroup("Grid Data")]
        public GridDataSO gridDataSo;

        [FormerlySerializedAs("buildingConfigsData")] [SerializeField, HideInInspector] private BuildingsDataBase buildingsDataBase;

        private GridMapWindow _gridMapWindow;
        private RenderMapWindow _renderer;

        [MenuItem("Tools/Create Map \ud83d\uddfa\ufe0f")]
        private static void OpenWindow() => GetWindow<CreateMapWindow>().Show();

        protected override void OnEnable()
        {
            base.OnEnable();
            InitializeRenderer();
        }

        private void InitializeRenderer()
        {
            if (gridDataSo == null || buildingsDataBase == null) return;

            _gridMapWindow ??= new GridMapWindow(gridDataSo.gridSize);

            if (_renderer == null)
            {
                _renderer = new RenderMapWindow(this, _gridMapWindow, gridDataSo, buildingsDataBase);
            }
            else
            {
                _renderer.UpdateGrid(gridDataSo, buildingsDataBase);
            }
        }

        protected override void OnImGUI()
        {
            base.OnImGUI();

            if (_renderer == null)
            {
                InitializeRenderer();
            }

            _renderer?.Draw();
        }

        public void UpdateGridData()
        {
            if (gridDataSo == null || buildingsDataBase == null) return;

            _renderer?.UpdateGrid(gridDataSo, buildingsDataBase);
            Repaint();
        }

        public void ClearGrid()
        {
            gridDataSo.ClearGrid();
            _gridMapWindow.InitializeGrid(gridDataSo.gridSize, gridDataSo.gridObjects);
            Repaint();
        }
    }
}
 