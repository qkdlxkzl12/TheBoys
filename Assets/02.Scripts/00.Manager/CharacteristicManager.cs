using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Trait {None,Fireball,Snowball,Battery, Nut, Crazy,Lens,Pet,Shield,Explosion,IceStorm,EleckOverload,GodBless }
public class CharacteristicManager : MonoBehaviour
{
    public static CharacteristicManager instance;
    [SerializeField]
    private CharacteristicInfo[] datas;
    Dictionary<CharacteristicInfo,Trait> trait = new Dictionary<CharacteristicInfo, Trait>();

    public bool isOwnLens;
    public bool isOwnCrazy;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        for (int i = 0; i < datas.Length; i++)
        {
            trait.Add(datas[i],Trait.None + i);
            datas[i].Init();
        }
        
    }

    public void GainTrait(CharacteristicInfo traitInfo, int level)
    {
        switch (trait[traitInfo])
        {
            case Trait.None:
                break;
            case Trait.Fireball:
                //¸¶¹ý È¹µæ
                break;
            case Trait.Snowball:
                //¸¶¹ý È¹µæ
                break;
            case Trait.Battery:
                //¸¶¹ý È¹µæ
                break;
            case Trait.Nut:
                //¸¶¹ý È¹µæ
                break;
            case Trait.Crazy:
                break;
            case Trait.Lens:
                //BulletManager¿¡¼­ Å©±â Á¶Á¤
                break;
            case Trait.Pet:
                //È°¼ºÈ­
                break;
            case Trait.Shield:
                //È°¼ºÈ­
                break;
            case Trait.Explosion:
                break;
            case Trait.IceStorm:
                break;
            case Trait.EleckOverload:
                break;
            case Trait.GodBless:
                break; 
            default:
                break;
        }
    }
    IEnumerator ShieldRecovery(float delay)
    { 
        yield return new WaitForSeconds(delay);

    }
}
