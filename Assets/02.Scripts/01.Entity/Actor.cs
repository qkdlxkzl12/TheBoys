using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour
{
    protected int maxHp;
    protected int curHp;
    protected int hp; //�ӽ�
    protected int moveSpeed; //�ӽ�
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

    //����� ������
    public void AttackTo(Actor target)
    {
        //����� ActorŸ���� �ƴ� ���
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
        //���� �� �߻��ϴ� �̺�Ʈ �ۼ�(��� �� ���������� ����)
    }

    virtual public void TakeAttack(int damage)
    {
        //������ ������ �߻��ϴ� �̺�Ʈ

        if (gameObject.tag == "Player") //?
        {
            PlayerGlobal player = gameObject.GetComponent<PlayerGlobal>();


        }

        Damaged(damage);
    }

    //�������� ����
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

    //���Ͱ� ���� ��
    virtual protected void OnDie()
    {
        if(gameObject.tag == "Player") //�÷��̾� ���� ���� �̺�Ʈ
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


