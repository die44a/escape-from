using System;
using _Project.Runtime.Interfaces;
using _Project.Runtime.Player.Controllers;
using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public class PickableWeapon : MonoBehaviour, IInteractable
    {
        [Header("Weapon Setup")]
        [SerializeField] private WeaponConfig weaponConfig; 
        [SerializeField] private GameObject weaponPrefab; 

        [Header("Visuals")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void Setup(GameObject prefab, WeaponConfig config)
        {
            weaponPrefab = prefab;
            weaponConfig = config;
            if (spriteRenderer) spriteRenderer.sprite = config.weaponSprite;
        }

        private void Start()
        {
            if (weaponConfig) 
                spriteRenderer.sprite = weaponConfig.weaponSprite;
            else
                Debug.LogWarning($"Sprite Renderer not set at {name}");
        }

        public SpriteRenderer Renderer => spriteRenderer;
        public bool IsInteractable => true;
        
        public void Interact(GameObject initiator, Action onComplete = null)
        {
            if (initiator.TryGetComponent<WeaponSlot>(out var slot))
            {
                slot.SwapWeapon(weaponPrefab, weaponConfig);
            
                onComplete?.Invoke();
                Destroy(gameObject);
            }
        }

        public string GetInteractionLabel() => $"Поднять {weaponConfig?.weaponName}";
    }
}