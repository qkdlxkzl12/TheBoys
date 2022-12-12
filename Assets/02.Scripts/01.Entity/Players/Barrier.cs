using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{ 
    public GameObject Me;
    bool On_Off = false; //현재 상태 확인
    public float Shield_Time = 3f; //쉴드 주기
    IEnumerator ReBa;

    void Start()
    {
        GuardRecycle();
    }

    void GuardRecycle()
    {
        ReBa = ReOn();

        if(On_Off == true)
        {
            On_Off = false;
            Me.GetComponent<SpriteRenderer>().enabled = false;
            Me.GetComponent<CircleCollider2D>().enabled = false;
        }
        else
        {
            On_Off = true;
            Me.GetComponent<SpriteRenderer>().enabled = true;
            Me.GetComponent<CircleCollider2D>().enabled = true;
        }

        StartCoroutine(ReBa);
    }

    IEnumerator ReOn()
    {
        yield return new WaitForSeconds(Shield_Time);

        GuardRecycle();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(On_Off == true)
        {
            if (coll.gameObject.CompareTag("EnemyBullet"))
            {
                Destroy(coll.gameObject);

                StopCoroutine(ReBa);
                GuardRecycle();
            }
        }
    }
}
