using _Project.Runtime.Core.Weapon;
using _Project.Services;
using _Project.Services.Input;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Player.Controllers
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private GameObject lootBasePrefab;

        private WeaponBase _currentWeapon;
        private WeaponConfig _currentConfig;
        private GameObject _currentPrefab;

        private DiContainer _container;

        [Inject]
        public void Construct(
            DiContainer container)
        {
            _container = container;
        }

        public void SwapWeapon(GameObject newPrefab, WeaponConfig newConfig)
        {
            if (_currentWeapon)
                DropWeapon();

            _currentPrefab = newPrefab;
            _currentConfig = newConfig;
            
            var obj = _container.InstantiatePrefab(newPrefab, transform.position, Quaternion.identity, transform);
    
            _currentWeapon = obj.GetComponent<WeaponBase>();
            _currentWeapon.InitWeapon(newConfig);
        }

        private void DropWeapon()
        {
            if (!_currentWeapon) return;
            
            if (lootBasePrefab && _currentConfig && _currentPrefab)
            {
                var lootObj = _container.InstantiatePrefab(lootBasePrefab, transform.position, Quaternion.identity, null);

                if (lootObj.TryGetComponent<PickableWeapon>(out var pickable))
                    pickable.Setup(_currentPrefab, _currentConfig);
            }
            else
                Debug.LogWarning("Лут не создан: не хватает префаба или конфига, но старое оружие будет удалено.");

            Destroy(_currentWeapon.gameObject);
    
            _currentWeapon = null;
            _currentConfig = null;
            _currentPrefab = null;
        }

        public void TryAttack()
        {
            _currentWeapon?.TryAttack();
        }
    }
}