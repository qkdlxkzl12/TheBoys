using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGlobal : Actor
{
    GameObject Player;

    public int Add_HP; //추가 체력
    public float SpeedPower;//추가 이동속도
    public float Add_SpeedPower;// 정규화 보정
    public bool Move_Type; //이동 방식) True : 벡터 , False : 원시적

    float SpeedPower_Diagonal; // 정규화용

    public GameObject[] Bullet; //총알 프리팹

    public int Mag; //탄창 밀집 개수
    List<int> Magazine = new List<int>(); //탄창 큐
    Queue<int> Ready_Slot = new Queue<int>(); //탄창에 장전될 다음 총알들

    public SpriteRenderer[] Chamber; // 표시할 스프라이트 

    bool DamageTime = false; //피격 데미지 쿨타임용
    bool Healing = true;

    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // 정규화 및 초당 이동량 보정

        for (int i = 1; i <= Mag; i++)
        {
            Magazine.Add(0);
            Debug.Log("기본 총알 장전");
        }
    }

    private void Start()
    {
        Player = GameManager.instance.player; // 전달
        hp += Add_HP;
    }

    private void Update()
    {

        if (Healing == true && hp <= 2) //자동 회복
        {
            if (hp == 1)
                hp += 2;
            else if (hp == 2)
                hp += 1;

            Debug.Log("회복됨 : " + hp);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Magazine != null)
            {

                Shooting_System();
                Magazine_UI();

            }
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

        Healing = false;
        Invoke("Heal", 5f);
    }

    void Shooting_System()
    {
        Debug.Log("해당 총알 발사 : " + Magazine[0]);

        if (Magazine[0] == 0) //기본 총알 발사
            Ready_Slot.Enqueue(0);

        Magazine.RemoveAt(0);
        Magazine.Add(Ready_Slot.Dequeue());
        Debug.Log("해당 총알 장전 : " + Magazine[4]);

        foreach (int i in Ready_Slot)
        {
            Debug.Log("다음에 장전될 총알 : " + i);
        }
    }

    void Magazine_UI()
    { 
       for (int i = 0; i < Chamber.Length; i++)
        {
            if (Magazine[i] == 0)
            {
                Chamber[i].color = new Color(1, 1, 1, 1);
            }
            else if (Magazine[i] == 1)
            {
                Chamber[i].color = new Color(1, 0, 0, 1);
            }
            else if (Magazine[i] == 2)
            {
                Chamber[i].color = new Color(0, 1, 0, 1);
            }
            else if (Magazine[i] == 3)
            {
                Chamber[i].color = new Color(0, 0, 1, 1);
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D coll)  // 피격 혹은 획득
    {
        if (coll.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(coll.gameObject);
            Hit_System();
        }

        if(coll.gameObject.CompareTag("Item") && coll.name == "임시 아이템(Red)")
        {
            Ready_Slot.Enqueue(1);
            Debug.Log("붉은 총알 장전");
        }

        if (coll.gameObject.CompareTag("Item") && coll.name == "임시 아이템(Green)")
        {
            Ready_Slot.Enqueue(2);
            Debug.Log("녹록 총알 장전");
        }

        if (coll.gameObject.CompareTag("Item") && coll.name == "임시 아이템(Blue)")
        {
            Ready_Slot.Enqueue(3);
            Debug.Log("푸른 총알 장전");
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


