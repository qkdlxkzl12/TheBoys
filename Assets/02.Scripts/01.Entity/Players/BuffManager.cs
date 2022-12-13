using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Buffs {Illusion, Recharge, Broken, Flame }

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    IEnumerator Illu;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ApplyBuff(Buffs type, float Times, Actor unit)
    {

        switch (type)
        {
            case Buffs.Illusion:
                Illusion(unit);
                break;

            case Buffs.Recharge:
                recharge(unit);
                break;
            case Buffs.Broken:
                recharge(unit);
                break;
            case Buffs.Flame:
                burning(unit);
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
                yield return new WaitForSeconds(Times);
                player.SwitchMovement(false);
                Debug.Log("환각 종료");
                Illu = null;
            }
        }

        void recharge(Actor getter)
        {
            if (getter is Enemy == false)
                return;
            PlayerGlobal player = getter as PlayerGlobal;
        }

        void burning(Actor getter)
        {
            if (getter != null)
            {
                getter.GetComponent<Actor>().Fire = true;
            }
        }
    }

    void Fire_Recycling()
    {

    }

    private void Update()
    {
        
    }

}
