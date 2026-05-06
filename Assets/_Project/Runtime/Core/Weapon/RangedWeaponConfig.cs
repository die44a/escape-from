using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    [CreateAssetMenu(fileName = "RangedWeaponConfig", menuName = "Configs/RangedWeaponConfig")]
    public class RangedWeaponConfig : WeaponData
    {
        [Header("Ranged Specs")]
        public GameObject bulletPrefab;
        public float bulletSpeed = 15f;
        public float recoilForce = 0.15f; 
        
        public float spread = 2f; 
        
        private void OnValidate() => type = WeaponType.Ranged;
    }
}