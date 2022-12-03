using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Boss : Enemy
{
    protected Vector3 initPos { get; } = new Vector3(7, 0, 0);
    private Vector3 comeoutPos { get; } = new Vector3(16, 5, 0);
    protected Sequence comeoutSeq;
    private string bossName;
    private Slider bossSlider;

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
        this.bossName = $"<b>{bossName}</b>";
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
        base.Damaged(value);
        bossSlider.value = curHp;
    }

    protected override void OnDie()
    {
        //애니메이션 실행
        
        //삭제
        base.OnDie();
    }
}
