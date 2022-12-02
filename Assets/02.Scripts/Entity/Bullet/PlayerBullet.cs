using System.Collections;
using System.Collections.Generic;
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
        else if (element == Element.Iron)
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
