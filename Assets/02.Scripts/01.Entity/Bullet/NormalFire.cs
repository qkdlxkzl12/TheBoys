using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFire : MonoBehaviour
{
    public GameObject me;
    public float Speed;

    void FixedUpdate()
    {
        me.transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(me);
    }

}

