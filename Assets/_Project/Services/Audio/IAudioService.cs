namespace _Project.Services.Audio
{
    public interface IAudioService
    {
        void PlaySound(SoundId soundId, float volumeScale = 1f);
        void PlayUISound(SoundId soundId, float volumeScale = 1f);

        void PlayMusic(MusicId musicId, bool loop = true);
        void StopMusic();

        void SetMasterVolume(float volume);
        void SetMusicVolume(float volume);
        void SetSfxVolume(float volume);
        void SetUiVolume(float volume);

        float GetMasterVolume();
        float GetMusicVolume();
        float GetSfxVolume();
        float GetUiVolume();
        void PlayFootstep();
        void PlayDash();
    }
}