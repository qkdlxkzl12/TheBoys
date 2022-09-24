using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Actor
{
    [SerializeField]
    Element element;
    BulletType type;
    Vector2 direction;
    float value;

    //public EnemyBullet(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp, attackDamage, moveSpeed, attackDelay)
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Test. 속성에 따른 맞는 매소드 전달
        if (element == Element.Fire)
            GameManager.instance.speicalAvility = TestAvility1;
        else if(element == Element.Thunder)
            GameManager.instance.speicalAvility = TestAvility2;
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
