using UnityEngine;

namespace _Project.Runtime.Core.Traps
{
    public class SpikeTrap : BaseTrap
    {
        public void OnSpikesActionDamage()
            => DealDamageToAll();
            
    }
}