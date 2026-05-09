using System.Collections;
using UnityEngine;

namespace _Project.Runtime.Enemy
{
    [RequireComponent(typeof(EnemyMovement))]
    public class EnemyAnimationsController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private EnemyDamageController damageController;
        
        [Header("Visuals")]
        [SerializeField] private float flashDuration = 0.15f;
        

        private static readonly int WalkKey = Animator.StringToHash("walk") ;
        private static readonly int DeadKey = Animator.StringToHash("dead") ;
        private static readonly int DirectionKey = Animator.StringToHash("direction") ;
        private static readonly int AttackKey = Animator.StringToHash("attack");

        private Coroutine _hitEffectCoroutine;
        private readonly Color _hitColor = new Color(1f, 0.4f, 0.4f, 1f); 
        private readonly Color _normalColor = Color.white;
        private bool _isDead;
        
        private void OnEnable()
        {
            Debug.Assert(animator != null, $"Animator is missing on {gameObject.name}");
            Debug.Assert(movement != null, $"Movement is missing on {gameObject.name}");
            Debug.Assert(damageController != null, $"DamageController is missing on {gameObject.name}");
            
            damageController.OnHit += HandleHit;
            damageController.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            damageController.OnHit -= HandleHit;
            damageController.OnDeath -= HandleDeath;
        }

        private void LateUpdate()
        {
            animator.SetBool(WalkKey, movement.IsMoving);
            UpdateDirection(movement.LastDirection);
        }

        public void TriggerAttack()
        {
            if (_isDead) return;
            animator.SetTrigger(AttackKey);
        }

        private void HandleHit()
        {
            if (_hitEffectCoroutine != null) 
                StopCoroutine(_hitEffectCoroutine);

            _hitEffectCoroutine = StartCoroutine(HitFlashRoutine());
        }
        
        private IEnumerator HitFlashRoutine()
        {
            spriteRenderer.color = _hitColor;

            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.color = _normalColor;

            _hitEffectCoroutine = null;
        }

        private void HandleDeath()
        {
            if (!animator || _isDead) return;

            _isDead = true;
            animator.SetBool(WalkKey, false);
            animator.SetTrigger(DeadKey);
        }

        private void UpdateDirection(Vector2 moveInput)
        {
            if (moveInput.sqrMagnitude < 0.01f) return;

            var dir = 0;
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                dir = moveInput.x > 0 ? 3 : 2;
            else
                dir = moveInput.y > 0 ? 1 : 0;

            animator.SetInteger(DirectionKey, dir);
        }
    }
}
