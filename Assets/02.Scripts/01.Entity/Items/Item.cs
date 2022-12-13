using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ItemType { Exp, TraitOrb }
public class Item : Actor
{ 
    ItemType type;
    Transform target;
    Sequence ApearSeq;
    bool isMagnetism;
    public Item(ItemType type) : base()
    {
        this.type = type;
    }
    public void Start()
    {
        target = GameManager.instance.player.transform;
        DOTween.Init();
        ApearSeq = DOTween.Sequence()
            .OnStart(() => { 
                isMagnetism = false;
            } )
            .SetAutoKill(false)
            .Append(transform.DOScale(0.5f, 0.3f).From(false))
            .OnComplete(() => isMagnetism = true );
    }

    private void Update()
    {
        if(isMagnetism == true)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.position += dir * (2.5f + Vector3.Distance(transform.position, target.position) / 2) * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, 70 * Time.deltaTime));
        }
    }

    public override void TakeAttack(float damage)
    {
        Debug.LogError("Item is doesn't take damaged");
    }

    protected void InitAnimPlay()
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.Euler(0,0,Random.Range(0,12) * 30);
        ApearSeq.Restart();
    }
}
