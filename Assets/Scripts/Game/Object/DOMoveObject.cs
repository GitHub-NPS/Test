using DG.Tweening;
using NPS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DOMoveObject : MonoBehaviour
{
    [SerializeField] private UnityEvent OnComplete;

    public void Set(Vector3 from, Vector3 to, float speed, bool direction = true)
    {
        this.transform.position = from;

        if (direction)
        {
            float angle = MathHelper.Angle(from, to);
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        float dis = MathHelper.SqrMagnitude(from, to);
        dis = Mathf.Sqrt(dis);
        this.transform.DOMove(to, dis / speed).SetEase(Ease.Linear).OnComplete(Complete);
    }

    private void Complete()
    {
        OnComplete?.Invoke();
    }
}
