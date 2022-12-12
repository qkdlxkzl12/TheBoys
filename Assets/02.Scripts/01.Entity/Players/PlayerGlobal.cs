using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Build;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerGlobal : Actor
{
    GameObject Player;
    [HideInInspector]
    public Animator Anime;
    [HideInInspector]
    public SpriteRenderer Player_Sprite;

    //기본
    public int Basic_HP; //기본 체력
    public float SpeedPower;//추가 이동속도
    public float Add_SpeedPower;// 정규화 보정

    //public int[] Level; //레벨
    //public int Exp;
    //public int[] Max_Exp;

    public bool Can_Move = true;
    public bool Move_Type; //이동 방식) True : 벡터 , False : 원시적

    public bool Dead_Statue = false;

    float SpeedPower_Diagonal; // 정규화용

    //사격
    public GameObject[] P_Bullet;

    public bool Can_Shoot = true;
    public float Shooting_Time;

    public int Mag; //탄창 밀집 개수
    List<Element> Magazine = new List<Element>(); //약실
    Queue<Element> Ready_Slot = new Queue<Element>(); //탄창에 장전될 다음 총알들


    //피격 
    bool DamageTime = false; //피격 데미지 쿨타임용
    public bool Can_Heal = true;


    //버프
    BuffManager Buff;

    [HideInInspector]
    public bool Buff_Illusion = false;


    //총알 
    public bool Silver_Bullet = false;


    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // 정규화 및 초당 이동량 보정

        for (int i = 1; i <= Mag; i++)
        {
            Magazine.Add(Element.Normal);
            Debug.Log("기본 총알 장전 : " + Magazine[0]);
        }

        attackDamage = 1;
    }

    private void Start()
    {
        Player = GameManager.instance.player; // 전달

        Anime = Player.GetComponent<Animator>();
        Player_Sprite = Player.GetComponent<SpriteRenderer>();

        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();

        Shooting_System();
    }

    private void Update()
    {
        if (Can_Heal == true && Basic_HP != 3) //자동 회복
        {
            Basic_HP = 3;
            Debug.Log("회복됨 : " + Basic_HP);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Ready_Slot.Enqueue(Element.Snowball);
        }

    }

    void Hit_System()
    {
        if (DamageTime == false) //피격 데미지 쿨타임 여부
        {
            Damaged(1);
            Debug.Log(Basic_HP);

            CancelInvoke("Heal");
        }

        Debug.Log("무적시간 활성화");

        DamageTime = true;  //데미지 쿨타임 시작
        Player_Sprite.color = new Color32(255, 255, 255, 128);//
        Invoke("DamageCoolTime", 0.5f);

        Can_Heal = false;
        Invoke("Heal", 5f);
    }

    public void Shooting_System()
    {
        GameObject Inst_Bullets;

        IEnumerator Shot()
        {
            yield return new WaitForSeconds(Shooting_Time);

            if (Can_Shoot == true)
            {
                if (Magazine[0] == Element.Normal) //기본 총알 발사
                    Ready_Slot.Enqueue(Element.Normal); //기본 총알 대기

                if (Magazine[0] == Element.Normal)
                    Inst_Bullets = Instantiate(P_Bullet[0], Player.transform);
                else if (Magazine[0] == Element.Silver)
                    Inst_Bullets = Instantiate(P_Bullet[1], Player.transform);            
                else if (Magazine[0] == Element.Battery)
                    Inst_Bullets = Instantiate(P_Bullet[2], Player.transform);
                else if (Magazine[0] == Element.Fire)
                    Inst_Bullets = Instantiate(P_Bullet[3], Player.transform);
                else if (Magazine[0] == Element.Snowball)
                    Inst_Bullets = Instantiate(P_Bullet[4], Player.transform);
                else if (Magazine[0] == Element.Thunder)
                    Inst_Bullets = Instantiate(P_Bullet[5], Player.transform);
                else
                    Inst_Bullets = Instantiate(P_Bullet[0], Player.transform);

                Vector2 FireHole = new Vector2(Player.transform.position.x + 1.5f, Player.transform.position.y - 0.5f);
                Inst_Bullets.transform.position = FireHole;

                Magazine.RemoveAt(0);
                Magazine.Add(Ready_Slot.Dequeue());
            }

            StartCoroutine(Shot());
        }

        StartCoroutine(Shot());
    }

    public void Dead_Event()
    {
        Dead_Statue = true;
        Anime.SetBool("OnDead", true);
        Can_Shoot = false;
        Can_Move = false;

        IEnumerator Dead()
        {
            yield return new WaitForSeconds(2.5f);
            GameObject.Find("PlayerUI").GetComponent<PlayerUI>().DeadEvent();
            Destroy(Player);
        }

        if(Dead_Statue == true)
            StartCoroutine(Dead());
    }


    private void OnTriggerEnter2D(Collider2D coll)  // 피격 혹은 획득
    {
        Actor act = coll.GetComponent<Actor>();

        if (coll.gameObject.CompareTag("PlayerBullet")  || coll.gameObject.CompareTag("Item") || coll.gameObject.CompareTag("Untagged"))
        {

        }
        else
        {
            if(Dead_Statue == false)
            {
                Hit_System();

                if (coll.gameObject.CompareTag("EnemyBullet"))
                    Destroy(coll.gameObject);
            }
        }
    }


    void DamageCoolTime() //피격 데미지 쿨타임용
    {
        DamageTime = false;
        Debug.Log("무적시간 비활성화");
        Player_Sprite.color = new Color32(255, 255, 255, 255);
    }

    void Heal() //힐 용
    {
        Can_Heal = true;
    }


    //데미지를 받음
    override protected void Damaged(int value)
    {

        Basic_HP -= value;

        if (Basic_HP <= 0)
        {
            OnDie();
        }
    }

    //액터가 죽을 때
    override protected void OnDie()
    {
        if (gameObject.tag == "Player") //플레이어 전용 데드 이벤트
        {
            gameObject.GetComponent<PlayerGlobal>().Dead_Event();
        }
        else
        {
            Debug.Log(gameObject.name + "Die");
            Destroy(gameObject);
        }
    }


    void FixedUpdate() // 이동
    {
        if(Can_Move == true)
        {
            if (Move_Type)
            {
                if (Buff_Illusion == false)
                {
                    Vector2 Vectors = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                    Vectors.Normalize();
                    transform.Translate(Vectors * (SpeedPower_Diagonal) * Time.deltaTime);
                }
                else if (Buff_Illusion == true)
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
