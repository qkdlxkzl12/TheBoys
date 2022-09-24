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
        this.hp = 1;
        this.attackDamage = 1;
        this.moveSpeed = 1;
        this.attackDelay = 1;
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
        Actor target = targetObj.GetComponent<Actor>();
        if (target != null)
        {
            this.OnAttack();
            target.GetAttack(null);
        }
        else
        {
            //대상이 Actor타입이 아닐 경우
            Debug.LogError("[ERROR:001]" + targetObj.name + "'s type is not Actor");
        }

    }
    public void OnAttack()
    {
        //특성 등 전달
    }
    public void GetAttack(Func<int> attackType)
    {
        //Action과 Delegate를 이용한 공격시 특수한 효과 구현(더생각해봐야함)
        Damaged(1);
    }
    //데미지를 받음
    public void Damaged(int value_)
    {
        hp -= value_;
        if(hp <= 0)
        {
            OnDie();
        }
    }

    //액터가 죽을 때
    virtual protected void OnDie()
    {
        Destroy(gameObject);
    }

}
