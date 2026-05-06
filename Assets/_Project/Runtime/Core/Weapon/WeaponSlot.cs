using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.Weapon
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private GameObject lootBasePrefab; 
        
        private WeaponBase _currentWeapon;
        private WeaponData _currentData;       
        private GameObject _currentPrefab;    
        
        private DiContainer _container;

        [Inject]
        public void Construct(DiContainer container) => _container = container;

        public void SwapWeapon(GameObject newPrefab, WeaponData newData)
        {
            if (_currentWeapon != null)
                DropWeapon();

            _currentPrefab = newPrefab;
            _currentData = newData;
            
            var obj = _container.InstantiatePrefab(newPrefab, transform.position, Quaternion.identity, transform);
    
            _currentWeapon = obj.GetComponent<WeaponBase>();
            _currentWeapon.Initialize(newData);
        }

        private void DropWeapon()
        {
            if (lootBasePrefab == null || _currentData == null) return;

            var lootObj = _container.InstantiatePrefab(lootBasePrefab, transform.position, Quaternion.identity, null);
    
            if (lootObj.TryGetComponent<PickableWeapon>(out var pickable))
                pickable.Setup(_currentPrefab, _currentData);

            Destroy(_currentWeapon.gameObject);
            _currentWeapon = null;
        }
        
        public void TryAttack() => _currentWeapon?.TryAttack();
    }
}