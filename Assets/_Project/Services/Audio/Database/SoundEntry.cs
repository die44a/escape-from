using System;
using UnityEngine;

namespace _Project.Services.Audio.Database
{
    [Serializable]
    public class SoundEntry : AudioEntry
    {
        public SoundId Id;

        [Header("Pitch randomization")]
        [Range(0f, 0.3f)]
        public float PitchVariance = 0.05f;
    }
}