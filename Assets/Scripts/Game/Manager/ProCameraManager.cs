using Com.LuisPedroFonseca.ProCamera2D;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProCameraManager : MonoBehaviour
{
    public static ProCameraManager S;
    public Camera Camera => cam;

    private ProCamera2D m_Camera;
    private ProCamera2DShake m_Camera_Shake;

    private Camera cam;

    private void Awake()
    {
        if (!S) S = this;

        cam = Camera.main;
        m_Camera = cam.GetComponent<ProCamera2D>();
        m_Camera_Shake = m_Camera.GetComponent<ProCamera2DShake>();
    }

    private void Start()
    {
        
    }

    public void AddFollowTarget(Transform target, float targetInfluenceH = 1, float targetInfluenceV = 1, float duration = 0, Vector2 targetOffset = default(Vector2))
    {
        if (m_Camera == null) return;
        m_Camera.AddCameraTarget(target, targetInfluenceH, targetInfluenceV, duration, targetOffset);
    }

    public void RemoveFollowTarget(Transform target, float duration = 0)
    {
        if (m_Camera == null) return;
        m_Camera.RemoveCameraTarget(target, duration);
    }

    public void FollowTime(Transform target, float timeGo, float timeBack, float timeStay = 0f)
    {
        StartCoroutine(_FollowTime(target, timeGo, timeBack, timeStay));
    }

    private IEnumerator _FollowTime(Transform target, float timeGo, float timeBack, float timeStay = 0f)
    {
        var oldTarget = new List<CameraTarget>();
        foreach (var item in m_Camera.CameraTargets)
        {
            oldTarget.Add(item);
        }

        m_Camera.RemoveAllCameraTargets();
        AddFollowTarget(target, 1, 1, timeGo);

        yield return new WaitForSeconds(timeGo + timeStay);

        RemoveFollowTarget(target, timeBack);
        yield return new WaitForSeconds(timeBack);
        m_Camera.CameraTargets = oldTarget;
    }

    public void Shake(string name)
    {
        m_Camera_Shake?.Shake(name);
    }

    public void Zoom(float size, float time = 0)
    {
        return;

        if(time != 0) cam.DOOrthoSize(size, time);
        else cam.orthographicSize = size;
    }
}
