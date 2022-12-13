using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;

#pragma warning disable 8321

public class Enemy : Actor
{
    public Enemy(float hp, float attackDamage) : base(hp, attackDamage) { }
    protected bool isDying;

    [Header("Statue")]
    public int Max_Hp; //�ִ� ü��
    public float MoveSpeed; //�̵� �ӵ�
    public float AttackSpeed; //���� �ӵ�
    public int AttackNum; //���� Ƚ��
    int AttackUse; //���� ���� Ƚ��
    float AttackCoolLess; //������ ��Ÿ�� ����
    public float MoveLoadingTime; //�̵� �� ���� �� ��� �ð�
    public bool Can_Act = false; //�̵� ���� ���� ���� 

    [ Header("Attack Type") ]
    public bool AttackTypeMonkey = false;
    public bool AttackTypeRaven = false; //��� ����
    public bool AttackTypeLazoul = false;

    float onAttackingTime = 0.49f; //��� ��� ��ȯ �ð�

    bool On_Rush = false;
    bool Can_Rush;

    [Header("Prefabs")]
    public GameObject Me;
    public Animator Anime;
    public GameObject Knives;
    [HideInInspector]
    public GameObject Player;
   
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (AttackTypeRaven == true)
            AttackNum = UnityEngine.Random.Range(2, 4);
        //Anime = Me.GetComponent<Animator>();
        curHp = Max_Hp;
        AttackUse = 0;
        AttackCoolLess = 0;
        increaseAttackSpeed = 1;
        Can_Rush = true;

        Invoke("TurnOn", MoveLoadingTime);
    }

    private void FixedUpdate()
    {
        if(Can_Act)
        {
            MovingOn();
        }

        if (On_Rush)
        {
            GetComponent<SpriteRenderer>().flipX = ((transform.position.x - Player.transform.position.x) < 0) ? false : true;
            transform.Translate(Vector2.left * (MoveSpeed * 16f) * Time.deltaTime);
        }
    }

   void MovingOn()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Me.transform.Translate((Player.transform.position.normalized - Me.transform.position.normalized) * MoveSpeed * Time.deltaTime);

        if(Player.transform.position.x - Me.transform.position.x <= 0)
        {
            GetComponent<SpriteRenderer>().flipX= false;
        }   
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    IEnumerator AttackOn()
    {
        yield return new WaitForSeconds(AttackSpeed - AttackCoolLess);
        Shoot();
    }

    void Shoot()
    {
        if (AttackUse < AttackNum)
        {
            Attack();
        }
        else
        {
            if (AttackTypeRaven)
            {
                Can_Act = false;
                Anime.SetBool("onRush", true);
                Invoke("RushOn", 0.5f);
            }
            else
            {
                AttackUse = 0;
                StartCoroutine(AttackOn());
            }
        }

        void Attack()
        {
            if (Can_Act)
            {
                if(AttackTypeRaven == true)
                {
                    Attack_Motion();
                    Invoke("Attack_Motion", onAttackingTime);

                    Vector3 d = Player.transform.position - Me.transform.position;
                    float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

                    var bullet1 = Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(angle - 40, Vector3.forward)).GetComponent<NormalFire>();
                    bullet1.Init(increaseAttackSpeed,isBroken);
                    var bullet2 =Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(angle - 90, Vector3.forward)).GetComponent<NormalFire>();
                    bullet2.Init(increaseAttackSpeed, isBroken);
                    var bullet3 = Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(angle - 140, Vector3.forward)).GetComponent<NormalFire>();
                    bullet3.Init(increaseAttackSpeed, isBroken);
                    AttackCoolLess = 0.5f;
                }
                else if (AttackTypeLazoul== true)
                {
                    Attack_Motion();
                    Invoke("Attack_Motion", onAttackingTime);

                    Vector3 d = Player.transform.position - Me.transform.position;
                    float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

                    var bullet =  Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(angle - 90, Vector3.forward)).GetComponent<NormalFire>();
                    bullet.Init(increaseAttackSpeed, isBroken);
                    AttackCoolLess = 0.5f;
                }
                else
                {
                    Attack_Motion();
                    Invoke("Attack_Motion", onAttackingTime);

                    if (Me.GetComponent<SpriteRenderer>().flipX == false)
                    {
                        var bullet = Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(Me.transform.rotation.z + 90, Vector3.forward)).GetComponent<NormalFire>();
                        bullet.Init(increaseAttackSpeed, isBroken);
                    }
                    else
                    {
                        var bullet = Instantiate(Knives, new Vector3(Me.transform.position.x, Me.transform.position.y, Me.transform.position.z), Quaternion.AngleAxis(Me.transform.rotation.z - 90, Vector3.forward)).GetComponent<NormalFire>();
                        bullet.Init(increaseAttackSpeed, isBroken);
                    }
                }
                AttackUse++;
                Invoke("Shoot", 1.5f);
            }
        }

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
        On_Rush = true;
        Debug.Log("ImRush!");
        Can_Rush = false;

       IEnumerator RushStop()
        {
            yield return new WaitForSeconds(1f);
            Anime.SetBool("onRush", false);
            On_Rush = false;
            Can_Act = true;
            OnDead();
        }

        StartCoroutine(RushStop());
    }

    void Dead_Motion()
    {
        Anime.SetBool("onDead", true);
    }

    void TurnOn()
    {
        Can_Act = true;
        Shoot();
    }

    public void OnDead()
    {
        Can_Act = false;
        isDying = true;
        StopAllCoroutines();
        GetComponent<CapsuleCollider2D>().enabled = false;
        Dead_Motion();

       IEnumerator Ondead()
        {
            yield return new WaitForSeconds(0.8f);
            base.OnDie();
        }

        StartCoroutine(Ondead());
    }

    protected override void OnDie()
    {
        OnDead();
    }

    protected override void Damaged(float value)
    {
        if(isDying == false)
            base.Damaged(value);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        Actor act = col.gameObject.GetComponent<Actor>();
        if (col.CompareTag("Player"))
        {
            AttackTo(act);
        }
    }

    public void CeateAward()
    {
        ItemManager.instance.CreateExpOrb(transform.position, 30);
    }
}
