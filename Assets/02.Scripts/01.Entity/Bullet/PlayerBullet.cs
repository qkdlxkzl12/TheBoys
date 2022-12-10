using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public GameObject me;

    public Element element;
    public BulletType type;

    public float MoveSpeed;

    bool SnowBall = false; //����캼
    float SnowBallScale = 1f;

    Vector2 direction;
    float value;


    //public EnemyBullet(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp, attackDamage, moveSpeed, attackDelay)
    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Start()
    {
        me.transform.parent = null;
        Init(element);

        if (GameManager.instance.player.GetComponent<PlayerGlobal>().trait == Trait.������) //������ Ư��
        {
            me.transform.localScale = new Vector2(me.transform.localScale.x * 2, me.transform.localScale.y * 2);

            if (GameManager.instance.player.GetComponent<PlayerGlobal>().synergy == Synergy.������) //������ �ó���
            {
                me.transform.localScale = new Vector2(me.transform.localScale.x * 1.5f, me.transform.localScale.y * 1.5f);
            }
        }
    }


    void FixedUpdate()
    {
        me.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);

        SnowBallScale += (Time.deltaTime * 0.025f);

        if(SnowBall)
        {
            if(SnowBallScale != 0)
                me.transform.localScale = new Vector2(me.transform.localScale.x * SnowBallScale, me.transform.localScale.y * SnowBallScale);
        }

    }


    public void Init(Element element)
    {
        this.element = element;
        
        if(element == Element.Normal)
        {
            //?
        }
        else if (element == Element.Silver)
        {
            GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet = true;
        }
        else if (element == Element.Thunder)
        {

        }
        else if (element == Element.Battery)
        {

        }
        else if (element == Element.Fire)
        {

        }
        else if (element == Element.Snowball)
        {
            SnowBall = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D coll)  // �ǰ� Ȥ�� ȹ��
    {
        if (coll.gameObject.CompareTag("Player") || coll.gameObject.CompareTag("Item") || coll.gameObject.CompareTag("Untagged") || coll.gameObject.CompareTag("PlayerBullet"))
        {
            
        }
        else
        {
            Actor act = coll.GetComponent<Actor>();

            if (coll.gameObject.CompareTag("Enemy"))
            {
                OnDeadChecking(coll);
            }

            AttackTo(act);

            if(element == Element.Snowball)
            {
                AttackTo(act); //������ ���� (�ϴ� �̷��� ����)

                if (GameManager.instance.player.GetComponent<PlayerGlobal>().synergy == Synergy.������)
                {
                    AttackTo(act); //������ ���� (�ϴ� �̷��� ����)
                }
            }

            if (element != Element.Silver && GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet == true) // ��ź
            {
                GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet = false;
                AttackTo(act);
            }

            if(coll.gameObject.CompareTag("Enemy") || coll.gameObject.CompareTag("Boss"))
                Destroy(me.gameObject);
        }
    }


    void OnDeadChecking(Collider2D coll)
    {
        Actor act = coll.GetComponent<Actor>();

        if (element != Element.Silver && GameManager.instance.player.GetComponent<PlayerGlobal>().Silver_Bullet == true)
        {
            if (element == Element.Snowball)
            {
                if (GameManager.instance.player.GetComponent<PlayerGlobal>().synergy == Synergy.������)
                {
                    if (act.curHp <= 4)
                    {
                        act.curHp = 666;
                        coll.gameObject.GetComponent<Enemy>().OnDead();
                    }
                }
                else
                    if (act.curHp <= 3)
                {
                    act.curHp = 666;
                    coll.gameObject.GetComponent<Enemy>().OnDead();
                }
            }
            else
                if (act.curHp <= 2)
            {
                act.curHp = 666;
                coll.gameObject.GetComponent<Enemy>().OnDead();
            }
        }
        else
        {
            if (element == Element.Snowball)
            {
                if (GameManager.instance.player.GetComponent<PlayerGlobal>().synergy == Synergy.������)
                {
                    if (act.curHp <= 3)
                    {
                        act.curHp = 666;
                        coll.gameObject.GetComponent<Enemy>().OnDead();
                    }
                }
                else
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

    //Test. �ӽ� �ҼӼ� �żҵ�
    void TestAvility1(GameObject obj)
    {
        Debug.Log("�� �Ӽ� ����. ������: " + obj.name);
    }

    //TEst. �ӽ� �����Ӽ� �żҵ�
    void TestAvility2(GameObject obj)
    {
        Debug.Log("���� �Ӽ� ����. ������: " + obj.name);
    }
}
