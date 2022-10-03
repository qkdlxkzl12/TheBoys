using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Actor
{
   

    public Bullet() : base() { }

    void Start()
    {

    }
    
    //총알이 발사하기위해 값들 조정
    



    private void OnBecameInvisible()
    {
        OnDie();
    }
}
