using UnityEngine;

namespace App.Scripts.Enemies
{
    public class EnemyMovementEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem jumpEffectParticles;
        [SerializeField] private ParticleSystem hoveredEffectParticles;
        [SerializeField] private ParticleSystem zigzagEffectParticles;
        [SerializeField] private TrailRenderer movementTrail;
        
        private Enemy _enemy;
        private MovementStyle _currentStyle;
        
        void Start()
        {
            _enemy = GetComponentInParent<Enemy>() ?? GetComponent<Enemy>();
            if (_enemy == null)
            {
                Debug.LogError("EnemyMovementEffects: Не удалось найти компонент Enemy!");
                return;
            }
            
            // Инициализация эффектов в зависимости от типа движения
            SetupEffectsForMovementStyle(_enemy.EnemyConfig.movementParameters.movementStyle);
        }
        
        public void SetupEffectsForMovementStyle(MovementStyle style)
        {
            _currentStyle = style;
            
            // Отключаем все эффекты
            if (jumpEffectParticles != null) jumpEffectParticles.Stop();
            if (hoveredEffectParticles != null) hoveredEffectParticles.Stop();
            if (zigzagEffectParticles != null) zigzagEffectParticles.Stop();
            if (movementTrail != null) movementTrail.emitting = false;
            
            // Включаем нужные эффекты в зависимости от типа движения
            switch (style)
            {
                case MovementStyle.Jumping:
                    if (jumpEffectParticles != null) jumpEffectParticles.Play();
                    if (movementTrail != null)
                    {
                        movementTrail.emitting = true;
                        movementTrail.startWidth = 0.1f;
                        movementTrail.endWidth = 0.05f;
                    }
                    break;
                
                case MovementStyle.Zigzag:
                    if (zigzagEffectParticles != null) zigzagEffectParticles.Play();
                    if (movementTrail != null)
                    {
                        movementTrail.emitting = true;
                        movementTrail.startWidth = 0.15f;
                        movementTrail.endWidth = 0.0f;
                    }
                    break;
                
                case MovementStyle.Hovering:
                    if (hoveredEffectParticles != null) hoveredEffectParticles.Play();
                    if (movementTrail != null)
                    {
                        movementTrail.emitting = true;
                        movementTrail.startWidth = 0.2f;
                        movementTrail.endWidth = 0.1f;
                    }
                    break;
            }
        }
        
        // Можно добавить эффект при смерти врага
        public void PlayDeathEffect()
        {
            // Если есть частицы для эффекта смерти, можно их здесь активировать
        }
    }
} 