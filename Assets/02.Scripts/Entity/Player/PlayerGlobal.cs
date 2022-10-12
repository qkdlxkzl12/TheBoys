using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobal : Actor
{

    GameObject Player;

    public int Add_Hp; // �߰� ü��

    public float SpeedPower;//�߰� �̵��ӵ�
    public float Add_SpeedPower;// ����ȭ ����
    public bool Move_Type; //�̵� ���) True : ���� , False : ������

    float SpeedPower_Diagonal; // ����ȭ��

    private void Awake()
    {
        SpeedPower_Diagonal = SpeedPower * Add_SpeedPower; // ����ȭ �� �ʴ� �̵��� ����
    }

    private void Start()
    {
        Player = GameManager.instance.player; // ����
        hp += Add_Hp;
    }

    private void OnCollisionEnter2D(Collision2D coll) // �ǰ�
    {

        if (coll.gameObject.CompareTag("EnemyBullet"))
        {
            AttackTo(Player);
            Destroy(coll.gameObject); // �Ѿ� ����
        }

    }

    void FixedUpdate() // �̵�
    {
        if (Move_Type)
        {
            Vector2 Vectors = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vectors.Normalize();
            transform.Translate(Vectors * (SpeedPower_Diagonal) * Time.deltaTime);

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


