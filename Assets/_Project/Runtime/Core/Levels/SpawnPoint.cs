using UnityEngine;

namespace _Project.Runtime.Core.Levels
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Color gizmoColor = Color.green;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, 0.5f);
            
            Gizmos.DrawRay(transform.position, transform.up * 1f); 
        }
    }
}