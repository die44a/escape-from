using _Project.Runtime.Core.Props;
using _Project.Runtime.Player.Main;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerWalletController : MonoBehaviour
    {
        [Inject] private PlayerStats _stats;

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
            if (!other.TryGetComponent<Coin>(out var coin)) return;
            coin.Collect();
            AddCoin(coin.type);
        }
    }
}