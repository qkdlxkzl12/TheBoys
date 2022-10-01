using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    Element element_;
    BulletType type_;
    Vector2 direction_;
    float value_;

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
        element_ = element;
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
