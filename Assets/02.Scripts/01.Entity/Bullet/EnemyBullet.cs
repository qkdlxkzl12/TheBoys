using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBullet : Bullet
{
    private BulletType type;
    private Vector2 moveDirection;
    private float changeAmount;
    private Action duringFiring;
    private Coroutine coroutine;

    private void Update()
    {

        if (duringFiring != null)
        {
            if(coroutine == null)
                duringFiring();
        }
        else
        {
            Debug.LogError("[ERROR:003]:Bullet's fring method is null");
        }
    }

    private void InitState(Vector3 spawnPos, BulletType type, Vector2 moveDirection, int moveSpeed, float changeAmount = 0)
    {
        if (type == BulletType.Round && changeAmount == 0)
        {
            Debug.LogError("[ERROR:002]" + gameObject.name + "'s type is round. but changeAmount is null");
            return;
        }
        gameObject.SetActive(true);
        transform.position = spawnPos;
        this.type = type;
        this.moveDirection = moveDirection;
        this.moveDirection.Normalize();
        this.moveSpeed = moveSpeed;
        this.changeAmount = changeAmount;
        duringFiring = () => transform.position += this.moveDirection.ToVec3() * this.moveSpeed * Time.deltaTime;
    }
    public EnemyBullet SetModeStaright(Vector3 spawnPos, Vector2 moveDir, int moveSpeed)
    {
        InitState(spawnPos, BulletType.Straight, moveDir, moveSpeed);
        return this;
    }

    public EnemyBullet SetModeTargeting(Vector3 spawnPos, int moveSpeed)
    {
        Vector3 targetDir = spawnPos.DistanceWithTarget();
        targetDir.Normalize();
        InitState(spawnPos, BulletType.Target, targetDir, moveSpeed);
        return this;
    }

    public EnemyBullet SetModeWave(Vector3 spawnPos, Vector2 moveDir, int moveSpeed, float chageAmount)
    {
        InitState(spawnPos, BulletType.Round, moveDir, moveSpeed, chageAmount);
        duringFiring += () =>
        {
            //현재 방향을 Vector2.Down 기준의 각도를 구함
            //구한 각도에 회전량을 더해줌
            Vector3 newDirection = moveDirection.AddDegree(this.changeAmount * Time.deltaTime);

            moveDirection = newDirection.normalized;
        };
        return this;
    }

    //미작성
    public EnemyBullet SetModeTracking(Vector3 spawnPos, int moveSpeed, Vector2 startDir)
    {
        startDir.Normalize();
        moveDirection = spawnPos;
        InitState(spawnPos, BulletType.Target, startDir, moveSpeed);
        
        duringFiring += () =>
        {
            if (transform.position.x - 1.5 > GameManager.instance.player.transform.position.x)
            {
                float distanceY = Mathf.Abs(transform.position.y - GameManager.instance.target.transform.position.y) * 0.6f;
                Vector3 targetV = transform.position.DistanceWithTarget();
                targetV.Normalize();
                Vector3 newDirection = Vector3.Slerp(moveDirection, targetV, 2 * Time.deltaTime * distanceY);
                moveDirection = newDirection.normalized;
            }
        };
        return this;
    }

    public void SetPositioning(Vector2 movementValue, float sec)
    {
        coroutine = StartCoroutine(Positioning(transform.position, transform.position.ToVec2() + movementValue, sec));
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
        if(type == BulletType.Target)
        {
            //자리를 잡으면 타겟 방향으로 발사될 때 경로를 재조정해줌
            Vector3 targetDir = GameManager.instance.target.transform.position - transform.position;
            targetDir.Normalize();
            moveDirection = targetDir;
        }
        coroutine = null;
    }

    public void ExstraMove(Vector2 vec)
    {

    }

    override protected void OnDie()
    {
        gameObject.SetActive(false);
        duringFiring = null;
        StopAllCoroutines();
        //Debug.Log(gameObject.name + "를 숨김");
    }
}
