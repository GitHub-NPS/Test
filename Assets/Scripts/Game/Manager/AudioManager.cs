using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager S;

    public AudioSource MusicSource;
    public AudioSource SoundSource;

    private GeneralSave general;

    private void Awake()
    {
        if (!S) S = this;
    }

    private void Start()
    {
        general = DataManager.Save.General;

        MusicSource.volume = general.Music;
        SoundSource.volume = general.Sound;
    }

    public void SetMusic(float value)
    {
        general.SetMusic(value);
        MusicSource.volume = general.Music;

        MuteMusic(general.Music == 0);

        general.Save();
    }

    public void SetSound(float value)
    {
        general.SetSound(value);
        SoundSource.volume = general.Sound;

        MuteSound(general.Sound == 0);

        general.Save();
    }

    private void UnMuteMusic()
    {
        MuteMusic(false);
    }

    private void MuteMusic(bool isMute)
    {
        MusicSource.enabled = !isMute;
    }

    private void MuteSound(bool isMute)
    {
        SoundSource.enabled = !isMute;

        if (isMute) ResourceManager.S.ClearSound();
    }

    private void UnMuteSound()
    {
        MuteSound(false);
    }

    public void PlayOneShoot(AudioClip clip, bool isMusic = true)
    {
        if (!SoundSource.enabled) return;

        SoundSource.PlayOneShot(clip);

        if (!isMusic)
        {
            if (!SoundSource.enabled) return;
            PlayOneShoot(clip);

            if (!MusicSource.enabled) return;
            MuteMusic(true);

            Invoke("UnMuteMusic", clip.length);
        }
    }

    public void PlayOneShoot(string name, bool save = true, bool isMusic = true)
    {
        if (!SoundSource.enabled) return;

        PlayOneShoot(ResourceManager.S.LoadSound(name, save), isMusic);
    }

    public void PlayLoop(AudioClip clip)
    {
        if (!MusicSource.enabled) return;

        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void PlayLoop(string name, bool save = true)
    {
        if (!MusicSource.enabled) return;

        PlayLoop(ResourceManager.S.LoadMusic(name, save));
    }
}