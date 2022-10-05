using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public GameObject Player;

    private void OnCollisionEnter2D(Collision2D coll)
    {
      
        if (coll.gameObject.CompareTag("EnemyBullet"))
        {

            PlayerStatus.Player_HP -= 1;

            Destroy(coll.gameObject);
        }

    }

}
