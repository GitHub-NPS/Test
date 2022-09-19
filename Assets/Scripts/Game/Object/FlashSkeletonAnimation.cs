using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSkeletonAnimation : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void Flash(float time)
    {
        Flash(time, Color.white);
    }

    public void Flash(float time, Color color)
    {
        Timing.RunCoroutine(_Flash(time, color));
    }

    private IEnumerator<float> _Flash(float time, Color color)
    {
        int fillPhase = Shader.PropertyToID("_FillPhase");
        int fillColor = Shader.PropertyToID("_FillColor");

        mpb.SetFloat(fillPhase, 1);
        if (mr) mr.SetPropertyBlock(mpb);
        mpb.SetColor(fillColor, color);

        yield return Timing.WaitForSeconds(time);

        if (mpb != null) mpb.SetFloat(fillPhase, 0);
        if (mr) mr.SetPropertyBlock(mpb);

        yield break;
    }
}
