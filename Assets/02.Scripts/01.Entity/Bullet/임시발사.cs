using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 임시발사 : MonoBehaviour
{
    public GameObject me;

    void FixedUpdate()
    {
        me.transform.Translate(Vector2.up * 40 * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(me);
    }

}

