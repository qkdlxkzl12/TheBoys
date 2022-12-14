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
    readonly Vector2 limitMovementLT = new Vector2(-1.2f,1.3f);
    readonly Vector2 limitMovementRD = new Vector2(1.1f, -1);
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
    
    private bool isIllusion = false;


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
        ShootingSystem();
        MoveControll();
    }

    public void ShootingSystem()
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

    public override void TakeAttack(float damage)
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

        if(curHp >= 3)
        {
            GameObject.Find("BloodyUI").SetActive(false);
        }

    }

    protected override void Damaged(float value)
    {
        if (Dead_Statue == false)
        {
            curHp -= value;

            if (curHp <= 2)
            {
                GameObject.Find("BloodyUI").SetActive(true);
            }

            if (curHp <= 0)
            {
                Dead_Statue = true;
                Anime.SetBool("OnDead", true);
                Can_Move = false;

                IEnumerator Dead()
                {
                    yield return new WaitForSeconds(2.5f);
                    GameObject.Find("PauseUI").GetComponent<PlayerUI>().DeadEvent();
                    //Destroy(Player);
                }
                StartCoroutine(Dead());
            }
        }
    }

    void MoveControll() // 이동
    {
        if (Can_Move == false)
            return;
        Vector2 vec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * (isIllusion == true ? -1 : 1);
        //움직임 제한
        if (transform.position.x + limitMovementLT.x < -8.9f && vec.x < 0)
            vec.x = 0;
        if (transform.position.y + limitMovementLT.y > 5f && vec.y > 0)
            vec.y = 0;
        if (transform.position.x + limitMovementRD.x > 8.9f && vec.x > 0)
            vec.x = 0;
        if (transform.position.y + limitMovementRD.y < -4.3f && vec.y < 0)
            vec.y = 0;
        //-8.9, 5


        vec.Normalize();
        transform.Translate(vec * (SpeedPower_Diagonal) * Time.deltaTime);
    }

    public void SwitchMovement(bool isSwitching)
    {
        isIllusion = isSwitching;
    }
}
