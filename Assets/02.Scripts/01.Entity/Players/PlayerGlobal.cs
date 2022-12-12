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

    [HideInInspector]
    public bool Buff_Illusion = false;


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

    void FixedUpdate() // �̵�
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
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow)) //�ϼ�
        {
            Player.transform.Translate(-(SpeedPower_Diagonal) * Time.deltaTime, (SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)) //�ϵ�
        {
            Player.transform.Translate((SpeedPower_Diagonal) * Time.deltaTime, (SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)) // ����
        {
            Player.transform.Translate((SpeedPower_Diagonal) * Time.deltaTime, -(SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)) //����
        {
            Player.transform.Translate(-(SpeedPower_Diagonal) * Time.deltaTime, -(SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.UpArrow)) //��
        {
            Player.transform.Translate(0, (SpeedPower) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.LeftArrow)) //��
        {
            Player.transform.Translate(-(SpeedPower) * Time.deltaTime, 0, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow)) //�Ʒ�
        {
            Player.transform.Translate(0, -(SpeedPower) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.RightArrow)) //����
        {
            Player.transform.Translate((SpeedPower) * Time.deltaTime, 0, 0);
        }

        else
        {

        }
    }

}
