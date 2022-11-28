using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour
{
    protected int maxHp;
    protected int curHp;
    protected int hp; //임시
    protected int moveSpeed; //임시
    protected int attackDamage;

    public Actor()
    {
        maxHp = 5;
        curHp = maxHp;
        attackDamage = 1;
    }

    public Actor(int hp, int attackDamage)
    {
        maxHp = hp;
        curHp = maxHp;
        this.attackDamage = attackDamage;
    }

    //대상을 공격함
    public void AttackTo(GameObject targetObj)
    {
        Actor target_ = targetObj.GetComponent<Actor>();
        if (target_ != null)
        {
            this.OnAttack();
            target_.TakeAttack(this.attackDamage);
        }
        else
        {
            //대상이 Actor타입이 아닐 경우
            Debug.LogError("[ERROR:001]" + targetObj.name + "'s type is not Actor");
        }

    }

    virtual public void OnAttack()
    {
        //공격 시 발생하는 이벤트 작성(상속 후 내부적으로 구현)
    }

    virtual public void TakeAttack(int damage)
    {
        //공격을 받으면 발생하는 이벤트
        Damaged(damage);
    }

    //데미지를 받음
    virtual protected void Damaged(int value)
    {
        curHp -= value;
        if(curHp <= 0)
        {
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
