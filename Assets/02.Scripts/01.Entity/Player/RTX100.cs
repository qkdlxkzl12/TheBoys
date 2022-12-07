using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RTX100 : MonoBehaviour
{
    public GameObject me;

    public GameObject player;       //기준행성 (토성)
    public float speed;             //회전 속도

    public GameObject RTX_Bullets;
    public float Shooting_Time;

    public float Broken_Time;

    bool turnOn_1 = false;
    bool turnOn_2 = false;

    void Start()
    {
        player = GameManager.instance.player.gameObject;
        Shooting_Function();

        if (player.GetComponent<PlayerGlobal>().synergy == Synergy.고장)
        {
            turnOn_2 = true;
            Broken_Synergy();
        }
    }

    void Update()
    {
        if(turnOn_1 == false)
            Shooting_Function();

        if (player.GetComponent<PlayerGlobal>().synergy == Synergy.고장 && turnOn_2 == false)
        {
            turnOn_2 = true;
            Broken_Synergy();
        }
    }

    void FixedUpdate()
    { 
        OrbitAround();
    }

    private void OnTriggerEnter2D(Collider2D coll)  // 피격 혹은 획득
    {
        if (coll.gameObject.CompareTag("EnemyBullet"))
            Destroy(coll.gameObject);
    }

    void Shooting_Function()
    {
        turnOn_1 = true;
        IEnumerator Shot()
        {
            yield return new WaitForSeconds(Shooting_Time);

            GameObject RB = Instantiate(RTX_Bullets, new Vector3 (me.transform.position.x, me.transform.position.y, me.transform.position.z), Quaternion.identity);

            RB.GetComponent<SpriteRenderer>().color = new Color32(16, 16, 16, 255);

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

    void OrbitAround()
    {
        transform.RotateAround(player.transform.position, Vector3.back, speed * Time.deltaTime);
    }

}
