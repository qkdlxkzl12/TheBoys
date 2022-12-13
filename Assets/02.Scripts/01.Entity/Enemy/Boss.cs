using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Boss : Actor
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

    public Boss(float hp, float attackDamage, string bossName) : base(hp, attackDamage)
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

    override protected void Damaged(float value)
    {
        base.Damaged(value);
        bossSlider.value = curHp;
    }

    public void CreateAward()
    {
        ItemManager.instance.CreateTraitOrb(transform.position);
        ItemManager.instance.CreateExpOrb(transform.position, 20);
        ItemManager.instance.CreateExpOrb(transform.position, 20);
    }

    public void DestroyObject()
    {
        base.OnDie();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        Actor act = col.gameObject.GetComponent<Actor>();
        if (col.CompareTag("Player"))
        {
            AttackTo(act);
        }
    }
}
