using _Project.Runtime.Core.General;
using _Project.Runtime.Player.Controllers;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Enemy
{
    // Мы помечаем класс как abstract, чтобы нельзя было случайно повесить 
    // "пустого" контроллера на моба. Нужно обязательно выбрать Melee или Ranged.
    [RequireComponent(typeof(EnemyMovement))]
    public abstract class EnemyController : MonoBehaviour
    {
        [Header("Base Detection")]
        [SerializeField] protected float detectionRadius = 5f;
        [SerializeField] protected LayerMask playerMask;
        [SerializeField] protected LayerMask obstacleMask;
        [SerializeField] protected LayerMask mapMask;
        [SerializeField] protected Vector2 playerVisualOffset = new Vector2(0, 0.5f);

        [Header("Base Movement")]
        [SerializeField] protected float stopDistanceToPlayer = 0.7f;
        [SerializeField] protected float patrolRadius = 3f;
        [SerializeField] protected float patrolTargetClearance = 0.2f;
        [SerializeField] protected int patrolTargetAttempts = 12;

        [Header("Base Combat")]
        [SerializeField] protected float attackCooldown = 1.5f;
        [SerializeField] protected float attackRange = 0.8f;
        
        protected float _lastAttackTime;
        protected EnemyMovement _movement;
        protected Vector2 _startPosition;
        protected Vector2 _patrolTarget;
        
        [Inject(Optional = true)] protected PlayerController _player;
        
        protected Vector2 TargetPlayerPosition => (Vector2)_player.transform.position + playerVisualOffset;

        protected virtual void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            if (_movement == null)
            {
                Debug.LogError($"{nameof(EnemyController)} требует компонент {nameof(EnemyMovement)} на объекте '{name}'.", this);
                enabled = false;
                return;
            }
            _startPosition = transform.position;
            SetNewPatrolTarget();
        }

        protected virtual void FixedUpdate()
        {
            if (_movement.GetKnockbackStatus()) return;

            if (_player && CanSeePlayer())
            {
                float sqrDist = ((Vector2)transform.position - TargetPlayerPosition).sqrMagnitude;
                
                if (sqrDist <= stopDistanceToPlayer * stopDistanceToPlayer)
                {
                    _movement.Stop();
                    TryAttack(); // Этот метод будет переопределен в Orc или Archer
                }
                else
                {
                    _movement.MoveTowards(TargetPlayerPosition);
                }
            }
            else if (Vector2.Distance(transform.position, _startPosition) > 1f)
            {
                _movement.MoveTowards(_startPosition);
            }
            else
            {
                Patrol();
            }
        }

        protected virtual bool CanSeePlayer()
        {
            if (!_player) return false;

            float distance = Vector2.Distance(transform.position, TargetPlayerPosition);
            if (distance > detectionRadius) return false;

            Vector2 direction = (TargetPlayerPosition - (Vector2)transform.position).normalized;
            LayerMask combinedObstacles = obstacleMask | mapMask;
            
            var hit = Physics2D.Raycast(transform.position, direction, distance, combinedObstacles);
            return hit.collider == null;
        }

        // Ключевой абстрактный метод — каждый тип врага реализует его сам
        protected abstract void TryAttack();

        protected virtual void Patrol()
        {
            _movement.MoveTowards(_patrolTarget);
            if (Vector2.Distance(transform.position, _patrolTarget) < 0.2f)
            {
                SetNewPatrolTarget();
            }
        }

        protected virtual void SetNewPatrolTarget()
        {
            for (var i = 0; i < Mathf.Max(1, patrolTargetAttempts); i++)
            {
                var candidate = _startPosition + Random.insideUnitCircle * patrolRadius;
                var mask = obstacleMask.value != 0 ? obstacleMask : mapMask;
                if (Physics2D.OverlapCircle(candidate, patrolTargetClearance, mask)) continue;
                
                _patrolTarget = candidate;
                return;
            }
            _patrolTarget = _startPosition;
        }

        protected virtual void OnDrawGizmos()
        {
            // Агро-радиус
            Gizmos.color = Color.yellow;
            Vector3 center = Application.isPlaying ? (Vector3)_startPosition : transform.position;
            Gizmos.DrawWireSphere(center, detectionRadius);

            // Зона атаки
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            if (_player != null && CanSeePlayer())
            {
                Gizmos.DrawLine(transform.position, TargetPlayerPosition);
            }
        }
    }
}