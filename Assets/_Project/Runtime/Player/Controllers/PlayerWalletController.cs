using _Project.Runtime.Core.Props;
using _Project.Runtime.Player.Main;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerWalletController : MonoBehaviour
    {
        [Header("Coin magnet")]
        [SerializeField] private float magnetRadius = 0.3f;
        [SerializeField] private float magnetSpeed = 2f;
        [SerializeField] private float collectDistance = 0.2f;
        [SerializeField] private LayerMask coinLayers;

        [Inject] private PlayerStats _stats;

        private readonly Collider2D[] _overlapBuffer = new Collider2D[32];

        private void Awake()
        {
            if (coinLayers.value == 0)
                coinLayers = LayerMask.GetMask("Props");
        }

        private void FixedUpdate()
        {
            var origin = (Vector2)transform.position;
            var count = Physics2D.OverlapCircleNonAlloc(origin, magnetRadius, _overlapBuffer, coinLayers);

            for (var i = 0; i < count; i++)
            {
                if (!_overlapBuffer[i] || !_overlapBuffer[i].TryGetComponent<Coin>(out var coin))
                    continue;

                if (!coin.PickupEnabled)
                    continue;

                PullCoin(coin, origin);
            }
        }

        private void PullCoin(Coin coin, Vector2 playerPosition)
        {
            if (!coin || !coin.PickupEnabled)
                return;

            var coinPosition = (Vector2)coin.transform.position;
            var toPlayer = playerPosition - coinPosition;
            var distance = toPlayer.magnitude;

            if (distance <= collectDistance)
            {
                CollectCoin(coin);
                return;
            }

            var step = magnetSpeed * Time.fixedDeltaTime;
            coin.transform.position = coinPosition + toPlayer.normalized * Mathf.Min(step, distance);
        }

        private void AddCoin(CoinType type)
        {
            switch (type)
            {
                case CoinType.Bronze:
                    _stats.AddBronze();
                    break;
                case CoinType.Silver:
                    _stats.AddSilver();
                    break;
                case CoinType.Gold:
                    _stats.AddGold();
                    break;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<Coin>(out var coin) || !coin.PickupEnabled)
                return;

            CollectCoin(coin);
        }

        private void CollectCoin(Coin coin)
        {
            if (!coin || !coin.PickupEnabled)
                return;

            AddCoin(coin.type);
            coin.Collect();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.85f, 0.2f, 0.35f);
            Gizmos.DrawWireSphere(transform.position, magnetRadius);
        }
    }
}