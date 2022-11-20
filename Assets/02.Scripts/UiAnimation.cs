using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;

[Flags]
enum UiAnimationType 
{
    Fade  = 1 << 0, 
    Bigger   = 1 << 1, 
    Test1 = 1 << 2, 
    Test2 = 1 << 3
}

public class UiAnimation : MonoBehaviour
{
    [SerializeField]
    UiAnimationType type;
    Func<Coroutine> animations = null;
    [SerializeField]
    float durationTime;
    float elpesdTime;
    Tween startTween; 

    bool IsPlaying { get { return elpesdTime <= durationTime; } }
    
    void Awake()
    {
        DOTween.Init();
        InitAnimationEffect();
        startTween.OnComplete(() => Debug.Log("Complete!"));
        //TextMeshProUGUI tMesh = gameObject.GetComponent<TextMeshProUGUI>();
        //DOTween.To(() => "", str => tMesh.text = str, "Hi, welcome to the LucidBook.", 3) .SetLoops(-1,LoopType.Yoyo);
    }
    
    private void OnEnable()
    {
        transform.DOMoveY(250, 5f).SetEase(Ease.OutElastic).From(true);
        startTween.Play();
    }

    void InitAnimationEffect()
    {
        animations = () => StartCoroutine(AddTime());

        if ((type & UiAnimationType.Fade) != 0)
            animations += () => StartCoroutine(FadeAnim());
        if ((type & UiAnimationType.Bigger) != 0)
            animations += () => StartCoroutine(BiggerAnim());
    }

    IEnumerator AddTime()
    {
        elpesdTime = 0;
        while (elpesdTime <= durationTime)
        {
            elpesdTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    IEnumerator FadeAnim()
    {
        while (IsPlaying)
        {
            Debug.Log("test1");
            yield return null;
        }
    }

    IEnumerator BiggerAnim()
    {
        while (IsPlaying)
        {
            Debug.Log("test2");
            yield return null;
        }
    }

}
