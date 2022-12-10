using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : Item
{
    int expAmount;
    Exp() : base(ItemType.Exp) { }
    public new void Start()
    {
        base.Start();
    }
    // Start is called before the first frame update

    public void Activate(int amount)
    {
        expAmount = amount;
        InitAnimPlay();
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            PlayerUIManager.instance.GetExp(expAmount);
            gameObject.SetActive(false);
        }
    }
}
