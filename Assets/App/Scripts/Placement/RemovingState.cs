using UnityEngine;

namespace App.Scripts.Placement
{
    public class RemovingState : IBuildingState
    {
        private int gameObjectIndex = -1;
        private GridLayout _grid;
        private PreviewSystem _previewSystem;
        private GridData _floorData;
        private GridData _furnitureData;
        private ObjectPlacer _objectPlacer;
        private SoundFeedback _soundFeedback;

        public RemovingState(GridLayout grid, PreviewSystem previewSystem, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer,SoundFeedback soundFeedback)
        {
            _grid = grid;
            _previewSystem = previewSystem;
            _floorData = floorData;
            _furnitureData = furnitureData;
            _objectPlacer = objectPlacer;
            _soundFeedback = soundFeedback;

            previewSystem.StartShowingRemovePreview();
        }

        public void EndState()
        {
            _previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            GridData selectedData = null;
            if (!_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                selectedData = _furnitureData;
            }
            else if(_floorData.CanPlaceObjectAt(gridPosition,Vector2Int.one) == false)
            {
                selectedData = _floorData;
            }

            if (selectedData == null)
            {
                return;
            }
            else
            {
                gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
                if (gameObjectIndex == -1)
                {
                    return;
                }

                selectedData.RemoveObjectAt(gridPosition);
                _objectPlacer.RemoveObjectAt(gameObjectIndex);
            }

            Vector3 cellPosition = _grid.CellToWorld(gridPosition);
            _previewSystem.UpdatePosition(cellPosition, CheckIsSelectionIsValid(gridPosition));
        }

        private bool CheckIsSelectionIsValid(Vector3Int gridPosition)
        {
            return !(_furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
                     _floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool validity = CheckIsSelectionIsValid(gridPosition);
            _previewSystem.UpdatePosition(_grid.CellToWorld(gridPosition),validity);
        }
    }
}