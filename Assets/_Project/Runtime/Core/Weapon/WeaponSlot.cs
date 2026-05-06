using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField] private GameObject lootBasePrefab; 
        
        private WeaponBase _currentWeapon;
        private WeaponConfig _currentConfig;       
        private GameObject _currentPrefab;    
        
        public void SwapWeapon(GameObject newPrefab, WeaponConfig newConfig)
        {
            if (_currentWeapon != null)
                DropWeapon();

            _currentPrefab = newPrefab;
            _currentConfig = newConfig;
            
            var obj = Instantiate(newPrefab, transform.position, Quaternion.identity, transform);
    
            _currentWeapon = obj.GetComponent<WeaponBase>();
            _currentWeapon.InitWeapon(newConfig);
        }

        private void DropWeapon()
        {
            if (lootBasePrefab == null || _currentConfig == null) return;

            var lootObj = Instantiate(lootBasePrefab, transform.position, Quaternion.identity, null);
    
            if (lootObj.TryGetComponent<PickableWeapon>(out var pickable))
                pickable.Setup(_currentPrefab, _currentConfig);

            Destroy(_currentWeapon.gameObject);
            _currentWeapon = null;
        }
        
        public void TryAttack() => _currentWeapon?.TryAttack();
    }
}