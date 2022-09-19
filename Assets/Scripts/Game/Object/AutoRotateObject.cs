using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateObject : MonoBehaviour
{
    [SerializeField] private bool enable = false;
    [SerializeField] private float speed = 10f;
    
    private CoroutineHandle handle;
    private Quaternion origin;

    private void Awake()
    {
        origin = this.transform.localRotation;
    }

    private void OnEnable()
    {
        if (enable) AutoRotate();
    }

    public void Set(float speed)
    {
        this.speed = speed;
    }

    public void AutoRotate()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
        handle = Timing.RunCoroutine(_AutoRotate());
    }

    private void OnDisable()
    {
        this.transform.localRotation = origin;
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    private IEnumerator<float> _AutoRotate()
    {
        while (true)
        {
            float v = speed * Timing.DeltaTime * -200;
            this.transform.Rotate(new Vector3() { z = v });

            yield return Timing.DeltaTime;
        }
    }
}
