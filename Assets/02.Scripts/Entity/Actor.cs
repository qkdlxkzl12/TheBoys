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

    //����� ������
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
            //����� ActorŸ���� �ƴ� ���
            Debug.LogError("[ERROR:001]" + targetObj.name + "'s type is not Actor");
        }

    }

    virtual public void OnAttack()
    {
        //���� �� �߻��ϴ� �̺�Ʈ �ۼ�(��� �� ���������� ����)
    }

    virtual public void TakeAttack(int damage)
    {
        //������ ������ �߻��ϴ� �̺�Ʈ
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
        Debug.Log(gameObject.name + "Die");
        Destroy(gameObject);
    }

}
