using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    GameObject expPrefab;
    [SerializeField]
    GameObject traitOrbsPrefab;
    static public ItemManager instance;
    List<Exp> expOrbs = new List<Exp>();
    List<TraitOrb> traitOrbs = new List<TraitOrb>();
    Transform parent;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        parent = GameObject.Find("Items").transform;
    }
    public void Test2()
    {
        CreateTraitOrb(Vector3.right * 7);
    }

    public void Test(int value)
    {
        CreateExpOrb(Vector3.right * 7, value);
    }


    public void CreateExpOrb(Vector3 pos,int expAmount)
    {
        foreach (Exp expOrb in expOrbs)
        {
            if(expOrb.gameObject.activeSelf == false)
            {
                Debug.Log("ADD");
                expOrb.transform.position = pos;
                expOrb.Activate(expAmount);
                return;
            }
        }
        Exp newExpOrb = Instantiate(expPrefab, parent).GetComponent<Exp>();
        expOrbs.Add(newExpOrb);
        newExpOrb.transform.position = pos;
        newExpOrb.Activate(expAmount);
    }

    public void CreateTraitOrb(Vector3 pos)
    {
        foreach (TraitOrb traitOrb in traitOrbs)
        {
            if (traitOrb.gameObject.activeSelf == false)
            {
                traitOrb.transform.position = pos;
                traitOrb.Activate();
                return;
            }
        }
        TraitOrb newTraitOrb = Instantiate(traitOrbsPrefab, parent).GetComponent<TraitOrb>();
        traitOrbs.Add(newTraitOrb);
        newTraitOrb.transform.position = pos;
        newTraitOrb.Activate();
    }

}
