using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitOrb : Item
{
    TraitOrb() : base(ItemType.TraitOrb) { }
    public void Activate()
    {
        InitAnimPlay();
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            SlotMachineManager.instance.StartMachine();
            gameObject.SetActive(false);
        }
    }

}
