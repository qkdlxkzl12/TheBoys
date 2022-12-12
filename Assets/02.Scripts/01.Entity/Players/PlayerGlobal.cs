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

    //피격 
    bool DamageTime = false; //피격 데미지 쿨타임용
    float fireDelay;
    float elapsedTime;
    IEnumerator autoHeal;


    //버프
    BuffManager Buff;

    [HideInInspector]
    public bool Buff_Illusion = false;


    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // 정규화 및 초당 이동량 보정

    }

    private void Start()
    {
        Player = GameManager.instance.player; // 전달
        fireDelay = .35f;
        Anime = Player.GetComponent<Animator>();
        Player_Sprite = Player.GetComponent<SpriteRenderer>();
        //Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();

    }

    private void Update()
    {
        Shooting_System();
    }

    public void Shooting_System()
    {
        if (Dead_Statue == true)
            return;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= fireDelay)
        {
            BulletManager.instance.PlayerBulletFire(transform.position);
            elapsedTime = 0;
        }
    }

    public override void TakeAttack(int damage)
    {
        if (Dead_Statue == true|| DamageTime == true)
            return;
        base.TakeAttack(1);
        DamageTime = true;  //데미지 쿨타임 시작
        Player_Sprite.color = new Color32(255, 255, 255, 128);//
        Invoke("DamageCoolTime", 0.5f);
        if(autoHeal != null)
            StopCoroutine(autoHeal);
        autoHeal = Regeneration();
        StartCoroutine(autoHeal);
    }

    void DamageCoolTime() //피격 데미지 쿨타임용
    {
        DamageTime = false;
        Player_Sprite.color = new Color32(255, 255, 255, 255);
    }

    IEnumerator Regeneration() //힐 용
    {
        yield return new WaitForSeconds(5);
        Healing(1);
        if(curHp != maxHp)
        {
            autoHeal = Regeneration();
            StartCoroutine(autoHeal);
        }
    }

    protected override void Damaged(int value)
    {
        if (Dead_Statue == false)
        {
            curHp -= value;
            if (curHp <= 0)
            {
                Debug.Log("Hi");
                Dead_Statue = true;
                Anime.SetBool("OnDead", true);
                Can_Move = false;

                IEnumerator Dead()
                {
                    yield return new WaitForSeconds(2.5f);
                    GameObject.Find("PlayerUI").GetComponent<PlayerUI>().DeadEvent();
                    //Destroy(Player);
                }
                StartCoroutine(Dead());
            }
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
