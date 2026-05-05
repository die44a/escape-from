using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    [CreateAssetMenu(fileName = "MeleeWeaponData", menuName = "Project/Weapons/Melee Data")]
    public class MeleeWeaponData : WeaponData
    {
        [Header("Melee Specs")]
        public float hitRadius = 1.2f;
        
        public GameObject hitEffectPrefab;

        private void OnValidate() => type = WeaponType.Melee;
    }
}