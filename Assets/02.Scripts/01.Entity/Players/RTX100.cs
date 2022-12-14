using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RTX100 : Actor
{
    public GameObject me;

    public GameObject player;
    public float speed;

    public GameObject RTX_Bullets;
    public float Shooting_Time;
    public bool Can_Shoot = false;

    void Start()
    {
        player = GameManager.instance.player.gameObject;
        Shooting_Function();

        Targeting();
    }


    void Update()
    { 
        Targeting();
    }

    private void OnTriggerEnter2D(Collider2D coll)  // ÇÇ°Ý È¤Àº È¹µæ
    {
        if (coll.gameObject.CompareTag("EnemyBullet"))
            Debug.Log("ÃÑ¾Ë »èÁ¦");
    }

    void Targeting()
    {
        GameObject[] E_Targets = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject Boss = GameObject.FindGameObjectWithTag("Boss");

        transform.RotateAround(player.transform.position, Vector3.back, speed * Time.deltaTime);

        if (E_Targets.Length >= 1)
        {
            Can_Shoot = true;

            Vector3 d = E_Targets[0].transform.position - me.transform.position;

            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

            me.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (Boss != null)
        {
            Can_Shoot = true;

            Vector3 d = Boss.transform.position - me.transform.position;

            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

            me.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
            Can_Shoot = false;
    }

    void Shooting_Function()
    {
        IEnumerator Shot()
        {
            yield return new WaitForSeconds(Shooting_Time);

            if(Can_Shoot)
                Instantiate(RTX_Bullets, new Vector3 (me.transform.position.x, me.transform.position.y, me.transform.position.z), me.transform.rotation);

            StartCoroutine(Shot());
        }

        StartCoroutine(Shot());
    }

}
