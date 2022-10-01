using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour
{
    protected int hp_;
    protected int attackDamage_;
    protected int moveSpeed_;
    protected float attackDelay_;

    public Actor()
    {
        hp_ = 1;
        attackDamage_ = 1;
        moveSpeed_ = 1;
        attackDelay_ = 1;
    }

    public Actor(int hp, int attackDamage, int moveSpeed, float attackDelay)
    {
        hp_ = hp;
        attackDamage_ = attackDamage;
        moveSpeed_ = moveSpeed;
        attackDelay_ = attackDelay;
    }

    //대상을 공격함
    public void AttackTo(GameObject targetObj)
    {
        Actor target_ = targetObj.GetComponent<Actor>();
        if (target_ != null)
        {
            this.OnAttack();
            target_.GetAttack(null);
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
    protected void Damaged(int value)
    {
        hp_ -= value;
        if(hp_ <= 0)
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
