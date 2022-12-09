using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Enemy : Actor
{
    Action a;
    //public Enemy(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp,attackDamage,moveSpeed, attackDelay) { }
    public Enemy(int hp, int attackDamage) : base(hp, attackDamage) { }

    public float MoveSpeed; //�̵� �ӵ�
    public float AttackSpeed; //���� �ӵ�
    public float MoveLoadingTime; //�̵� �� ���� �� ��� �ð�
    public bool Can_Act = false; //�̵� ���� ���� ���� 

    float onAttackingTime = 0.6f; //��� ��� ��ȯ �ð�

    public GameObject Me;
    public Animator Anime;
    public GameObject Knives;
    [HideInInspector]
    public GameObject Player;
    
    [HideInInspector]
    public bool Fire; //ȭ�� ����
    [HideInInspector]
    public int FireStack; //ȭ�� ����
    [HideInInspector]
    public bool Recharge; //���� ����

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //Anime = Me.GetComponent<Animator>();
        curHp = 9;

        Invoke("TurnOn", MoveLoadingTime);
    }

    private void FixedUpdate()
    {
        if(Can_Act)
            MovingOn();
    }

   void MovingOn()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Me.transform.Translate((Player.transform.position.normalized - Me.transform.position.normalized) * MoveSpeed * Time.deltaTime);

        if(Player.transform.position.x - Me.transform.position.y <= 0)
        {
            Me.GetComponent<SpriteRenderer>().flipX= false;
        }   
        else
        {
            Me.GetComponent<SpriteRenderer>().flipX = true;
        }

    }

    void AttackOn()
    {
        IEnumerator Shot()
        {
            yield return new WaitForSeconds(AttackSpeed);

            Vector3 d = Player.transform.position - Me.transform.position;
            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

            Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(angle - 90, Vector3.forward));

            Attack_Motion();

            Invoke("Attack_Motion", onAttackingTime);
            StartCoroutine(Shot());
        }

        StartCoroutine(Shot());
    }

    void Attack_Motion()
    {
        if(Anime.GetBool("onAttack") == false)
            Anime.SetBool("onAttack", true);
        else
            Anime.SetBool("onAttack", false);

    }

    void TurnOn()
    {
        AttackOn();
        Can_Act = true;
    }

}
