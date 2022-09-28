using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instace;
    private void Awake()
    {
        if(instace == null)
            instace = this;
        else
            Destroy(this);
    }

    

}
