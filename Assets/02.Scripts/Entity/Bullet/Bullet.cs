using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : Actor
{
    private BulletType type_;
    private Vector2 moveDirection_;
    //Actor.moveSpeed_;
    private float changeAmount_;
    private Action duringFiring;
    private Coroutine coroutine;

    public Bullet() : base() { }

    void Start()
    {

    }
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
    //�Ѿ��� �߻��ϱ����� ���� ����
    public void InitState(Vector3 spawnPos, BulletType type, Vector2 moveDirection, int moveSpeed, float changeAmount = 0)
    {
        if (type == BulletType.Round && changeAmount == 0)
        {
            Debug.LogError("[ERROR:002]" + gameObject.name + "'s type is round. but changeAmount is null");
            return;
        }
        gameObject.SetActive(true);
        transform.position = spawnPos;
        type_ = type;
        moveDirection_ = moveDirection;
        moveDirection_.Normalize();
        moveSpeed_ = moveSpeed;
        changeAmount_ = changeAmount;
        duringFiring = () => transform.position += moveDirection_.ToVec3() * moveSpeed_ * Time.deltaTime;
        if (type_ == BulletType.Round)
        {
            duringFiring += () =>
            {
                //���� ������ Vector2.Down ������ ������ ����
                //���� ������ ȸ������ ������
                Vector3 direction = Quaternion.AngleAxis(changeAmount_ * Time.deltaTime, Vector3.forward) * moveDirection_;
                moveDirection_ = direction;
                moveDirection_.Normalize();
            };
        }
    }

    public void SetPositioning(Vector2 firingPos, float sec)
    {
        coroutine = StartCoroutine(Positioning(transform.position, firingPos, sec));
    }
    
    IEnumerator Positioning(Vector2 startPos, Vector2 finishedPos, float sec)
    {
        float elapsedTime = 0;
        while (elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, finishedPos, elapsedTime / sec);
            yield return null;
        }
        coroutine = null;
    }


    override protected void OnDie()
    {
        gameObject.SetActive(false);
        duringFiring = null;
        Debug.Log(gameObject.name + "�� ����");
    }

    private void OnBecameInvisible()
    {
        OnDie();
    }
}
