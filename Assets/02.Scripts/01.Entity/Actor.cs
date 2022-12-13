using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour
{
    protected float maxHp;
    [HideInInspector]
    public float curHp;
    protected int hp; //�ӽ�
    protected int moveSpeed; //�ӽ�
    protected float attackDamage;

    [HideInInspector]
    public bool Fire; //ȭ�� ����
    [HideInInspector]
    public int FireStack; //ȭ�� ����
    [HideInInspector]
    public bool Recharge; //���� ����

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

    //����� ������
    public void AttackTo(Actor target)
    {
        //����� ActorŸ���� �ƴ� ���
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
        //���� �� �߻��ϴ� �̺�Ʈ �ۼ�(��� �� ���������� ����)
    }

    virtual public void TakeAttack(float damage)
    {
        //������ ������ �߻��ϴ� �̺�Ʈ
        Damaged(damage);
    }

    //�������� ����
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

    //���Ͱ� ���� ��
    virtual protected void OnDie()
    {
            Debug.Log(gameObject.name + "Die");
            Destroy(gameObject);
    }

}


