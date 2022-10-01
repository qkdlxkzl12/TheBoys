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
        hp_ -= value;
        if(hp_ <= 0)
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
