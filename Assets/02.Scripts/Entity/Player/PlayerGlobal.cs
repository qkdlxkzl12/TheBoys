using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobal : Actor
{

    public GameObject Player;

    public int Add_Hp; // 추가 체력

    public float SpeedPower;//추가 이동속도
    public float Add_SpeedPower;// 정규화 보정

    float SpeedPower_Diagonal; // 정규화용

    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // 정규화 및 초당 이동량 보정
    }

    private void Start()
    {
        hp += Add_Hp;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.CompareTag("EnemyBullet"))
        {
            Damaged(1);
            Destroy(coll.gameObject);
        }

    }

    void FixedUpdate() // 이동
    {
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow)) //북서
        {
            Player.transform.Translate(-(moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, (moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)) //북동
        {
            Player.transform.Translate((moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, (moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)) // 남동
        {
            Player.transform.Translate((moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, -(moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)) //남서
        {
            Player.transform.Translate(-(moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, -(moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.UpArrow)) //위
        {
            Player.transform.Translate(0, (moveSpeed * SpeedPower) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.LeftArrow)) //왼
        {
            Player.transform.Translate(-(moveSpeed * SpeedPower) * Time.deltaTime, 0, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow)) //아래
        {
            Player.transform.Translate(0, -(moveSpeed * SpeedPower) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.RightArrow)) //오른
        {
            Player.transform.Translate((moveSpeed * SpeedPower) * Time.deltaTime, 0, 0);
        }

        else
        {

        }
    }
}


