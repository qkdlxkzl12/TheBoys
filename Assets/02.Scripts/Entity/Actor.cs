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

    //����� ������
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
            //����� ActorŸ���� �ƴ� ���
            Debug.LogError("[ERROR:001]" + targetObj.name + "'s type is not Actor");
        }

    }

    public void OnAttack()
    {
        //Ư�� �� ����
    }

    public void GetAttack(Func<int> attackType)
    {
        //Action�� Delegate�� �̿��� ���ݽ� Ư���� ȿ�� ����(�������غ�����)
        Damaged(1);
    }

    //�������� ����
    protected void Damaged(int value)
    {
        hp -= value;
        if(hp <= 0)
        {
            OnDie();
        }
    }

    //���Ͱ� ���� ��
    virtual protected void OnDie()
    {
        Destroy(gameObject);
    }

}
