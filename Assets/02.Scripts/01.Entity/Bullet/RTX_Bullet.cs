using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RTX_Bullet : Bullet
{
    public float MoveSpeed;

    Action<Actor> OnHit;

    private void Start()
    {
        attackDamage = 20;

        OnHit += (target) =>
        {
            AttackTo(target);
            OnDie();
        };
    }

    private void OnTriggerEnter2D(Collider2D col)  // АјАн
    {
        Actor act = col.GetComponent<Actor>();

        if (col.gameObject.CompareTag("Enemy") || col.CompareTag("Boss"))
        {
            if (OnHit == null)
                Debug.Log("SUPER ERROR" + gameObject.name + Time.realtimeSinceStartup);
            OnHit(act);
        }
    }

    void FixedUpdate()
    {
        gameObject.transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
    }
}
