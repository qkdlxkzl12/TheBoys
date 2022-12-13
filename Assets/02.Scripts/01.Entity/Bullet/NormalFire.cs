using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFire : Actor
{
    public GameObject me;
    public float Speed;

    private void Start()
    {
        Invoke("Suicide", 6f);
        moveSpeed = Speed;
    }

    void FixedUpdate()
    {
        me.transform.Translate(Vector2.up * moveSpeed * Time.deltaTime * increaseAttackSpeed );
    }

    public void Init(float incAttackSpeed, bool isBroken = false)
    {
        increaseAttackSpeed = incAttackSpeed;
        int rand = UnityEngine.Random.Range(0, 100);
        if (isBroken == true && rand < 40)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            gameObject.layer = 13;
        }
    }

    void Suicide()
    {
        Destroy(me);
    }

    private void OnBecameInvisible()
    {
        Destroy(me);
    }

}

