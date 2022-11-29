using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JafarLazer : AttatckableObject
{
    JafarLazer() : base(1,true)
    {

    }

    public Transform target;

    public void Fire(float time)
    {
        gameObject.SetActive(true);
        StartCoroutine(lazer(time));
    }

    IEnumerator lazer(float time)
    {
        float renameTime = time;
        while(renameTime > 0)
        {
            renameTime -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Actor actor = col.GetComponent<Actor>();
        if(actor == null)
        {
            Debug.LogError("Collision with non-Actor.");
            return;
        }    
        if(actor.CompareTag("Player"))
        {
            Debug.Log("Hit!");
            var player = actor as PlayerGlobal;
            this.AttackTo(actor);
        }
    }
}
