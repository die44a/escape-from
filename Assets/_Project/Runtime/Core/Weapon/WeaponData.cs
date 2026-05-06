using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    // Используем понятное имя для Enum
    public enum WeaponType { Ranged, Melee }

    public abstract class WeaponData : ScriptableObject
    {
        [Header("Visuals")]
        public Sprite weaponSprite;
        public RuntimeAnimatorController animatorOverride;
        
        [Header("General Settings")]
        public WeaponType type;
        public string weaponName;
        public float attackRate = 0.2f;
        public float damage = 10f;
    }
}