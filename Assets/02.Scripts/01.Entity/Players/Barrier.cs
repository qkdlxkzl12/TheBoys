using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Actor
{ 
    public float shieldDelay = 10f; //ΩØµÂ ¡÷±‚

    protected override void Damaged(float value)
    {
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("EnemyBullet") || coll.CompareTag("EnemySkill"))
        {
            CharacteristicManager.instance.ShieldRecovery(shieldDelay, gameObject);
        }
    }
}
