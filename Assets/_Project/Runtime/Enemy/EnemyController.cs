using _Project.Runtime.Player.Controllers;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float detectionRadius = 2f;
        [SerializeField] private float patrolRadius = 3f;
        [SerializeField] private float patrolTargetClearance = 0.2f;
        [SerializeField] private int patrolTargetAttempts = 12;
        
        [SerializeField] private LayerMask mapMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private Vector2 playerVisualOffset = new Vector2(0, 0.5f);
        
        [SerializeField] private float damageAmount = 2f;
        [SerializeField] private float attackCooldown = 3f;
        private float _lastAttackTime;

        private EnemyMovement _movement;
        private Vector2 _startPosition;
        private Vector2 _patrolTarget;
        
        [Inject(Optional = true)] private PlayerController _player;
        [SerializeField] private PlayerController playerOverride;
        
        private Vector2 TargetPlayerPosition => (Vector2)_player.transform.position + playerVisualOffset;

        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            _startPosition = transform.position;
            SetNewPatrolTarget();
        }

        private void Start()
        {
            if (playerOverride != null)
            {
                _player = playerOverride;
                return;
            }

            if (_player != null) return;

#if UNITY_2023_1_OR_NEWER
            _player = FindFirstObjectByType<PlayerController>();
#else
            _player = FindObjectOfType<PlayerController>();
#endif
        }

        private void FixedUpdate()
        {
            if (_movement.GetKnockbackStatus()) return;

            if (_player != null && CanSeePlayer())
            {
                _movement.MoveTowards(TargetPlayerPosition);
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

        private bool CanSeePlayer()
        {
            if (_player == null) return false;

            if (Physics2D.OverlapCircle(_startPosition, detectionRadius, playerMask) == null)
                return false;
            
            var directionToPlayer = (TargetPlayerPosition - (Vector2)transform.position);
            var distance = directionToPlayer.magnitude;

            if (distance > detectionRadius) return false;

            if (obstacleMask.value != 0)
            {
                var obstacleHit = Physics2D.Raycast(
                    transform.position,
                    directionToPlayer.normalized,
                    distance,
                    obstacleMask
                );

                return obstacleHit.collider == null;
            }

            var hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distance, mapMask | playerMask);
            return hit.collider != null && hit.collider.gameObject == _player.gameObject;
        }

        private void Patrol()
        {
            _movement.MoveTowards(_patrolTarget);

            if (Vector2.Distance(transform.position, _patrolTarget) < 0.2f)
            {
                SetNewPatrolTarget();
            }
        }

        private void SetNewPatrolTarget()
        {
            for (var i = 0; i < Mathf.Max(1, patrolTargetAttempts); i++)
            {
                var candidate = _startPosition + Random.insideUnitCircle * patrolRadius;
                var mask = obstacleMask.value != 0 ? obstacleMask : mapMask;
                var blocked = Physics2D.OverlapCircle(candidate, patrolTargetClearance, mask) != null;
                if (blocked) continue;
                _patrolTarget = candidate;
                return;
            }

            _patrolTarget = _startPosition;
        }
        
        private void OnCollisionStay2D(Collision2D collision)
        {
            // Проверяем, не пора ли снова ударить
            if (Time.time - _lastAttackTime < attackCooldown) return;

            // Пытаемся получить интерфейс урона у того, с кем столкнулись
            if (!collision.gameObject.TryGetComponent<IDamageable>(out var damageable)) return;
            damageable.ApplyDamage(damageAmount);
            _lastAttackTime = Time.time;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var center = Application.isPlaying ? (Vector3)_startPosition : transform.position;
            Gizmos.DrawWireSphere(center, detectionRadius);

            if (_player == null) return;
            var spotted = CanSeePlayer();
            Gizmos.color = spotted ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, _player.transform.position);
        }
    }
}