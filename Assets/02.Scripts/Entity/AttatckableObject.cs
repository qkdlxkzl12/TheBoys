using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatckableObject : Actor
{
    bool isIgnoringHit;
    public AttatckableObject(int damage,bool isIgnoringHit = false) : base(1, damage) 
    {
        attackDamage = 1;
        this.isIgnoringHit = isIgnoringHit; 
    }

    override protected void Damaged(int value)
    {
        if (isIgnoringHit == true)
            return;
        base.Damaged(value);
    }
}
