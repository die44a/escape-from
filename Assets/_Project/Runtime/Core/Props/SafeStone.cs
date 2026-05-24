using System;
using _Project.Runtime.Interfaces;
using _Project.Runtime.Player.Controllers;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.Props
{
    public class SafeRune : MonoBehaviour, IInteractable
    {
        [SerializeField] private CircleCollider2D safeZone;
        [SerializeField] private Animator animator;

        [Header("Cost for bonus time")]
        [SerializeField] private int bronzeCost;
        [SerializeField] private int silverCost;
        [SerializeField] private int goldCost;

        [Header("Reward")]
        [SerializeField] private float addTime = 45f;

        [Inject] private PlayerWalletController _wallet;
        [Inject] private HealthTimeController _health;

        private bool _isActivated;
        private bool _isRewardTaken;

        private static readonly int Activate =
            Animator.StringToHash("activate");

        public SpriteRenderer Renderer { get; private set; }

        public bool IsInteractable => !_isRewardTaken;

        public string GetInteractionLabel()
        {
            if (!_isActivated)
                return "Активировать рунический камень";

            if (!_isRewardTaken)
                return
                    $"Увеличить время на +{addTime} сек за {goldCost} золота, {silverCost} серебра и {bronzeCost} бронзы";

            return "Руна использована";
        }

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();

            if (safeZone)
                safeZone.enabled = false;
        }

        public void Interact(GameObject initiator, Action onComplete = null)
        {
            if (!_isActivated)
            {
                ActivateRune(onComplete);
                return;
            }

            if (_isRewardTaken)
                return;

            if (!_wallet.TrySpendBundle(bronzeCost, silverCost, goldCost))
                return;

            GiveReward(onComplete);
        }

        private void ActivateRune(Action onComplete)
        {
            _isActivated = true;

            if (animator)
                animator.SetTrigger(Activate);

            if (safeZone)
                safeZone.enabled = true;

            onComplete?.Invoke();
        }

        private void GiveReward(Action onComplete)
        {
            _isRewardTaken = true;

            _health.AddTime(addTime);

            onComplete?.Invoke();
        }
    }
}