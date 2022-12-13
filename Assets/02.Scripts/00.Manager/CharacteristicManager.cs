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

    [SerializeField]
    private GameObject barrier;
    [SerializeField]
    private GameObject pet;
    Dictionary<CharacteristicInfo,Trait> trait = new Dictionary<CharacteristicInfo, Trait>();
    public bool hasLens;
    public bool hasCrazy;
    public bool isAwakenFire;
    public bool isAwakenBattery;
    public bool isAwakenSnow;
    public bool isAwakenNut;
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
            trait.Add(datas[i],Trait.None + i + 1);
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
                switch (level)
                {
                    case 1:
                        BulletManager.instance.PlayerBulletAdd(Element.Fire, 2);
                        break;
                    case 2:
                        BulletManager.instance.PlayerBulletAdd(Element.Fire, 2);
                        break;
                    case 3:
                        BulletManager.instance.PlayerBulletAdd(Element.Fire, 3);
                        break;
                    case 4:
                        BulletManager.instance.PlayerBulletAdd(Element.Fire, 3);
                        break;
                    case 5:
                        BulletManager.instance.PlayerBulletAdd(Element.Fire, 2);
                        break;
                }
                break;
            case Trait.Snowball:
                switch (level)
                {
                    case 1:
                        BulletManager.instance.PlayerBulletAdd(Element.Snowball, 2);
                        break;
                    case 2:
                        BulletManager.instance.PlayerBulletAdd(Element.Snowball, 2);
                        break;
                    case 3:
                        BulletManager.instance.PlayerBulletAdd(Element.Snowball, 3);
                        break;
                    case 4:
                        BulletManager.instance.PlayerBulletAdd(Element.Snowball, 3);
                        break;
                    case 5:
                        BulletManager.instance.PlayerBulletAdd(Element.Snowball, 5);
                        break;
                }
                break;
            case Trait.Battery:
                switch (level)
                {
                    case 1:
                        BulletManager.instance.PlayerBulletAdd(Element.Battery, 2);
                        break;
                    case 2:
                        BulletManager.instance.PlayerBulletAdd(Element.Battery, 2);
                        break;
                    case 3:
                        BulletManager.instance.PlayerBulletAdd(Element.Battery, 3);
                        break;
                    case 4:
                        BulletManager.instance.PlayerBulletAdd(Element.Battery, 3);
                        break;
                    case 5:
                        BulletManager.instance.PlayerBulletAdd(Element.Fire, 5);
                        break;
                }
                break;
            case Trait.Nut:
                switch (level)
                {
                    case 1:
                        BulletManager.instance.PlayerBulletAdd(Element.Silver, 2);
                        break;
                    case 2:
                        BulletManager.instance.PlayerBulletAdd(Element.Silver, 2);
                        break;
                    case 3:
                        BulletManager.instance.PlayerBulletAdd(Element.Silver, 3);
                        break;
                    case 4:
                        BulletManager.instance.PlayerBulletAdd(Element.Silver, 3);
                        break;
                    case 5:
                        BulletManager.instance.PlayerBulletAdd(Element.Silver, 5);
                        break;
                }
                break;
            case Trait.Crazy:
                hasCrazy = true;
                break;
            case Trait.Lens:
                hasLens = true;
                //BulletManager에서 크기 조정
                break;
            case Trait.Pet:
                pet.SetActive(true);
                //활성화
                break;
            case Trait.Shield:
                barrier.SetActive(true);
                //활성화
                break;
            case Trait.Explosion:
                isAwakenFire = true;
                break;
            case Trait.IceStorm:
                isAwakenSnow = true;
                break;
            case Trait.EleckOverload:
                isAwakenBattery = true;
                break;
            case Trait.GodBless:
                isAwakenNut = true;
                break; 
            default:
                break;
        }
    }

    public void ShieldRecovery(float delay, GameObject shield)
    {
        IEnumerator ShieldRecovery(float delay, GameObject shield)
        {
            if (shield.GetComponent<Barrier>() == null)
                yield break;
            shield.SetActive(false);
            yield return new WaitForSeconds(delay);

            shield.SetActive(true);
        }

        StartCoroutine(ShieldRecovery(delay, shield));
    }

}
