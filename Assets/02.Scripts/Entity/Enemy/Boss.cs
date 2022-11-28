using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Boss : Enemy
{
    Vector3 initPos { get; } = new Vector3(7, 0, 0);
    Vector3 comeoutPos { get; } = new Vector3(16, 5, 0);
    protected Sequence comeoutSeq;
    [SerializeField]
    string bossName;
    Slider bossSlider;

    protected virtual void Start()
    {
        DOTween.Init();
        comeoutSeq = DOTween.Sequence().
        SetAutoKill(false).
        OnStart(() =>
        {
            transform.position = initPos;
            gameObject.SetActive(true);
        }).
        Append(transform.DOMoveX(comeoutPos.x, 1f).SetEase(Ease.InQuad).From()).
        Join(transform.DOMoveY(comeoutPos.y, 1f).SetEase(Ease.OutQuad).From()).
        AppendInterval(1).
        OnComplete(() => { StartCoroutine(ChoseAction());});
        InitUiInfo();
    }

    public Boss(int hp, int attackDamage, string bossName) : base(hp, attackDamage)
    {
        this.bossName = bossName;
    }

    protected virtual IEnumerator ChoseAction()
    {
        yield return null;
    }

    public void InitUiInfo()
    {
        bossSlider = UiManager.instance.InitBossUi(bossName, maxHp);
    }

    override protected void Damaged(int value)
    {
        curHp -= value;
        bossSlider.value = curHp;
        if (curHp <= 0)
        {
            OnDie();
        }
    }
}