using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGlobal : Actor
{
    GameObject Player;

    //�⺻
    public int Add_HP; //�߰� ü��
    public float SpeedPower;//�߰� �̵��ӵ�
    public float Add_SpeedPower;// ����ȭ ����
    public bool Move_Type; //�̵� ���) True : ���� , False : ������

    float SpeedPower_Diagonal; // ����ȭ��

    //�Ѿ�
    public int Mag; //źâ ���� ����
    List<int> Magazine = new List<int>(); //źâ ť
    Queue<int> Ready_Slot = new Queue<int>(); //źâ�� ������ ���� �Ѿ˵�

    //public SpriteRenderer[] Chamber; // ǥ���� ��������Ʈ 


    //�ǰ� 
    bool DamageTime = false; //�ǰ� ������ ��Ÿ�ӿ�
    bool Healing = true;


    //����
    BuffManager Buff;
    public bool Buff_Illusion = false;
   

    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // ����ȭ �� �ʴ� �̵��� ����

        for (int i = 1; i <= Mag; i++)
        {
            Magazine.Add(0);
            Debug.Log("�⺻ �Ѿ� ����");
        }
    }

    private void Start()
    {
        Player = GameManager.instance.player; // ����
        hp += Add_HP;

        Buff = GameObject.Find("BuffManager").GetComponent<BuffManager>();
    }

    private void Update()
    {

        if (Healing == true && hp <= 2) //�ڵ� ȸ��
        {
            if (hp == 1)
                hp += 2;
            else if (hp == 2)
                hp += 1;

            Debug.Log("ȸ���� : " + hp);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Magazine != null)
            {

                Shooting_System();

            }
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
            AttackTo(Player);
            Debug.Log(hp);

            CancelInvoke("Heal");
        }

        Debug.Log("�����ð� Ȱ��ȭ");
        DamageTime = true;  //������ ��Ÿ�� ����
        Invoke("DamageCoolTime", 0.5f);

        Healing = false;
        Invoke("Heal", 5f);
    }

    void Shooting_System()
    {
        Debug.Log("�ش� �Ѿ� �߻� : " + Magazine[0]);

        if (Magazine[0] == 0) //�⺻ �Ѿ� �߻�
            Ready_Slot.Enqueue(0);

        Magazine.RemoveAt(0);
        Magazine.Add(Ready_Slot.Dequeue());
        Debug.Log("�ش� �Ѿ� ���� : " + Magazine[4]);

        foreach (int i in Ready_Slot)
        {
            Debug.Log("������ ������ �Ѿ� : " + i);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)  // �ǰ� Ȥ�� ȹ��
    {
        if (coll.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(coll.gameObject);
            Hit_System();
        }

        if(coll.gameObject.CompareTag("Item") && coll.name == "�ӽ� ������(Red)")
        {
            Ready_Slot.Enqueue(1);
            Debug.Log("���� �Ѿ� ����");
        }

        if (coll.gameObject.CompareTag("Item") && coll.name == "�ӽ� ������(Green)")
        {
            Ready_Slot.Enqueue(2);
            Debug.Log("��� �Ѿ� ����");
        }

        if (coll.gameObject.CompareTag("Item") && coll.name == "�ӽ� ������(Blue)")
        {
            Ready_Slot.Enqueue(3);
            Debug.Log("Ǫ�� �Ѿ� ����");
        }
    }


    void DamageCoolTime() //�ǰ� ������ ��Ÿ�ӿ�
    {
        DamageTime = false;
        Debug.Log("�����ð� ��Ȱ��ȭ");
    }

    void Heal() //�� ��
    {
        Healing = true;
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
            Player.transform.Translate(-(moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, (moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)) //�ϵ�
        {
            Player.transform.Translate((moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, (moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)) // ����
        {
            Player.transform.Translate((moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, -(moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)) //����
        {
            Player.transform.Translate(-(moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, -(moveSpeed * SpeedPower_Diagonal) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.UpArrow)) //��
        {
            Player.transform.Translate(0, (moveSpeed * SpeedPower) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.LeftArrow)) //��
        {
            Player.transform.Translate(-(moveSpeed * SpeedPower) * Time.deltaTime, 0, 0);
        }

        else if (Input.GetKey(KeyCode.DownArrow)) //�Ʒ�
        {
            Player.transform.Translate(0, -(moveSpeed * SpeedPower) * Time.deltaTime, 0);
        }

        else if (Input.GetKey(KeyCode.RightArrow)) //����
        {
            Player.transform.Translate((moveSpeed * SpeedPower) * Time.deltaTime, 0, 0);
        }

        else
        {

        }
    }

}


