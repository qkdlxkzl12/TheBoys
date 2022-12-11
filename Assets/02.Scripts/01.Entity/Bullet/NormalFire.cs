using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFire : MonoBehaviour
{
    public GameObject me;
    public float Speed;

    private void Start()
    {
        Invoke("Suicide", 6f);
    }

    void FixedUpdate()
    {
        me.transform.Translate(Vector2.up * Speed * Time.deltaTime);
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

