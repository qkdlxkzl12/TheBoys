using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public Element element;
    public BulletType type;

    //public Sprite

    Vector2 direction;
    float value;

    float DeadTime = 15f;

    //public EnemyBullet(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp, attackDamage, moveSpeed, attackDelay)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Element element)
    {
        this.element = element;
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
