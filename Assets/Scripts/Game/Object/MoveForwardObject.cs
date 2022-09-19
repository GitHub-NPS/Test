using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardObject : MonoBehaviour
{
    [SerializeField] private bool enable = false;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float angle = 0;

    private CoroutineHandle handle;

    private void OnEnable()
    {
        if (enable) Move();
    }

    public void Set(float angle, float speed)
    {
        this.angle = angle;
        this.speed = speed;

        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Set(float speed)
    {
        this.speed = speed;
    }

    public void Move()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
        handle = Timing.RunCoroutine(_Move(speed));
    }

    public void Stop()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
        handle = default;
    }

    private void OnDisable()
    {
        if (handle.IsValid) Timing.KillCoroutines(handle);
    }

    private IEnumerator<float> _Move(float speed)
    {
        while (true)
        {
            transform.Translate(Vector3.right * speed * Timing.DeltaTime);
            yield return Timing.DeltaTime;
        }
    }
}
