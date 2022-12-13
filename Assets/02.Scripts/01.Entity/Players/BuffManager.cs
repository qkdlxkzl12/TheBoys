using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Buffs {Illusion, Recharge, Broken, Flame }

public class BuffManager : MonoBehaviour
{
    
    BuffTaker burnTaker;
    BuffTaker rechargeTaker;
    BuffTaker broken;

    public static BuffManager instance;

    IEnumerator Illu;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        burnTaker = transform.GetChild(0).GetComponent<BuffTaker>();
        rechargeTaker = transform.GetChild(1).GetComponent<BuffTaker>();
        broken = transform.GetChild(2).GetComponent<BuffTaker>();
    }

    public void ApplyBuff(Buffs type,float time,  Actor unit)
    {

        switch (type)
        {
            case Buffs.Illusion:
                Illusion(unit);
                break;

            case Buffs.Recharge:
                rechargeTaker.Apply(unit, time);
                break;
            case Buffs.Broken:
                broken.Apply(unit, time);
                break;
            case Buffs.Flame:
                burnTaker.Apply(unit, time);
                break;
        }

        void Illusion(Actor getter)
        {
            if (getter is PlayerGlobal == false)
                return;
            PlayerGlobal player = getter as PlayerGlobal;
            if (Illu != null)
            {
                StopCoroutine(Illu);
                Illu = null;
                Debug.Log("환각 초기화");
            }

            Illu = Ilus();
            StartCoroutine(Illu);
            Debug.Log("환각 시작");

            IEnumerator Ilus()
            {
                player.SwitchMovement(true);
                yield return new WaitForSeconds(time);
                player.SwitchMovement(false);
                Debug.Log("환각 종료");
                Illu = null;
            }
        }
    }

}
