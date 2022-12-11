using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

#pragma warning disable 8321

public class Enemy : Actor
{
    Action a;
    //public Enemy(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp,attackDamage,moveSpeed, attackDelay) { }
    public Enemy(int hp, int attackDamage) : base(hp, attackDamage) { }

    public int Max_Hp; //최대 체력
    public float MoveSpeed; //이동 속도
    public float AttackSpeed; //공격 속도
    public float MoveLoadingTime; //이동 및 공격 전 대기 시간
    public bool Can_Act = false; //이동 공격 가능 여부 
    public bool AttackTypeRaven = false; //까마귀 여부
    public bool AttackTypeLazoul = false;

    float onAttackingTime = 0.6f; //대기 모션 전환 시간

    bool On_Rush = false;
    bool Can_Rush;

    public GameObject Me;
    public Animator Anime;
    public GameObject Knives;
    [HideInInspector]
    public GameObject Player;
   

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //Anime = Me.GetComponent<Animator>();
        curHp = Max_Hp;

        Can_Rush = true;

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

        if (On_Rush)
            Me.transform.Translate(Vector2.left * (MoveSpeed * 6.5f) * Time.deltaTime);
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

        if (AttackTypeRaven)
        {
            if (Math.Abs(Player.transform.position.x - Me.transform.position.x) <= 7f)
            {
                if (Can_Rush == true)
                {
                    RushOn();
                }
            }
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

                GameObject EB = Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(angle - 90, Vector3.forward));

                if(Recharge)
                    EB.GetComponent<NormalFire>().Speed *= 0.5f;

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
        Can_Act = false;
        Anime.SetBool("onRush", true);
        On_Rush = true;
        Debug.Log("ImRush!");
        Can_Rush = false;

       IEnumerator RushStop()
        {
            yield return new WaitForSeconds(1f);
            Anime.SetBool("onRush", false);
            On_Rush = false;
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

    public void OnDead()
    {
        Can_Act = false;
        StopAllCoroutines();
        Me.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 128);
        Dead_Motion();

       IEnumerator Ondead()
        {
            yield return new WaitForSeconds(0.8f);

            Debug.Log(gameObject.name + "Die");
            Destroy(gameObject);
        }

        StartCoroutine(Ondead());
    }

}
