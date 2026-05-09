using System;
using _Project.Runtime.Core.General;
using _Project.Runtime.Player.Controllers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Runtime.Enemy
{

    [RequireComponent(typeof(EnemyMovement))]
    public abstract class EnemyController : MonoBehaviour
    {
        [SerializeField] protected float detectionRadius = 5f;
        [Tooltip("Враг не выйдет за этот радиус от точки спавна и агрится только если игрок внутри него.")]
        [SerializeField] protected float leashRadius = 5f;
        [SerializeField] protected LayerMask playerMask;
        [SerializeField] protected LayerMask obstacleMask;
        [SerializeField] protected LayerMask mapMask;

        [SerializeField] protected float stopDistanceToPlayer = 0.7f;
        [SerializeField] protected float patrolRadius = 3f;
        [SerializeField] protected float patrolTargetClearance = 0.2f;
        [SerializeField] protected int patrolTargetAttempts = 12;
        
        [SerializeField] protected float attackCooldown = 1.5f;
        
        protected float LastAttackTime;
        protected bool IsAttacking;
        protected EnemyMovement _movement;
        protected Vector2 _startPosition;
        private Vector2 _patrolTarget;

        public event Action OnAttack;
        
        [Inject(Optional = true)] protected PlayerController Player;
        
        protected Vector2 TargetPlayerPosition => (Vector2)Player.transform.position;

        protected virtual void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
            if (_movement == null)
            {
                enabled = false;
                return;
            }
            _startPosition = transform.position;
            SetNewPatrolTarget();
        }

        protected virtual void FixedUpdate()
        {
            if (_movement.GetKnockbackStatus()) return;

            if (IsAttacking)
            {
                _movement.Stop();
                return;
            }
            
            if (Player && CanSeePlayer())
            {
                var sqrDist = ((Vector2)transform.position - TargetPlayerPosition).sqrMagnitude;
                
                if (sqrDist <= stopDistanceToPlayer * stopDistanceToPlayer)
                {
                    _movement.Stop();
                    TryAttack();
                    OnAttack?.Invoke();
                }
                else
                {
                    _movement.MoveTowards(GetLeashClampedTarget(TargetPlayerPosition));
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
            if (!Player) return false;

            var spawnDistance = Vector2.Distance(_startPosition, TargetPlayerPosition);
            if (leashRadius > 0f && spawnDistance > leashRadius) return false;

            var distance = Vector2.Distance(transform.position, TargetPlayerPosition);
            if (distance > detectionRadius) return false;

            var direction = (TargetPlayerPosition - (Vector2)transform.position).normalized;
            LayerMask combinedObstacles = obstacleMask | mapMask;
            
            var hit = Physics2D.Raycast(transform.position, direction, distance, combinedObstacles);
            return !hit.collider;
        }

        private Vector2 GetLeashClampedTarget(Vector2 desiredTarget)
        {
            if (leashRadius <= 0f) return desiredTarget;

            var fromSpawn = desiredTarget - _startPosition;
            var dist = fromSpawn.magnitude;
            if (dist <= leashRadius) return desiredTarget;

            return _startPosition + fromSpawn / dist * leashRadius;
        }

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
        
        public void OnAttackEnd()
        {
            IsAttacking = false;
        }
    }
}