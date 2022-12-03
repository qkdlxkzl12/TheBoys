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
        this.OnAttack();
        target.TakeAttack(this.attackDamage);
    }

    virtual public void OnAttack()
    {
        //공격 시 발생하는 이벤트 작성(상속 후 내부적으로 구현)
    }

    virtual public void TakeAttack(int damage)
    {
        //공격을 받으면 발생하는 이벤트

        if (gameObject.tag == "Player") //?
        {
            PlayerGlobal player = gameObject.GetComponent<PlayerGlobal>();


        }

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
        if(gameObject.tag == "Player") //플레이어 전용 데드 이벤트
        {
            gameObject.GetComponent<PlayerGlobal>().Dead_Event();
        }   
        else
        {
            Debug.Log(gameObject.name + "Die");
            Destroy(gameObject);
        }
    }



}


