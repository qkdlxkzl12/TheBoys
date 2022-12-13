using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class BuffTaker : MonoBehaviour
{
    public BuffTaker(bool isStackable, float delay = 0)
    {
        this.isStackable = isStackable;
        this.delay = delay;
    }
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    Buffs type;
    [SerializeField]
    bool isStackable;
    [SerializeField]
    float delay;
    private List<Actor> getter;
    private List<int> stackCnt;
    Action<int> execute;
    float curTime;

    public void Awake()
    {
        getter = new List<Actor>();
        stackCnt = new List<int>();
        switch (type)
        {
            case Buffs.Illusion:
                break;
            case Buffs.Recharge:
                execute = (index) => StartCoroutine(Recharge(getter[index]));
                break;
            case Buffs.Broken:
                execute = (index) => StartCoroutine(Broken(getter[index]));
                break;
            case Buffs.Flame:
                //�������̱� ������ curTime�� �������� ���� �ٲ�� �ȵǼ� ���� ���� �Ѱ���
                execute = (index) => StartCoroutine(Burn(getter[index],curTime));
                break;
            default:
                break;
        }
    }

    public void Apply(Actor act,float duration)
    {
        int thisIndex = 0;
        if (getter.Contains(act) == true)
        {
            thisIndex = getter.IndexOf(act);
        }
        else
        {
            getter.Add(act);
            if (isStackable == true)
                stackCnt.Add(0);
            thisIndex = getter.IndexOf(act);
        }

        if (isStackable == true)
        {
            //�������̶�� ���� �����ְ� ���� �ð���� ����
            curTime = duration;
            if (execute != null)
                execute(thisIndex);
        }
        else
        {
            //���� �����ϰų� �ð��� ��������
            if(curTime <= 0)
            {
                if (curTime < duration)
                    curTime = duration;
                if (getter[thisIndex] != null)
                    execute(thisIndex);
            }
            else
            {
                if (curTime < duration)
                    curTime = duration;
            }

        }
    }

    IEnumerator Burn(Actor act,float holdTime)
    {
        Actor damagerGiver = new Actor(0, 3);
        int index = getter.IndexOf(act);
        stackCnt[index]++;
        Debug.Log($"Burn Stack is {stackCnt[index]}");
        //������ �ְ� 3�����̸� 3���� ���ְ� ����. �׸��� 0�̶�� ����
        if (CharacteristicManager.instance.isAwakenFire == true && stackCnt[index] >= 3)
        {
            stackCnt[index] = 0;
            damagerGiver = new Actor(0, 50);
            damagerGiver.AttackTo(act);
            Debug.Log($"���� ������! {stackCnt[index]}");
            var particle = Instantiate(explosion,act.gameObject.transform.position,Quaternion.identity);
            Destroy(particle,0.7f);
            if (getter.Contains(act) == true)
            {
                index = getter.IndexOf(act);
                getter.Remove(act);
                stackCnt.RemoveAt(index);
            }
            yield break;
        }
        //����â�� ȭ���̹��� ����
        while (holdTime > 0)
        {
            yield return new WaitForSeconds(delay);
            if(getter.Contains(act) == false)
            {
                yield break;
            }
            if(act == null)
            {
                //�߰��� ����� �װų� ������ٸ� ����Ʈ�� ���� �� ��������
                index = getter.IndexOf(act);
                getter.Remove(act);
                stackCnt.RemoveAt(index);
                yield break;
            }
            holdTime -= delay;
            damagerGiver.AttackTo(act);
        }
        if (getter.Contains(act) == true)
        {
            index = getter.IndexOf(act);
            stackCnt[index]--;
        }
        //������ ������ ��� ����Ʈ���� ����
        if (stackCnt[index] == 0)
        {
            if (getter.Contains(act) == true)
            {
                index = getter.IndexOf(act);
                getter.Remove(act);
                stackCnt.RemoveAt(index);
            }
        }
        //����â�� ȭ�� �̹��� ����
    }

    IEnumerator Recharge(Actor act)
    {
        act.SetAttackSpeed(0.8f);
        while (curTime > 0)
        {
            yield return null;
            curTime -= Time.deltaTime;
        }
        act.SetAttackSpeed(1f);
    }


    IEnumerator Broken(Actor act)
    {
        Debug.Log("���� ����");
        act.SetBroken(true);
        while (curTime > 0)
        {
            yield return null;
            curTime -= Time.deltaTime;
        }
        act.SetBroken(false);
    }
}