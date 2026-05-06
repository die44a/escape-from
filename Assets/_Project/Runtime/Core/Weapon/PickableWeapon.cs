using System;
using _Project.Runtime.Interfaces;
using _Project.Runtime.Player.Main;
using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public class PickableWeapon : MonoBehaviour, IInteractable
    {
        [Header("Weapon Setup")]
        [SerializeField] private WeaponData weaponData; 
        [SerializeField] private GameObject weaponPrefab; 

        [Header("Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void Setup(GameObject prefab, WeaponData data)
        {
            weaponPrefab = prefab;
            weaponData = data;
            if (spriteRenderer != null) spriteRenderer.sprite = data.weaponSprite;
        }

        private void Start()
        {
            if (weaponData != null) spriteRenderer.sprite = weaponData.weaponSprite;
        }

        public SpriteRenderer Renderer => spriteRenderer;
        public bool IsInteractable => true;
        
        public void Interact(GameObject initiator, Action onComplete = null)
        {
            if (initiator.TryGetComponent<WeaponSlot>(out var slot))
            {
                slot.SwapWeapon(weaponPrefab, weaponData);
            
                onComplete?.Invoke();
                Destroy(gameObject);
            }
        }

        public string GetInteractionLabel() => $"Поднять {weaponData?.weaponName}";
    }
}