using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Buffs {Illusion, Recharge, Flame}

public class BuffManager : MonoBehaviour
{
    IEnumerator Illu;

    public void DeBuff(Buffs Type, float Times)
    {

        switch (Type)
        {
            case Buffs.Illusion:
                illusion();
                break;

            case Buffs.Recharge:
                recharge();
                break;
        }

        void illusion()
        {
            GameManager.instance.player.GetComponent<PlayerGlobal>().Buff_Illusion = true;

            if (Illu != null)
            {
                StopCoroutine(Illu);
                Illu = null;
                Debug.Log("ȯ�� �ʱ�ȭ");
            }

            Illu = Ilus();
            StartCoroutine(Illu);
            Debug.Log("ȯ�� ����");

            IEnumerator Ilus()
            {
                yield return new WaitForSeconds(Times);
                GameManager.instance.player.GetComponent<PlayerGlobal>().Buff_Illusion = false;
                Debug.Log("ȯ�� ����");
                Illu = null;
            }
        }

        void recharge()
        {
            GameObject[] Enemy_Bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

            /*Actor[]

            foreach 
            */


        }

    }

    private void Update()
    {
        
    }

}
