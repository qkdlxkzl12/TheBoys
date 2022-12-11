using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Build;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum Trait { ��ȣ��, ������, RTX, ����, ����, ���� }
public enum Synergy { ����, ������, ����, ����, ����, ���� }

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

    //���
    public GameObject[] P_Bullet;

    public bool Can_Shoot = true;
    public float Shooting_Time;

    public int Mag; //źâ ���� ����
    List<Element> Magazine = new List<Element>(); //źâ ť
    Queue<Element> Ready_Slot = new Queue<Element>(); //źâ�� ������ ���� �Ѿ˵�


    //�ǰ� 
    bool DamageTime = false; //�ǰ� ������ ��Ÿ�ӿ�
    public bool Can_Heal = true;


    //����
    BuffManager Buff;

    [HideInInspector]
    public bool Buff_Illusion = false;


    //�Ѿ� 
    public bool Silver_Bullet = false;

    //Ư��
    public Trait trait;

    int Shield = 1; //���差
    float Shield_Time = 3f; //���� �ֱ�

    GameObject Rtx; //RTX

    //�ó���
    public Synergy synergy;


    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // ����ȭ �� �ʴ� �̵��� ����

        for (int i = 1; i <= Mag; i++)
        {
            Magazine.Add(Element.Normal);
            Debug.Log("�⺻ �Ѿ� ���� : " + Magazine[0]);
        }

        attackDamage = 1;
    }

    private void Start()
    {
        Player = GameManager.instance.player; // ����

        Anime = Player.GetComponent<Animator>();
        Player_Sprite = Player.GetComponent<SpriteRenderer>();

        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();

        Rtx = Player.transform.Find("RTX").gameObject;

        Trait_System();
        Shooting_System();
    }

    private void Update()
    {
        if (Can_Heal == true && Basic_HP != 3) //�ڵ� ȸ��
        {
            Basic_HP = 3;
            Debug.Log("ȸ���� : " + Basic_HP);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {

        }

    }

    void Hit_System()
    {
        if (DamageTime == false) //�ǰ� ������ ��Ÿ�� ����
        {
            Damaged(1);
            Debug.Log(Basic_HP);

            CancelInvoke("Heal");
        }

        Debug.Log("�����ð� Ȱ��ȭ");

        DamageTime = true;  //������ ��Ÿ�� ����
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
                if (Magazine[0] == Element.Normal) //�⺻ �Ѿ� �߻�
                    Ready_Slot.Enqueue(Element.Normal); //�⺻ �Ѿ� ���

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

    public void Trait_System()
    {
        if(trait == Trait.��ȣ��)
        {
            if (synergy == Synergy.����)
            {
                Shield += 1;
            }
            else
            {
                if (Shield <= 0)
                {
                    Shield += 1;
                    Debug.Log("���� �����");
                    Invoke("Trait_System", Shield_Time);
                }
                else
                {
                    Shield = 0;
                    Debug.Log("���� �ı�");
                    Invoke("Trait_System", Shield_Time);
                }
            }
        }

        if (trait == Trait.������)
        {
            //�÷��̾� �ҷ����� ���
        }

        if (trait == Trait.RTX)
        {
            Rtx.SetActive(true);
        }

        if (trait == Trait.����)
        {
            //�̻��
        }

        if (trait == Trait.����)
        {
            Shooting_Time *= (1f - Random.Range(0.2f, 0.3f));
        }
    }

    public void Dead_Event()
    {
        Dead_Statue = true;
        Anime.SetBool("OnDead", true);
        Can_Shoot = false;
        Can_Move = false;
        Rtx.SetActive(false);

        IEnumerator Dead()
        {
            yield return new WaitForSeconds(2.5f);
            GameObject.Find("PlayerUI").GetComponent<PlayerUI>().DeadEvent();
            Destroy(Player);
        }

        if(Dead_Statue == true)
            StartCoroutine(Dead());
    }


    private void OnTriggerEnter2D(Collider2D coll)  // �ǰ� Ȥ�� ȹ��
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


    void DamageCoolTime() //�ǰ� ������ ��Ÿ�ӿ�
    {
        DamageTime = false;
        Debug.Log("�����ð� ��Ȱ��ȭ");
        Player_Sprite.color = new Color32(255, 255, 255, 255);
    }

    void Heal() //�� ��
    {
        Can_Heal = true;
    }


    //�������� ����
    override protected void Damaged(int value)
    {
        if (Shield > 0)
        {
            Shield -= value;

            if (synergy == Synergy.���� && Shield <= 0)
            {
                GameObject[] E_Bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
                foreach (GameObject i in E_Bullets)
                    Destroy(i);

                //player.synergy = Synergy.����;
                Invoke("Trait_System", Shield_Time);
            }
        }
        else
            Basic_HP -= value;

        if (Basic_HP <= 0)
        {
            OnDie();
        }
    }

    //���Ͱ� ���� ��
    override protected void OnDie()
    {
        if (gameObject.tag == "Player") //�÷��̾� ���� ���� �̺�Ʈ
        {
            gameObject.GetComponent<PlayerGlobal>().Dead_Event();
        }
        else
        {
            Debug.Log(gameObject.name + "Die");
            Destroy(gameObject);
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
