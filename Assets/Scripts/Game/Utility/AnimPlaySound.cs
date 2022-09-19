using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimPlaySound : MonoBehaviour
{
    [SerializeField] private List<AnimSoundData> animSound;
    [SerializeField] private SkeletonAnimation sa;
    [SerializeField] private SkeletonGraphic sg;

    private PlaySound play;
    private Dictionary<string, AnimSoundData> dic = new Dictionary<string, AnimSoundData>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        sg = GetComponent<SkeletonGraphic>();
        sa = GetComponent<SkeletonAnimation>();
    }
#endif

    private void Awake()
    {
        play = GetComponent<PlaySound>();

        if (sa) sa.AnimationState.Event += OnEventSA;

        if (sg) sg.AnimationState.Event += OnEventSG;

        dic.Clear();
        for (int i = 0; i < animSound.Count; i++)
        {
            string key = animSound[i].Anim + "-" + animSound[i].Event;
            if (!dic.ContainsKey(key)) dic.Add(key, animSound[i]);
        }
    }

    private void OnEventSG(TrackEntry trackEntry, Spine.Event e)
    {
        OnEvent(trackEntry, e);
    }

    private void OnEventSA(TrackEntry trackEntry, Spine.Event e)
    {
        OnEvent(trackEntry, e);
    }

    private void OnDestroy()
    {
        if (sa) sa.AnimationState.Event -= OnEventSA;

        if (sg) sg.AnimationState.Event -= OnEventSG;
    }

    private void OnEvent(TrackEntry trackEntry, Spine.Event e)
    {
        string key = trackEntry.Animation.Name + "-" + e.Data.Name;
        if (dic.ContainsKey(key) && AllowPlaySound(trackEntry, e)) play.Play(dic[key].Sound);
    }

    protected virtual bool AllowPlaySound(TrackEntry trackEntry, Spine.Event e)
    {
        return true;
    }    
}

[System.Serializable]
public class AnimSoundData
{
    public string Anim;
    public string Event;
    public SoundData Sound;
}
