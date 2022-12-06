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

        if(GameManager.instance.player.GetComponent<PlayerGlobal>().trait == Trait.돋보기) //돋보기 특성
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


    private void OnTriggerEnter2D(Collider2D coll)  // 피격 혹은 획득
    {
        if (coll.gameObject.CompareTag("Player") || coll.gameObject.CompareTag("Item") || coll.gameObject.CompareTag("Untagged"))
        {
            
        }
        else
        {
            Actor act = coll.GetComponent<Actor>();

            AttackTo(act);

            if(GameManager.instance.player.GetComponent<PlayerGlobal>().trait == Trait.돋보기)
            {
                AttackTo(act); //데미지 증가 (일단 이렇게 구현)
            }

            if (coll.gameObject.CompareTag("EnemyBullet"))
                Destroy(coll.gameObject);
            Destroy(me.gameObject);
        }
    }


    //Test. 임시 불속성 매소드
    void TestAvility1(GameObject obj)
    {
        Debug.Log("불 속성 전이. 전달자: " + obj.name);
    }

    //TEst. 임시 번개속성 매소드
    void TestAvility2(GameObject obj)
    {
        Debug.Log("번개 속성 전이. 전달자: " + obj.name);
    }
}
