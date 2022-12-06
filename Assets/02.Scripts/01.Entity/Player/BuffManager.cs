using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Buffs {Illusion, Flame}

public class BuffManager : MonoBehaviour
{
    IEnumerator Illu;

    public void DeBuff(Buffs Type,  float Times)
    {

        switch(Type)
        {
            case Buffs.Illusion:
                illusion();
                break;

        }

        void illusion()
        {
            GameManager.instance.player.GetComponent<PlayerGlobal>().Buff_Illusion = true;

            if(Illu != null)
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
                yield return new WaitForSeconds(Times);
                GameManager.instance.player.GetComponent<PlayerGlobal>().Buff_Illusion = false;
                Debug.Log("환각 종료");
                Illu = null;
            }


        }

        
    }
    

}
