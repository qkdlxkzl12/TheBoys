using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Actor
{
    BulletType type;
    Vector2 moveDirection;
    //Actor.moveSpeed;
    float changeAmount;

    public Bullet() : base() { }

    void Start()
    {
        type = BulletType.STRAIGHT;
        moveDirection = new Vector2(0,-1);
        moveDirection.Normalize();
        moveSpeed = 8;
    }

    public void Init(BulletType _type, Vector2 _moveDirection, int _moveSpeed, float _changeAmount = 0)
    {
        gameObject.SetActive(true);
        if (type == BulletType.ROUND && changeAmount == 0)
        {
            Debug.LogError("[ERROR:002]"+gameObject.name+"'s type is round. but changeAmount is null");
            return;
        }
        type = _type;
        moveDirection = _moveDirection;
        moveDirection.Normalize();
        moveSpeed = _moveSpeed;
        changeAmount = _changeAmount;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection.ToVec3() * moveSpeed * Time.deltaTime;
        if (type == BulletType.ROUND)
        {

        }
    }


}
