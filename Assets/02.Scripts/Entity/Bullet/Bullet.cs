using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Actor
{
   

    public Bullet() : base() { }

    void Start()
    {

    }
    
    //�Ѿ��� �߻��ϱ����� ���� ����
    



    private void OnBecameInvisible()
    {
        OnDie();
    }
}
