using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RTX100 : MonoBehaviour
{
    public GameObject me;

    public GameObject player;
    public float speed;

    public GameObject RTX_Bullets;
    public float Shooting_Time;
    public bool Can_Shoot = false;

    public float Broken_Time;

    bool turnOn_1 = false;
    bool turnOn_2 = false;

    void Start()
    {
        player = GameManager.instance.player.gameObject;
        Shooting_Function();

        if (player.GetComponent<PlayerGlobal>().synergy == Synergy.∞Ì¿Â)
        {
            turnOn_2 = true;
            Broken_Synergy();
        }

        Targeting();
    }

    void Update()
    {
        if(turnOn_1 == false)
            Shooting_Function();

        if (player.GetComponent<PlayerGlobal>().synergy == Synergy.∞Ì¿Â && turnOn_2 == false)
        {
            turnOn_2 = true;
            Broken_Synergy();
        }

    }

    void FixedUpdate()
    { 
        Targeting();
    }

    private void OnTriggerEnter2D(Collider2D coll)  // ««∞› »§¿∫ »πµÊ
    {
        if (coll.gameObject.CompareTag("EnemyBullet"))
            Destroy(coll.gameObject);
    }

    void Targeting()
    {
        GameObject[] E_Targets = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject Boss = GameObject.FindGameObjectWithTag("Boss");

        transform.RotateAround(player.transform.position, Vector3.back, speed * Time.deltaTime);

        if (E_Targets.Length >= 1)
        {
            Can_Shoot = true;

            if (Boss != null)
            {
                E_Targets[0] = Boss;
            }

            Vector3 d = E_Targets[0].transform.position - me.transform.position;

            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

            me.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
            Can_Shoot = false;
    }

    void Shooting_Function()
    {
        turnOn_1 = true;
        IEnumerator Shot()
        {
            yield return new WaitForSeconds(Shooting_Time);

            if(Can_Shoot)
                Instantiate(RTX_Bullets, new Vector3 (me.transform.position.x, me.transform.position.y, me.transform.position.z), me.transform.rotation);

            StartCoroutine(Shot());
        }

        StartCoroutine(Shot());
    }

    void Broken_Synergy()
    {
        IEnumerator Broke()
        {
            yield return new WaitForSeconds(Broken_Time);

            GameObject[] Enemy_Bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

            if(Enemy_Bullets != null)
            {
               int list = Random.Range(0, Enemy_Bullets.Length - 1);

                GameObject EB = Enemy_Bullets[list];

                EB.GetComponent<CircleCollider2D>().enabled = false;
                EB.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 32);
            }

            StartCoroutine(Broke());
        }
        StartCoroutine(Broke());
    }

}
