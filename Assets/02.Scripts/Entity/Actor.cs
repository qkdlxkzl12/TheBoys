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
    //����� ������
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
    public void Damaged(int value_)
    {
        hp -= value_;
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
