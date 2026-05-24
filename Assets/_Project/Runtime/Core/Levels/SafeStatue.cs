using _Project.Runtime.Player.Controllers;
using UnityEngine;

namespace _Project.Runtime.Core.Levels
{
    public class SafeStatue : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<HealthTimeController>(out var health))
            {
                health.EnterSafeZone();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<HealthTimeController>(out var health))
            {
                health.ExitSafeZone();
            }
        }
    }
}