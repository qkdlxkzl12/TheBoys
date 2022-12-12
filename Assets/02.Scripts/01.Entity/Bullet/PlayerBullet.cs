using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerBullet : Bullet
{
    public Element element;
    public BulletType type;

    public float MoveSpeed;

    float snoballForce = 0f;

    Vector2 direction;
    float value;

    Action OnUpdate;
    Action<Actor> OnHit;
    

    //public EnemyBullet(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp, attackDamage, moveSpeed, attackDelay)
    // Start is called before the first frame update
    void Awake()
    {
       
    }

    private void Start()
    {
    }


    void Update()
    {
        if(OnUpdate != null)
            OnUpdate();
    }


    public void Init(Element element )
    {
        this.element = element;
        OnUpdate = () => transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
        if (element == Element.Normal)
        {
            attackDamage = 2;
        }
        else if (element == Element.Silver)
        {
            //GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet = true;
        }
        else if (element == Element.Thunder)
        {
            //사라짐
        }
        else if (element == Element.Battery)
        {
            //BuffManager에서 target에게 충전 적용
        }
        else if (element == Element.Fire)
        {
            OnHit += (target) => {
                //BuffManager에서 target에게 화상 적용
            };
        }
        else if (element == Element.Snowball)
        {
            OnUpdate += () => {
                snoballForce += Time.deltaTime * 0.1f;
                transform.localScale += Vector3.one * snoballForce;
                };
            OnHit = (target) => {
                Debug.Log(snoballForce);
                snoballForce = 0;
            };

        }
        OnHit = (target) =>
        {
            AttackTo(target);
            OnDie();
        };
    }


    private void OnTriggerEnter2D(Collider2D col)  // 공격
    {
            Actor act = col.GetComponent<Actor>();

        if (col.gameObject.CompareTag("Enemy") || col.CompareTag("Boss"))
        {
            OnHit(act);
            //OnDeadChecking(coll);
        }
           

            //if (element != Element.Silver && GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet == true) // 은탄
            {
                //GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet = false;
                //AttackTo(act);
            }
    }


    void OnDeadChecking(Collider2D coll)
    {
        Actor act = coll.GetComponent<Actor>();

        //if (element != Element.Silver && GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet == true)
        {
            if (element == Element.Snowball)
            {
                //if (GameManager.instance.player.GetComponent<PlayerGlobal>().synergy == Synergy.눈보라)
                //{
                //    if (act.curHp <= 4)
                //    {
                //        act.curHp = 666;
                //        coll.gameObject.GetComponent<Enemy>().OnDead();
                //    }
                //}
                //else
                //    if (act.curHp <= 3)
                //{
                //    act.curHp = 666;
                //    coll.gameObject.GetComponent<Enemy>().OnDead();
                //}
            }
            else
                if (act.curHp <= 2)
            {
                act.curHp = 666;
                coll.gameObject.GetComponent<Enemy>().OnDead();
            }
        }
        //else
        {
            if (element == Element.Snowball)
            {
                //if (GameManager.instance.player.GetComponent<PlayerGlobal>().synergy == Synergy.눈보라)
                {
                    if (act.curHp <= 3)
                    {
                        act.curHp = 666;
                        coll.gameObject.GetComponent<Enemy>().OnDead();
                    }
                }
                //else
                    if (act.curHp <= 2)
                {
                    act.curHp = 666;
                    coll.gameObject.GetComponent<Enemy>().OnDead();
                }
            }
            else
                if (act.curHp <= 1)
            {
                act.curHp = 666;
                coll.gameObject.GetComponent<Enemy>().OnDead();
            }
        }
    }
    override protected void OnDie()
    {
        gameObject.SetActive(false);
    }
}
