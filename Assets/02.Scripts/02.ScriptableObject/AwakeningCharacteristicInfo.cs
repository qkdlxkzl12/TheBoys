using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "AwakeningCharacteristicInfo", menuName = "Scriptable Object/AwakeningCharacteristicInfo")]
public class AwakeningCharacteristicInfo : CharacteristicInfo
{
    [SerializeField]
    CharacteristicInfo[] baseCharacteristic;
    private void Awake()
    {
        if (baseCharacteristic.Length != 2)
        {
            Debug.LogError("Basee characteristic is stranged.");
        }
        base.Awake();
        base.type = CharacteristicType.Awakening;
    }
    public override bool IsCanAcheive()
    {
        foreach(var characteristic in baseCharacteristic)
        {
            if(characteristic.IsSatisfyAwaken() == false)
                return false;
        }
        return true;
    }
}
