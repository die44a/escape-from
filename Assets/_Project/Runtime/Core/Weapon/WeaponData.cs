using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    // Используем понятное имя для Enum
    public enum WeaponType { Ranged, Melee }

    public abstract class WeaponData : ScriptableObject
    {
        [Header("General Settings")]
        public WeaponType type;
        public string weaponName;
        public Sprite weaponSprite;
        public float attackRate = 0.2f; // Кулдаун между атаками
        public float damage = 10f;
    }
}