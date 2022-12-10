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

    public float MoveSpeed; //이동 속도
    public float AttackSpeed; //공격 속도
    public float MoveLoadingTime; //이동 및 공격 전 대기 시간
    public bool Can_Act = false; //이동 공격 가능 여부 
    public bool AttackTypeRaven = false; //까마귀 여부
    public bool AttackTypeLazoul = false;

    float onAttackingTime = 0.6f; //대기 모션 전환 시간

    public GameObject Me;
    public Animator Anime;
    public GameObject Knives;
    [HideInInspector]
    public GameObject Player;
    
    [HideInInspector]
    public bool Fire; //화상 버프
    [HideInInspector]
    public int FireStack; //화상 스택
    [HideInInspector]
    public bool Recharge; //충전 버프

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //Anime = Me.GetComponent<Animator>();
        curHp = 9;

        Invoke("TurnOn", MoveLoadingTime);
    }

    private void Update()
    {
        if(FireStack >= 3) //화상 폭발
        {

        }
    }

    private void FixedUpdate()
    {
        if(Can_Act)
        {
            MovingOn();
        }
    }

   void MovingOn()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Me.transform.Translate((Player.transform.position.normalized - Me.transform.position.normalized) * MoveSpeed * Time.deltaTime);

        if(Player.transform.position.x - Me.transform.position.x <= 0)
        {
            Me.GetComponent<SpriteRenderer>().flipX= false;
        }   
        else
        {
            Me.GetComponent<SpriteRenderer>().flipX = true;
        }

        if(AttackTypeRaven)
            if(Math.Abs(Player.transform.position.x - Me.transform.position.y) <= 15)
            {
                Can_Act = false;
                RushOn();
            }
    }

    void AttackOn()
    {
        IEnumerator Shot()
        {
            yield return new WaitForSeconds(AttackSpeed);

            if(Can_Act)
            {
                Vector3 d = Player.transform.position - Me.transform.position;
                float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

                Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(angle - 90, Vector3.forward));

                Attack_Motion();
            }

            Invoke("Attack_Motion", onAttackingTime);
            StartCoroutine(Shot());
        }

        StartCoroutine(Shot());
    }

    void Attack_Motion()
    {
        if(AttackTypeRaven == false)
        {
            if (Anime.GetBool("onAttack") == false)
                Anime.SetBool("onAttack", true);
            else
                Anime.SetBool("onAttack", false);
        }
    }

    void RushOn()
    {
        Anime.SetBool("onRush", true);
        Me.transform.Translate(Player.transform.position * Time.deltaTime);

       IEnumerator RushStop()
        {
            yield return new WaitForSeconds(0.5f);
            Anime.SetBool("onRush", false);
            Can_Act = true;
        }

        StartCoroutine(RushStop());
    }

    void Dead_Motion()
    {
        if(AttackTypeLazoul == false)
        {
            Anime.SetBool("onDead", true);
        }
    }

    void TurnOn()
    {
        AttackOn();
            
        Can_Act = true;
    }

    override protected void OnDie()
    {
        Debug.Log(gameObject.name + "Die");
        Destroy(gameObject);
    }

}
