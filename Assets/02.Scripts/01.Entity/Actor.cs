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
    protected float moveSpeed; //�ӽ�
    protected float attackDamage;
    protected float increaseAttackSpeed = 1;
    protected bool isBroken;

    public Actor()
    {
        maxHp = 5;
        curHp = maxHp;
        attackDamage = 1;
        increaseAttackSpeed = 1;
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
            Destroy(gameObject);
    }

    public void SetAttackSpeed(float value = -1)
    {
        if (value < 0)
            increaseAttackSpeed = 1;
        else
            increaseAttackSpeed = value;
    }

    public void SetBroken(bool value = false)
    {
        isBroken = value;
    }
}


