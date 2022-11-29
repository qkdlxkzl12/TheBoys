using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGlobal : Actor
{
    GameObject Player;

    //기본
    public int Add_HP; //추가 체력
    public float SpeedPower;//추가 이동속도
    public float Add_SpeedPower;// 정규화 보정
    public bool Move_Type; //이동 방식) True : 벡터 , False : 원시적

    float SpeedPower_Diagonal; // 정규화용

    public bool onDead = false;


    //사격
    public GameObject[] P_Bullet;

    public bool Can_Shoot = true;
    public float Shooting_Time;

    public int Mag; //탄창 밀집 개수
    List<Element> Magazine = new List<Element>(); //탄창 큐
    Queue<Element> Ready_Slot = new Queue<Element>(); //탄창에 장전될 다음 총알들

    //public SpriteRenderer[] Chamber; // 표시할 스프라이트 


    //피격 
    bool DamageTime = false; //피격 데미지 쿨타임용
    public bool Can_Heal = true;


    //버프
    BuffManager Buff;

    [HideInInspector]
    public bool Buff_Illusion = false;


    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // 정규화 및 초당 이동량 보정

        for (int i = 1; i <= Mag; i++)
        {
            Magazine.Add(Element.Normal);
            Debug.Log("기본 총알 장전 : " + Magazine[0]);
        }
    }

    private void Start()
    {
        Player = GameManager.instance.player; // 전달
        hp += Add_HP;

        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();
    }

    private void Update()
    {

        if (Can_Heal == true && hp <= 2) //자동 회복
        {
            if (hp == 1)
                hp += 2;
            else if (hp == 2)
                hp += 1;

            Debug.Log("회복됨 : " + hp);
        }

            if (Magazine != null && Can_Shoot == true)
            {

                Invoke("Shooting_System", Shooting_Time);

            }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Buff.DeBuff(Buffs.Illusion, 5f);
        }

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

        Can_Heal = false;
        Invoke("Heal", 5f);
    }

    public void Shooting_System()
    {
        if (Magazine[0] == Element.Normal) //기본 총알 발사
            Ready_Slot.Enqueue(Element.Normal); //기본 총알 대기

       if (Magazine[0] == Element.Normal)
            Instantiate(P_Bullet[0], Player.transform);
       else if (Magazine[0] == Element.Iron)
            Instantiate(P_Bullet[1], Player.transform);
        else if (Magazine[0] == Element.Battery)
            Instantiate(P_Bullet[2], Player.transform);
        else if (Magazine[0] == Element.Fire)
            Instantiate(P_Bullet[3], Player.transform);
        else if (Magazine[0] == Element.Snowball)
            Instantiate(P_Bullet[4], Player.transform);
       else
            Instantiate(P_Bullet[0], Player.transform);

        Magazine.RemoveAt(0);
        Magazine.Add(Ready_Slot.Dequeue());
    }


    private void OnTriggerEnter2D(Collider2D coll)  // 피격 혹은 획득
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
        Can_Heal = true;
    }


    void FixedUpdate() // 이동
    {
        if (Move_Type)
        {
            if (Buff_Illusion == false)
            {
                Vector2 Vectors = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                Vectors.Normalize();
                transform.Translate(Vectors * (SpeedPower_Diagonal) * Time.deltaTime);
            }
            else if(Buff_Illusion == true)
            {
                Vector2 Vectors = new Vector2(-Input.GetAxisRaw("Horizontal"), -Input.GetAxisRaw("Vertical"));
                Vectors.Normalize();
                transform.Translate(Vectors * (SpeedPower_Diagonal) * Time.deltaTime);
            }

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
            Player.transform.Translate(-(SpeedPower_Diagonal) * Time.deltaTime, (SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)) //북동
        {
            Player.transform.Translate((SpeedPower_Diagonal) * Time.deltaTime, (SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)) // 남동
        {
            Player.transform.Translate((SpeedPower_Diagonal) * Time.deltaTime, -(SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)) //남서
        {
            Player.transform.Translate(-(SpeedPower_Diagonal) * Time.deltaTime, -(SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.UpArrow)) //위
        {
            Player.transform.Translate(0, (SpeedPower) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.LeftArrow)) //왼
        {
            Player.transform.Translate(-(SpeedPower) * Time.deltaTime, 0, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow)) //아래
        {
            Player.transform.Translate(0, -(SpeedPower) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.RightArrow)) //오른
        {
            Player.transform.Translate((SpeedPower) * Time.deltaTime, 0, 0);
        }

        else
        {

        }
    }

}


