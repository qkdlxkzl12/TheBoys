using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JarfarFace : MonoBehaviour
{
    Sequence EnterSeq;
    SpriteRenderer sr;
    private void Awake()
    {
        DOTween.Init();
        sr = gameObject.GetComponent<SpriteRenderer>();
        EnterSeq = DOTween.Sequence()
            .SetAutoKill(false)
            .Append(transform.DOScale(0, 1f).SetEase(Ease.OutCirc).From(false))
            .Join(sr.DOFade(0, 1f))
            .OnStart(() => {
                transform.localScale = Vector3.one;
                sr.color = new Color(1,1,1,1) ;
            });
            
    }   

    private void OnEnable()
    {
        EnterSeq.Restart();
    }

    private void OnDisable()
    {
    }
}
