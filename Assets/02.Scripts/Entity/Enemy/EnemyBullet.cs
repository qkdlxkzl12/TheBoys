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
        //Test. �Ӽ��� ���� �´� �żҵ� ����
        if (element == Element.Fire)
            GameManager.instance.speicalAvility = TestAvility1;
        else if(element == Element.Thunder)
            GameManager.instance.speicalAvility = TestAvility2;
    }

    public void Init(Element element)
    {
        this.element = element;
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
