using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    [CreateAssetMenu(fileName = "MeleeWeaponConfig", menuName = "Configs/MeleeWeaponConfig")]
    public class MeleeWeaponConfig : WeaponData
    {
        [Header("Melee Specs")]
        public float hitRadius = 1.2f;
        public float attackRate = 0.5f;
        public int damage = 10;
        public GameObject hitEffectPrefab;

        private void OnValidate() => type = WeaponType.Melee;
    }
}