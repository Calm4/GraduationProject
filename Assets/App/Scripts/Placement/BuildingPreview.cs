using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class BuildingPreview : MonoBehaviour
    {
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private float previewYOffset = 0.06f;
        [SerializeField] private GameObject cellIndicator;
        [SerializeField] private Material previewMaterialPrefab;
        
        private Building _previewObject;
        private Material _previewMaterialInstance;
        private Renderer _cellIndicatorRenderer;

        private void Start()
        {
            _previewMaterialInstance = new Material(previewMaterialPrefab);
            cellIndicator.SetActive(false);
            _cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
        }

        private void PrepareMeshAndMaterial(Building buildingPrefab)
        {
            //TODO: ПОФИКСИТЬ СИСТЕМУ PREVIEW
            MeshFilter prefabMeshFilter = buildingPrefab.GetComponent<MeshFilter>();
            if (prefabMeshFilter != null)
            {
                MeshFilter previewMeshFilter = _previewObject.GetComponent<MeshFilter>();
                if (previewMeshFilter != null)
                {
                    previewMeshFilter.mesh = prefabMeshFilter.sharedMesh;
                }
            }

            MeshRenderer prefabMeshRenderer = buildingPrefab.GetComponent<MeshRenderer>();
            if (prefabMeshRenderer != null)
            {
                MeshRenderer previewMeshRenderer = _previewObject.GetComponent<MeshRenderer>();
                if (previewMeshRenderer != null)
                {
                    previewMeshRenderer.material = prefabMeshRenderer.sharedMaterial;
                }
            }
        }

        public void StartShowingPlacementPreview(Building prefab, Vector2Int size)
        {
            if (_previewObject != null)
            {
                Destroy(_previewObject.gameObject);
            }
        
            _previewObject = buildingManager.CreateBuilding(prefab);
            PrepareMeshAndMaterial(_previewObject);
            PreparePreview(_previewObject);
            PrepareCursor(size);
            cellIndicator.SetActive(true);
        }
        private void PrepareCursor(Vector2Int size)
        {
            if (size.x > 0 || size.y > 0)
            {
                cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
                _cellIndicatorRenderer.material.mainTextureScale = size;
            }
        }

        private void PreparePreview(Building building)
        {
            Renderer[] renderers = building.GetComponentsInChildren<Renderer>();
            foreach (var render in renderers)
            {
                Material[] materials = render.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = _previewMaterialInstance;
                }

                render.materials = materials;
            }
        }

        public void StopShowingPreview()
        {
            cellIndicator.SetActive(false);
            if (_previewObject != null)
            {
                Destroy(_previewObject.gameObject);
                _previewObject = null;
            }
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {
            if (_previewObject != null)
            {
                MovePreview(position);
                ApplyFeedbackToPreview(validity);
            }

            MoveCursor(position);
            ApplyFeedbackToCursor(validity);
        }

        private void ApplyFeedbackToPreview(bool validity)
        {
            Color color = validity ? Color.white : Color.red;
            color.a = 0.5f;

            _previewMaterialInstance.color = color;
        }

        private void ApplyFeedbackToCursor(bool validity)
        {
            Color color = validity ? Color.white : Color.red;
            color.a = 0.5f;

            _cellIndicatorRenderer.material.color = color;
        }

        private void MoveCursor(Vector3 position)
        {
            cellIndicator.transform.position = position;
        }

        private void MovePreview(Vector3 position)
        {
            _previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
        }

        public void StartShowingRemovePreview()
        {
            cellIndicator.SetActive(true);
            PrepareCursor(Vector2Int.one);
            ApplyFeedbackToCursor(false);
        }
    }
}