using UnityEngine;

namespace _Project.Runtime.Core.Configs
{
    [CreateAssetMenu(fileName = "CoinsConfig", menuName = "Configs/CoinsConfig")]
    public class CoinsConfig : ScriptableObject
    {
        public int bronzeValue = 1;
        public int silverValue = 5;
        public int goldValue = 10;
    }
}