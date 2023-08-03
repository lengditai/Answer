using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LDT.Tweening;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Vector3 Begin;
    [SerializeField]
    private Vector3 End = new Vector3(20f,0f,20f);
    [SerializeField]
    private float Time = 3;
    [SerializeField]
    private bool Pingpong;
    [SerializeField]
    private Ease ease;

    MyTweenCore tweenCore;

    public void Move()
    {
        tweenCore = gameObject.DoMove(Begin, End, Time, Pingpong).SetEase(ease);
    }

    public void Stop()
    {
        MyTween.Kill(tweenCore);
    }
}
