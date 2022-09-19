using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSprite : MonoBehaviour
{
    [SerializeField] private Material flash;

    private SpriteRenderer sr;
    private Material mat;
    private CoroutineHandle handle;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mat = sr.material;
    }

    private void OnDestroy()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    public void Flash()
    {
        if (handle.IsValid)
        {
            sr.material = mat;
            Timing.KillCoroutines(handle);
        }
        handle = Timing.RunCoroutine(_Flash());
    }

    private IEnumerator<float> _Flash()
    {
        sr.material = flash;

        yield return Timing.WaitForSeconds(0.05f);

        sr.material = mat;

        yield break;
    }
}
