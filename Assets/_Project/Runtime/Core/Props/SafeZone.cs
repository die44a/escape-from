using UnityEngine;
using _Project.Runtime.Player.Controllers;

namespace _Project.Runtime.Core.Props
{
    public class SafeZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<HealthTimeController>(out var health))
            {
                health.EnterSafeZone();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<HealthTimeController>(out var health))
            {
                health.ExitSafeZone();
            }
        }
    }
}