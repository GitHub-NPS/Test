using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWaitting : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private RectTransform progress;

    private float speed = -300f;
    private CoroutineHandle handleRotate;
    private CoroutineHandle handleKill;

    public void Show(float exit = -1)
    {
        if (handleKill.IsValid) Timing.KillCoroutines(handleKill);
        if (handleRotate.IsValid) Timing.KillCoroutines(handleRotate);

        //progress.rotation = Quaternion.Euler(Vector3.zero);        
        handleRotate = Timing.RunCoroutine(_Rotate());
        content.SetActive(true);

        if (exit > 0) handleKill = Timing.RunCoroutine(_Kill(exit));
    }

    private IEnumerator<float> _Kill(float time)
    {
        yield return Timing.WaitForSeconds(time);

        Hide();
    }

    private IEnumerator<float> _Rotate()
    {
        while (true)
        {
            progress.Rotate(0f, 0f, speed * Timing.DeltaTime);
            yield return Timing.WaitForOneFrame;
        }
    }

    public void Hide()
    {
        if (handleKill.IsValid) Timing.KillCoroutines(handleKill);
        if (handleRotate.IsValid) Timing.KillCoroutines(handleRotate);
        content.SetActive(false);
    }
}
