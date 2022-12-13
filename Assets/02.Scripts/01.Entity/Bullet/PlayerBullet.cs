using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerBullet : Bullet
{
    static bool isStrong;
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
        //������ Ư���� ȹ��������
        if (CharacteristicManager.instance.hasLens == true)
        {
            transform.localScale = Vector3.one * 1.1f;
            snoballForce = 0.2f;
        }

        if (element == Element.Normal)
        {
            attackDamage = 10;
            OnUpdate = null;
            OnHit = null;
        }
        else if (element == Element.Silver)
        {
            attackDamage = 15;
            OnHit = (target) => {
                isStrong = true;
            };
        }
        else if (element == Element.Thunder)
        {
            //�����
        }
        else if (element == Element.Battery)
        {
            //BuffManager���� target���� ���� ����
        }
        else if (element == Element.Fire)
        {
            OnHit = (target) => {
                //BuffManager���� target���� ȭ�� ����
            };
        }
        else if (element == Element.Snowball)
        {
            attackDamage = 15;
            OnUpdate = () => {
                snoballForce += Time.deltaTime;
                if(Time.timeScale != 0)
                    transform.localScale += Vector3.one * snoballForce * 0.01f;
                transform.Rotate(Vector3.forward * Time.deltaTime * 30);
                };
            OnHit = (target) => {
                attackDamage *= (1 + snoballForce);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                snoballForce = 0;
            };
        }
        OnUpdate += () => transform.position += Vector3.right * MoveSpeed * Time.deltaTime;
        //���Ѿ� ������ Ȱ��ȭ�Ǿ� ���� ��
        if (isStrong == true)
            OnHit += (target) =>
            {
                isStrong = false;
                attackDamage *= 1.5f;
            };
       OnHit += (target) =>
        {
            AttackTo(target);
            OnDie();
        };
    }


    private void OnTriggerEnter2D(Collider2D col)  // ����
    {
            Actor act = col.GetComponent<Actor>();

        if (col.gameObject.CompareTag("Enemy") || col.CompareTag("Boss"))
        {
            OnHit(act);
            //OnDeadChecking(coll);
        }
           

            //if (element != Element.Silver && GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet == true) // ��ź
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
                //if (GameManager.instance.player.GetComponent<PlayerGlobal>().synergy == Synergy.������)
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
                //if (GameManager.instance.player.GetComponent<PlayerGlobal>().synergy == Synergy.������)
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
        OnHit = null;
        OnUpdate = null;
        transform.localScale = Vector3.one;
        snoballForce = 0;
        gameObject.SetActive(false);
    }
}
