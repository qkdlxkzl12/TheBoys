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
                //스택형이기 때문에 curTime이 동적으로 값이 바뀌면 안되서 현재 값을 넘겨줌
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
            //스택형이라면 스택 더해주고 각자 시간대로 시작
            curTime = duration;
            if (execute != null)
                execute(thisIndex);
        }
        else
        {
            //새로 시작하거나 시간을 갱신해줌
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
        //각성이 있고 3스택이면 3스택 빼주고 폭발. 그리고 0이라면 종료
        if (CharacteristicManager.instance.isAwakenFire == true && stackCnt[index] >= 3)
        {
            stackCnt[index] = 0;
            damagerGiver = new Actor(0, 50);
            damagerGiver.AttackTo(act);
            Debug.Log($"폭발 데미지! {stackCnt[index]}");
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
        //상태창에 화상이미지 띄우기
        while (holdTime > 0)
        {
            yield return new WaitForSeconds(delay);
            if(getter.Contains(act) == false)
            {
                yield break;
            }
            if(act == null)
            {
                //중간에 대상이 죽거나 사라진다면 리스트에 삭제 후 강제종료
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
        //마지막 스택일 경우 리스트에서 삭제
        if (stackCnt[index] == 0)
        {
            if (getter.Contains(act) == true)
            {
                index = getter.IndexOf(act);
                getter.Remove(act);
                stackCnt.RemoveAt(index);
            }
        }
        //상태창에 화상 이미지 삭제
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
        Debug.Log("고장 시작");
        act.SetBroken(true);
        while (curTime > 0)
        {
            yield return null;
            curTime -= Time.deltaTime;
        }
        act.SetBroken(false);
    }
}