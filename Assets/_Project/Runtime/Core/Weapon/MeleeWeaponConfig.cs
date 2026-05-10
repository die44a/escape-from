using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public enum AttackShape { Circle, Box }
    
    [CreateAssetMenu(fileName = "MeleeWeaponConfig", menuName = "Configs/MeleeWeaponConfig")]
    public class MeleeWeaponConfig : WeaponConfig
    {
        [Header("Melee Specs")]
        public AttackShape shape;
        public Vector2 boxSize = new (1.5f, 0.5f);
        public float hitRadius = 1.2f;
        public GameObject hitEffectPrefab;

        private void OnValidate() => type = WeaponType.Melee;
    }
}