using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    //public Enemy(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp,attackDamage,moveSpeed, attackDelay) { }
    public Enemy(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(100, 5, 3, 0.3f) { }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
    }
}
