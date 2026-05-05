using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class EnemyMovement : MovementController 
    {
        public void MoveTowards(Vector2 target)
        {
            var direction = (target - (Vector2)transform.position).normalized;
            ApplyMovement(direction);
        }
    }
}