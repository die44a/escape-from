using UnityEngine;

namespace _Project.Runtime.Core.Props
{
    public enum CoinType { Bronze, Silver, Gold }

    public class Coin : MonoBehaviour
    {
        [SerializeField] public CoinType type;

        public bool PickupEnabled { get; private set; } = true;

        public void SetPickupEnabled(bool enabled) => PickupEnabled = enabled;

        public void Collect()
        {
            if (!PickupEnabled)
                return;

            Destroy(gameObject);
        }
    }
}