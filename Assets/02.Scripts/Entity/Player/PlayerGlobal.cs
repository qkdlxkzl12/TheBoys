using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobal : Actor
{

    GameObject Player;

    public int Add_Hp; // 추가 체력

    public float SpeedPower;//추가 이동속도
    public float Add_SpeedPower;// 정규화 보정
    public bool Move_Type; //이동 방식) True : 벡터 , False : 원시적

    float SpeedPower_Diagonal; // 정규화용

    bool DamageTime = false; //피격 데미지 쿨타임용
    
    bool Healing = false; //자동힐 제어용
    IEnumerator Heal;
    bool First_Hit;

    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // 정규화 및 초당 이동량 보정
    }

    private void Start()
    {
        Player = GameManager.instance.player; // 전달
        hp += Add_Hp;
        First_Hit = true;
    }

    private void OnCollisionEnter2D(Collision2D coll) // 피격
    {

        if (coll.gameObject.CompareTag("EnemyBullet"))
        {
            if (DamageTime == false) //피격 데미지 쿨타임 여부
            {

                if(First_Hit == false) //첫 피격 여부
                    StopCoroutine(Heal);
                else
                    First_Hit = false;

                Heal = AutoHeal(); //코루틴 재활용
                StartCoroutine(Heal);//자동 회복 갱신

                DamageTime = true;
                AttackTo(Player);

                StartCoroutine(DamageCoolTime()); //피격 데미지 쿨타임 시작

                Debug.Log(hp);
            }

            Destroy(coll.gameObject); // 총알 삭제
        }

    }

    IEnumerator DamageCoolTime() //피격 데미지 쿨타임
    {
        yield return new WaitForSeconds(0.5f);
        DamageTime = false;
    }

    IEnumerator AutoHeal() //시간 경과 후 회복
    {
        Healing = false;
        yield return new WaitForSeconds(5f);
        Healing = true;
    }

    private void Update()
    {

        if(Healing == true && hp <= 2)
        {
            hp = 3;
            Debug.Log("회복됨");
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log(hp);
        }
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


