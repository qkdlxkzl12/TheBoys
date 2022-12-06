using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Build;

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
    //public bool Silver_Bullet == false;

    //Ư��
    public Trait trait;

    [HideInInspector]
    public int Shield; //���差
    float Shield_Time = 3f; //���� �ֱ�

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

        curHp = Basic_HP;
        attackDamage = 1;
    }

    private void Start()
    {
        Player = GameManager.instance.player; // ����

        Anime = Player.GetComponent<Animator>();
        Player_Sprite = Player.GetComponent<SpriteRenderer>();

        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();

        Trait_System();
        Shooting_System();
    }

    private void Update()
    {

        if (Can_Heal == true && curHp != Basic_HP) //�ڵ� ȸ��
        {
            curHp = Basic_HP;
            Debug.Log("ȸ���� : " + curHp);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Buff.DeBuff(Buffs.Illusion, 5f);
        }

    }

    void Hit_System()
    {
        if (DamageTime == false) //�ǰ� ������ ��Ÿ�� ����
        {
            AttackTo(Player.GetComponent<Actor>());
            Debug.Log(curHp);

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
        else if (trait == Trait.������)
        {
            return true; //�÷��̾� �ҷ����� ���
        }
        else if (trait == Trait.RTX)
        {
            GameObject Rtx = Player.transform.GetChild(0).gameObject;
            Rtx.SetActive(true);
        }
        else if (trait == Trait.����)
        {
           //�� ã�� �������� ����. �ٵ� �� �±״� ���� ���ĸ��� �� ���� �з�?
        }
        else if (trait == Trait.����)
        {
            Shooting_Time *= (1f - Random.Range(0.2f, 0.3f));
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
            

            //��� �� �̺�Ʈ �߰� �ʿ�
            yield return new WaitForSeconds(1.5f);
            Destroy(Player);
        }

        if(Dead_Statue == false)
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

     new public void AttackTo(Actor target)
    {
        //����� ActorŸ���� �ƴ� ���
        if (target == null)
        {
            Debug.LogError("[ERROR:001]" + target.name + "'s type is not Actor");
            return;
        }
        this.OnAttack();
        target.TakeAttack(this.attackDamage);
    }

    override public void OnAttack()
    {
        //���� �� �߻��ϴ� �̺�Ʈ �ۼ�(��� �� ���������� ����)
    }

    override public void TakeAttack(int damage)
    {
        //������ ������ �߻��ϴ� �̺�Ʈ

        if (gameObject.tag == "Player") //?
        {
            PlayerGlobal player = gameObject.GetComponent<PlayerGlobal>();
        }

        Damaged(damage);
    }

    //�������� ����
    override protected void Damaged(int value)
    {
        if (gameObject.tag == "Player")
        {
            PlayerGlobal player = gameObject.GetComponent<PlayerGlobal>();

            if (player.Shield > 0)
            {
                player.Shield -= value;

                if (player.synergy == Synergy.���� && player.Shield <= 0)
                {
                    GameObject[] E_Bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
                    foreach (GameObject i in E_Bullets)
                        Destroy(i);

                    Invoke("Trait_System", Shield_Time);
                }
            }
        }
        else
            curHp -= value;

        if (curHp <= 0)
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
