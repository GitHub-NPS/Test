using System;
using MEC;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTutorial : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic ga;
    [SerializeField] private float speed = 5;
    [SerializeField] private GameObject fx;

    private void Awake()
    {
        ga.Initialize(false);
    }

    public void Set(bool isUI, HandType type)
    {        
        ga.AnimationState.SetAnimation(0, Constant.HandType2Anim[type], true);
    }

    public IEnumerator<float> _Move(Vector3 posEnd, Vector3 posStart, bool isLoop = false)
    {
        this.transform.position = posStart;
        fx.SetActive(true);

        while (true)
        {
            float step = speed * Time.deltaTime;
            this.gameObject.transform.position = Vector3.MoveTowards(transform.position, posEnd, step);

            if (Vector3.Distance(transform.position, posEnd) < 0.001f)
            {
                if (isLoop)
                {
                    fx.SetActive(false);
                    this.transform.position = posStart;
                    fx.SetActive(true);
                }
                else break;
            }

            yield return Timing.WaitForOneFrame;
        }
    }
}