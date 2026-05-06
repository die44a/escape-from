using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public class AnimationEventProxy : MonoBehaviour
    {
        private WeaponBase _weapon;

        private void Awake()
        {
            _weapon = GetComponentInParent<WeaponBase>();
        }

        public void OnAnimationAction()
        {
            if (_weapon != null)
                _weapon.OnAnimationAction();
        }
    }
}