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

    //public anime

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

        if(GameManager.instance.player.GetComponent<PlayerGlobal>().trait == Trait.������) //������ Ư��
        {
            me.transform.localScale = new Vector2(me.transform.localScale.x * 2, me.transform.localScale.y * 2);
        }    

    }


    void FixedUpdate()
    {
        
        me.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);

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

        }
    }


    private void OnTriggerEnter2D(Collider2D coll)  // �ǰ� Ȥ�� ȹ��
    {
        if (coll.gameObject.CompareTag("Player") || coll.gameObject.CompareTag("Item") || coll.gameObject.CompareTag("Untagged"))
        {
            
        }
        else
        {
            Actor act = coll.GetComponent<Actor>();

            AttackTo(act);

            if(GameManager.instance.player.GetComponent<PlayerGlobal>().trait == Trait.������)
            {
                AttackTo(act); //������ ���� (�ϴ� �̷��� ����)
            }

            if (coll.gameObject.CompareTag("EnemyBullet"))
                Destroy(coll.gameObject);
            Destroy(me.gameObject);
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
