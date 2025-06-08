using UnityEngine;

namespace App.Scripts.Enemies
{
    public enum MovementStyle
    {
        Normal,
        Jumping,
        Zigzag,
        Hovering
    }
    
    [System.Serializable]
    public class EnemyMovementParameters
    {
        public MovementStyle movementStyle = MovementStyle.Normal;
        
        // Параметры прыжка
        public float jumpHeight = 0.5f;
        public float jumpFrequency = 2.0f;
        
        // Параметры зигзага
        public float zigzagAmplitude = 0.5f;
        public float zigzagFrequency = 2.0f;
        
        // Параметры парения
        public float hoverHeight = 1.0f;
        public float hoverSpeed = 1.0f;
        public float rotationSpeed = 50.0f;
    }
    
    public class EnemyMovementController
    {
        private Transform _transform;
        private EnemyMovementParameters _params;
        private float _timeOffset;
        private float _baseY;
        private Vector3 _startPosition;
        
        public EnemyMovementController(Transform transform, EnemyMovementParameters parameters)
        {
            _transform = transform;
            _params = parameters;
            _timeOffset = Random.Range(0f, 2f * Mathf.PI);
            _baseY = transform.position.y;
            _startPosition = transform.position;
        }
        
        public Vector3 ApplyMovementEffect(Vector3 basePosition, Vector3 targetPosition, float moveSpeed)
        {
            Vector3 nextPosition = Vector3.MoveTowards(
                basePosition, 
                new Vector3(targetPosition.x, _baseY, targetPosition.y),
                moveSpeed * Time.deltaTime);
            
            switch (_params.movementStyle)
            {
                case MovementStyle.Jumping:
                    ApplyJumpEffect(ref nextPosition);
                    break;
                    
                case MovementStyle.Zigzag:
                    ApplyZigzagEffect(ref nextPosition, targetPosition);
                    break;
                    
                case MovementStyle.Hovering:
                    ApplyHoverEffect(ref nextPosition);
                    break;
                    
                // Normal не требует дополнительных эффектов
                case MovementStyle.Normal:
                default:
                    break;
            }
            
            // Поворот в направлении движения
            RotateTowardsTarget(nextPosition, targetPosition);
            
            return nextPosition;
        }
        
        private void ApplyJumpEffect(ref Vector3 position)
        {
            position.y = _baseY + _params.jumpHeight * Mathf.Abs(Mathf.Sin((Time.time + _timeOffset) * _params.jumpFrequency));
        }
        
        private void ApplyZigzagEffect(ref Vector3 position, Vector3 target)
        {
            // Вычисляем перпендикулярное направление к вектору движения для зигзага
            Vector3 forward = target - position;
            forward.y = 0;
            
            if (forward.magnitude > 0.1f)
            {
                Vector3 right = Vector3.Cross(Vector3.up, forward.normalized);
                float zigzagOffset = Mathf.Sin((Time.time + _timeOffset) * _params.zigzagFrequency) * _params.zigzagAmplitude;
                
                // Применяем отклонение
                position += right * zigzagOffset;
            }
        }
        
        private void ApplyHoverEffect(ref Vector3 position)
        {
            // Плавное изменение высоты при парении
            position.y = _baseY + _params.hoverHeight + 
                         _params.jumpHeight * Mathf.Sin((Time.time + _timeOffset) * _params.hoverSpeed);
            
            // Добавляем небольшое вращение вокруг оси Y
            _transform.Rotate(0, _params.rotationSpeed * Time.deltaTime, 0);
        }
        
        private void RotateTowardsTarget(Vector3 currentPos, Vector3 targetPos)
        {
            Vector3 direction = new Vector3(targetPos.x, currentPos.y, targetPos.y) - currentPos;
            
            if (direction.magnitude > 0.1f)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                _transform.rotation = Quaternion.Slerp(_transform.rotation, rotation, Time.deltaTime * 5f);
            }
        }
    }
} 