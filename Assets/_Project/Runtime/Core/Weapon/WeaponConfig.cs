using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public enum WeaponType { Ranged, Melee }

    public abstract class WeaponConfig : ScriptableObject
    {
        [Header("Visuals")]
        public Sprite weaponSprite;
        public RuntimeAnimatorController animatorOverride;
        
        [Header("General Settings")]
        public WeaponType type;
        public string weaponName;
        public float attackRate = 0.2f;
        public float damage = 10f;
        public float knockbackForce = 4f;
        public float knockbackDuration = 0.3f;
    }
}