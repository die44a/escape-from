using _Project.Runtime.Player.Controllers;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private float patrolRadius = 3f;
        [SerializeField] private float patrolTargetClearance = 0.2f;
        [SerializeField] private int patrolTargetAttempts = 12;
        [SerializeField] private LayerMask mapMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask playerMask;

        private EnemyMovement _movement;
        private Vector2 _startPosition;
        private Vector2 _patrolTarget;
        
        [Inject(Optional = true)] private PlayerController _player;
        [SerializeField] private PlayerController playerOverride;

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
                _movement.MoveTowards(_player.transform.position);
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

            if (Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask) == null)
                return false;

            Vector2 directionToPlayer = (_player.transform.position - transform.position);
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

            if (Vector2.Distance(transform.position, _patrolTarget) < 0.7f)
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            if (_player == null) return;
            var spotted = CanSeePlayer();
            Gizmos.color = spotted ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, _player.transform.position);
        }
    }
}