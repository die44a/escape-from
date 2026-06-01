using System;
using UnityEngine;

namespace _Project.Services.Audio.Database
{
    [Serializable]
    public abstract class AudioEntry
    {
        public AudioClip[] Clips;
        public float DefaultVolume = 1f;
    }
}