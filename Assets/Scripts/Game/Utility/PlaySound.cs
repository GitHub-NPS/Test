using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private bool enable = false;
    [SerializeField] private SoundData sound;

    private void OnEnable()
    {
        if (enable) Play();
    }

    public void Play()
    {
        if (DataManager.Save.General.Sound == 0) return;

        if (!sound.Clip && !string.IsNullOrEmpty(sound.NameClip) && ResourceManager.S) sound.Clip = ResourceManager.S.LoadSound(sound.NameClip, sound.Save);
        if (AudioManager.S && sound.Clip) AudioManager.S.PlayOneShoot(sound.Clip, sound.Music);
    }

    public void Play(SoundData sound)
    {
        this.sound = sound;
        Play();
    }
}

[System.Serializable]
public class SoundData
{
    public AudioClip Clip;
    public string NameClip = "Btn_Click";
    public bool Save = true;
    public bool Music = true;
}
