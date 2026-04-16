using UnityEngine;

namespace _Project.Runtime.Core.Camera
{
    public class CameraPivotController : MonoBehaviour
    {
        /// <summary>
        /// Привязывает pivot к объекту, делая его дочерним.
        /// </summary>
        public void AttachTo(GameObject target)
        {
            if (!target) return;

            transform.SetParent(target.transform);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Отвязывает пивот 
        /// </summary>
        public void Detach()
        {
            transform.SetParent(null);
        }
    }
}
