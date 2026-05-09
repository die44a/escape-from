using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class AnimationEventProxy : MonoBehaviour
    {
        private MeleeEnemy _parentController;

        private void Awake() => _parentController = GetComponentInParent<MeleeEnemy>();

        public void OnAttackFrame() 
            => _parentController.OnHitFrame();

        public void OnAttackEnd()
            => _parentController.OnAttackEnd();
    }
}
