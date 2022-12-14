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
    //�⺻
    public int Basic_HP; //�⺻ ü��
    public float SpeedPower;//�߰� �̵��ӵ�
    public float Add_SpeedPower;// ����ȭ ����

    //public int[] Level; //����
    //public int Exp;
    //public int[] Max_Exp;

    public bool Can_Move = true;
    public bool Move_Type; //�̵� ���) True : ���� , False : ������

    public bool Dead_Statue = false;

    float SpeedPower_Diagonal; // ����ȭ��

    //�ǰ� 
    bool DamageTime = false; //�ǰ� ������ ��Ÿ�ӿ�
    float fireDelay;
    float elapsedTime;
    IEnumerator autoHeal;


    //����
    BuffManager Buff;
    
    private bool isIllusion = false;


    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // ����ȭ �� �ʴ� �̵��� ����

    }

    private void Start()
    {
        Player = GameManager.instance.player; // ����
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
        DamageTime = true;  //������ ��Ÿ�� ����
        Player_Sprite.color = new Color32(255, 255, 255, 128);//
        Invoke("DamageCoolTime", 0.5f);
        if(autoHeal != null)
            StopCoroutine(autoHeal);
        autoHeal = Regeneration();
        StartCoroutine(autoHeal);
    }

    void DamageCoolTime() //�ǰ� ������ ��Ÿ�ӿ�
    {
        DamageTime = false;
        Player_Sprite.color = new Color32(255, 255, 255, 255);
    }

    IEnumerator Regeneration() //�� ��
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

    void MoveControll() // �̵�
    {
        if (Can_Move == false)
            return;
        Vector2 vec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * (isIllusion == true ? -1 : 1);
        //������ ����
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
