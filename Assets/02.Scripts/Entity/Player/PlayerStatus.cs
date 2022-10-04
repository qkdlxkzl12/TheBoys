using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Actor
{
    static public int Player_HP;

    public GameObject Player;

    void Start()
    {
        Player_HP = hp_;
    }


    void Update()
    {
        if (Player_HP <= 0)
        {
            Debug.Log("Player Dead");
            Destroy(Player);
        }
    }
}
