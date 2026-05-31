using UnityEngine;
using Zenject;
using _Project.Services.Audio;

public class AudioTest : MonoBehaviour
{
    [Inject] private IAudioService _audio;

    private void Start()
    {
        Debug.Log("AudioTest START");
        _audio.PlaySound(SoundId.CoinTest);
    }
}