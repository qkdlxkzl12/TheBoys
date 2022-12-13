using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour
{
    protected float maxHp;
    [HideInInspector]
    public float curHp;
    protected int hp; //임시
    protected int moveSpeed; //임시
    protected float attackDamage;

    [HideInInspector]
    public bool Fire; //화상 버프
    [HideInInspector]
    public int FireStack; //화상 스택
    [HideInInspector]
    public bool Recharge; //충전 버프

    public Actor()
    {
        maxHp = 5;
        curHp = maxHp;
        attackDamage = 1;
    }

    public Actor(float hp, float attackDamage)
    {
        maxHp = hp;
        curHp = hp;
        this.attackDamage = attackDamage;
    }
    public void AttackTo(GameObject test) { }

    //대상을 공격함
    public void AttackTo(Actor target)
    {
        //대상이 Actor타입이 아닐 경우
        if (target == null)
        {
            Debug.LogError("[ERROR:001]" + target.name + "'s type is not Actor");
            return;
        }
        this.OnAttack(target);
        target.TakeAttack(this.attackDamage);
    }
    virtual public void OnAttack(Actor target)
    {
        //공격 시 발생하는 이벤트 작성(상속 후 내부적으로 구현)
    }

    virtual public void TakeAttack(float damage)
    {
        //공격을 받으면 발생하는 이벤트
        Damaged(damage);
    }

    //데미지를 받음
    virtual protected void Damaged(float value)
    {
        curHp -= value;
        if(curHp <= 0)
        {
            curHp = 0;
            OnDie();
        }

    }

    protected void Healing(int value)
    {
        curHp += value;
        if(curHp > maxHp)
            curHp = maxHp;
    }

    //액터가 죽을 때
    virtual protected void OnDie()
    {
            Debug.Log(gameObject.name + "Die");
            Destroy(gameObject);
    }

}


