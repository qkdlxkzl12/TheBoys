using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobal : Actor
{
    GameObject Player;

    public int Add_HP;
    public float SpeedPower;//추가 이동속도
    public float Add_SpeedPower;// 정규화 보정
    public bool Move_Type; //이동 방식) True : 벡터 , False : 원시적

    float SpeedPower_Diagonal; // 정규화용

    bool DamageTime = false; //피격 데미지 쿨타임용
    bool Healing = true;

    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // 정규화 및 초당 이동량 보정
    }

    private void Start()
    {
        Player = GameManager.instance.player; // 전달
        hp += Add_HP;
    }

    private void Update()
    {

        if (Healing == true && hp <= 2)
        {
            if (hp == 1)
                hp += 2;
            else if (hp == 2)
                hp += 1;

            Debug.Log("회복됨 : " + hp);
        }

    }

    private void OnTriggerEnter2D(Collider2D coll)  // 피격
    {
        if (coll.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(coll.gameObject);
            Hit_System();
        }
    }

    void DamageCoolTime() //피격 데미지 쿨타임용
    {
        DamageTime = false;
        Debug.Log("무적시간 비활성화");
    }

    void Heal() //힐 용
    {
        Healing = true;
    }

    void Hit_System()
    {
        if (DamageTime == false) //피격 데미지 쿨타임 여부
        {
            AttackTo(Player);
            Debug.Log(hp);

            CancelInvoke("Heal");
        }

        Debug.Log("무적시간 활성화");
        DamageTime = true;  //데미지 쿨타임 시작
        Invoke("DamageCoolTime", 0.5f);

        Healing = false;
        Invoke("Heal", 5f);
    }

    void FixedUpdate() // 이동
    {
        if (Move_Type)
        {
            Vector2 Vectors = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vectors.Normalize();
            transform.Translate(Vectors * (SpeedPower_Diagonal) * Time.deltaTime);
        }

        else
        {
             Regular_Move();
        }
    }

    void Regular_Move()
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


