using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Trait { 보호막, 돋보기, RTX, 벼락, 광란, 없음 }
public enum Synergy { 은총, 천체망, 과충전, 전격, 폭격, 없음 }

public class PlayerGlobal : Actor
{
    GameObject Player;
    [HideInInspector]
    public Animator Anime;

    //기본
    public int Basic_HP; //기본 체력
    public float SpeedPower;//추가 이동속도
    public float Add_SpeedPower;// 정규화 보정

    public bool Can_Move = true;
    public bool Move_Type; //이동 방식) True : 벡터 , False : 원시적

    public bool Dead_Statue = false;

    float SpeedPower_Diagonal; // 정규화용

    //사격
    public GameObject[] P_Bullet;

    public bool Can_Shoot = true;
    public float Shooting_Time;

    public int Mag; //탄창 밀집 개수
    List<Element> Magazine = new List<Element>(); //탄창 큐
    Queue<Element> Ready_Slot = new Queue<Element>(); //탄창에 장전될 다음 총알들


    //피격 
    bool DamageTime = false; //피격 데미지 쿨타임용
    public bool Can_Heal = true;


    //버프
    BuffManager Buff;

    [HideInInspector]
    public bool Buff_Illusion = false;


    //특성
    public Trait trait;

    [HideInInspector]
    public int Shield; //쉴드량
    //public float Shiled_Time;


    //시너지
    public Synergy synergy;


    private void Awake()
    {

        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // 정규화 및 초당 이동량 보정

        for (int i = 1; i <= Mag; i++)
        {
            Magazine.Add(Element.Normal);
            Debug.Log("기본 총알 장전 : " + Magazine[0]);
        }

        curHp = Basic_HP;
        attackDamage = 1;
    }

    private void Start()
    {
        Player = GameManager.instance.player; // 전달

        Anime = Player.GetComponent<Animator>();

        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();

        Trait_System();
        Shooting_System();
    }

    private void Update()
    {

        if (Can_Heal == true && curHp != Basic_HP) //자동 회복
        {
            curHp = Basic_HP;
            Debug.Log("회복됨 : " + curHp);
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
            AttackTo(Player.GetComponent<Actor>());
            Debug.Log(curHp);

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
                else if (Magazine[0] == Element.Iron)
                    Inst_Bullets = Instantiate(P_Bullet[1], Player.transform);
                else if (Magazine[0] == Element.Battery)
                    Inst_Bullets = Instantiate(P_Bullet[2], Player.transform);
                else if (Magazine[0] == Element.Fire)
                    Inst_Bullets = Instantiate(P_Bullet[3], Player.transform);
                else if (Magazine[0] == Element.Snowball)
                    Inst_Bullets = Instantiate(P_Bullet[4], Player.transform);
                else
                    Inst_Bullets = Instantiate(P_Bullet[0], Player.transform);

                Inst_Bullets.transform.position = Player.transform.position;

                Magazine.RemoveAt(0);
                Magazine.Add(Ready_Slot.Dequeue());
            }

            StartCoroutine(Shot());
        }

        StartCoroutine(Shot());
    }

    public bool Trait_System()
    {
        if(trait == Trait.보호막)
        {
            Shield += 1; //여기서 함수 짜고 재귀
        }
        else if (trait == Trait.돋보기)
        {
            return true; //플레이어 불렛에서 사용
        }
        else if (trait == Trait.RTX)
        {
            //여기서 함수 짜고 재귀
        }
        else if (trait == Trait.벼락)
        {
           //적 찾고 랜덤으로 벼락. 근데 적 태그는 뭐고 자파르는 왜 따로 분류?
        }
        else if (trait == Trait.광란)
        {
            Shooting_Time *= Random.Range(1.05f, 1.1f);
        }

        return false;
    }

    public void Dead_Event()
    {
        Dead_Statue = true;
        Anime.SetBool("OnDead", true);
        Can_Shoot = false;
        Can_Move = false;
            
        IEnumerator Dead()
        {
            

            //사망 시 이벤트 추가 필요
            yield return new WaitForSeconds(1.5f);
            Destroy(Player);
        }

        if(Dead_Statue == false)
            StartCoroutine(Dead());
    }


    private void OnTriggerEnter2D(Collider2D coll)  // 피격 혹은 획득
    {
        Actor act = coll.GetComponent<Actor>();
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
