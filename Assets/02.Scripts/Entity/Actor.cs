using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour
{
    protected int hp;
    protected int attackDamage;
    protected int moveSpeed;
    protected float attackDelay;

    public Actor()
    {
        hp = 1;
        attackDamage = 1;
        moveSpeed = 1;
        attackDelay = 1;
    }

    public Actor(int hp, int attackDamage, int moveSpeed, float attackDelay)
    {
        this.hp = hp;
        this.attackDamage = attackDamage;
        this.moveSpeed = moveSpeed;
        this.attackDelay = attackDelay;
    }

    //대상을 공격함
    public void AttackTo(GameObject targetObj)
    {
        Actor target_ = targetObj.GetComponent<Actor>();
        if (target_ != null)
        {
            this.OnAttack();
            target_.GetAttack();
        }
        else
        {
            //대상이 Actor타입이 아닐 경우
            Debug.LogError("[ERROR:001]" + targetObj.name + "'s type is not Actor");
        }

    }

    public void OnAttack()
    {
        //특성 등 전달(내부적으로 상속으로 구현)
    }

    public void GetAttack()
    {
        //피격 받을 시 데미지 말고 다른 것이 있으면 내부적으로 상속으로 구현)
        Damaged(1);
    }

    //데미지를 받음
    protected void Damaged(int value)
    {
        hp -= value;
        if(hp <= 0)
        {
            OnDie();
        }
    }

    //액터가 죽을 때
    virtual protected void OnDie()
    {
        Debug.Log(gameObject.name + "Die");
        Destroy(gameObject);
    }

}
