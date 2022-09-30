using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : Actor
{
    BulletType type;
    Vector2 moveDirection;
    //Actor.moveSpeed;
    float changeAmount;
    Action duringFiring;

    public Bullet() : base() { }

    void Start()
    {
    }

    public void InitState(Vector3 _pos, BulletType _type, Vector2 _moveDirection, int _moveSpeed, float _changeAmount = 0)
    {
        gameObject.SetActive(true);
        transform.position = _pos;
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
        duringFiring = () => transform.position += moveDirection.ToVec3() * moveSpeed * Time.deltaTime;
        if (type == BulletType.ROUND)
        {
            float radian = Mathf.Deg2Rad * _changeAmount;
            duringFiring += () => {
                //현재 방향을 Vector2.Down 기준의 각도를 구함
                //구한 각도에 회전량을 더해줌
                float angle = moveDirection.AngleBetween(Vector2.down) + changeAmount;
                angle *= Mathf.Deg2Rad;
                Vector3 direction = Quaternion.AngleAxis(changeAmount * Time.deltaTime, Vector3.forward) * moveDirection;
                moveDirection = direction;
                moveDirection.Normalize();
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (duringFiring != null)
        {
            duringFiring();
        }else
        {
            Debug.LogError("[ERROR:003]:Bullet's fring method is null");
        }
    }

    override protected void OnDie()
    {
        gameObject.SetActive(false);
        Debug.Log(gameObject.name + "를 숨김");
    }

    private void OnBecameInvisible()
    {
        OnDie();
    }
}
