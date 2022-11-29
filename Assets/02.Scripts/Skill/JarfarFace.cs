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
            .OnStart(() => {
                transform.localScale = Vector3.one;
                sr.color = new Color(1, 1, 1, 1);
            })
            .Append(transform.DOScale(0, 1.5f).SetEase(Ease.OutCirc).From(false))
            .Join(sr.DOFade(0, 1f))
            .OnComplete(() => gameObject.SetActive(false) );
            
    }   

    private void OnEnable()
    {
        EnterSeq.Restart();
    }

    public void Use()
    {
        gameObject.SetActive(true);
    }
    
}
