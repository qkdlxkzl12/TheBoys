using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGlobal : Actor
{
    GameObject Player;

    //�⺻
    public int Basic_HP; //�⺻ ü��
    public float SpeedPower;//�߰� �̵��ӵ�
    public float Add_SpeedPower;// ����ȭ ����
    public bool Move_Type; //�̵� ���) True : ���� , False : ������

    float SpeedPower_Diagonal; // ����ȭ��

    public bool onDead = false;


    //���
    public GameObject[] P_Bullet;

    public bool Can_Shoot = true;
    public float Shooting_Time;

    public int Mag; //źâ ���� ����
    List<Element> Magazine = new List<Element>(); //źâ ť
    Queue<Element> Ready_Slot = new Queue<Element>(); //źâ�� ������ ���� �Ѿ˵�

    //public SpriteRenderer[] Chamber; // ǥ���� ��������Ʈ 


    //�ǰ� 
    bool DamageTime = false; //�ǰ� ������ ��Ÿ�ӿ�
    public bool Can_Heal = true;


    //����
    BuffManager Buff;

    [HideInInspector]
    public bool Buff_Illusion = false;


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

        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();
    }

    private void Update()
    {

        if (Can_Heal == true && curHp != Basic_HP) //�ڵ� ȸ��
        {
            curHp = Basic_HP;
            Debug.Log("ȸ���� : " + curHp);
        }

            if (Magazine != null && Can_Shoot == true)
            {

               // Invoke("Shooting_System", Shooting_Time);

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
        Invoke("DamageCoolTime", 0.5f);

        Can_Heal = false;
        Invoke("Heal", 5f);
    }

    public void Shooting_System()
    {
        if (Magazine[0] == Element.Normal) //�⺻ �Ѿ� �߻�
            Ready_Slot.Enqueue(Element.Normal); //�⺻ �Ѿ� ���

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


    private void OnTriggerEnter2D(Collider2D coll)  // �ǰ� Ȥ�� ȹ��
    {
        if (coll.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(coll.gameObject);
            Hit_System();
        }


    }

    void DamageCoolTime() //�ǰ� ������ ��Ÿ�ӿ�
    {
        DamageTime = false;
        Debug.Log("�����ð� ��Ȱ��ȭ");
    }

    void Heal() //�� ��
    {
        Can_Heal = true;
    }


    void FixedUpdate() // �̵�
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


