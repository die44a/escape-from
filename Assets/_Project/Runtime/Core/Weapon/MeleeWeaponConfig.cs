using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    [CreateAssetMenu(fileName = "MeleeWeaponConfig", menuName = "Configs/MeleeWeaponConfig")]
    public class MeleeWeaponConfig : WeaponConfig
    {
        [Header("Melee Specs")]
        public float hitRadius = 1.2f;
        public GameObject hitEffectPrefab;

        private void OnValidate() => type = WeaponType.Melee;
    }
}