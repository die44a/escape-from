using _Project.Runtime.Player.Controllers;
using _Project.Runtime.Player.Main;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.Weapon
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField] protected float orbitDistance = 0.7f;
        [SerializeField] protected float smoothSpeed = 12f;
        [SerializeField] protected Transform visualChild;
        [SerializeField] private int baseSortingOrder = 3600;
        [SerializeField] private float verticalOffset = 1f; 
        
        protected PlayerController Player;
        
        protected Animator Animator;
        protected SpriteRenderer SpriteRenderer;
        
        private Vector3 _currentVelocity;
        protected float NextAttackTime;
        
        protected static readonly int AttackTrigger = Animator.StringToHash("attack");

        [Inject]
        public void Construct(PlayerController player) => this.Player = player;

        protected virtual void Awake()
        {
            SpriteRenderer = visualChild.GetComponent<SpriteRenderer>();
            Animator = visualChild.GetComponent<Animator>();
        }

        public virtual void InitWeapon(WeaponConfig config)
        {
            SpriteRenderer.sprite = config.weaponSprite;
    
            if (config.animatorOverride != null)
                Animator.runtimeAnimatorController = config.animatorOverride;
        }
        
        protected virtual void LateUpdate()
        {
            if (Player.CurrentState == PlayerState.Dead)
            {
                SpriteRenderer.enabled = false;
                return;
            }
            UpdatePositionAndRotation();
        }

        private void UpdatePositionAndRotation()
        {
            var dir = Player.LastDirection.normalized;
            if (dir.sqrMagnitude < 0.01f) dir = Vector2.right;

            var playerCenter = Player.transform.position + new Vector3(0, verticalOffset, 0);
            var targetPos = playerCenter + (Vector3)(dir * orbitDistance);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _currentVelocity, 1f / smoothSpeed);
            
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            SpriteRenderer.sortingOrder = baseSortingOrder + (dir.y > 0 ? -1 : 1);
        }

        public abstract void TryAttack();

        public abstract void OnAnimationAction();
    }
}