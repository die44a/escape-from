using UnityEngine;

namespace _Project.Runtime.Core.Props
{
    public enum CoinType { Bronze, Silver, Gold }

    public class Coin : MonoBehaviour
    {
        [SerializeField] public CoinType type;
    
        public void Collect() 
            => Destroy(gameObject);
    }
}