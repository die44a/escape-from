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

            transform.localPosition = new Vector3(0, 1, 0);
            transform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Отвязывает пивот 
        /// </summary>
        public void Detach()
        {
            if (!this) return;
            transform.SetParent(null);
        }
    }
}
