using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    [CreateAssetMenu(fileName = "RangedWeaponData", menuName = "Project/Weapons/Ranged Data")]
    public class RangedWeaponData : WeaponData
    {
        [Header("Ranged Specs")]
        public GameObject bulletPrefab;
        public float bulletSpeed = 15f;
        public float recoilForce = 0.15f; 
        
        public float spread = 2f; 
        
        private void OnValidate() => type = WeaponType.Ranged;
    }
}