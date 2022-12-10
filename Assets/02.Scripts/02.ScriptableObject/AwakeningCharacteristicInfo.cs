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
        base.type = CharacteristicType.Awakening;
        base.Awake();
    }
    public override bool IsCanAcheive()
    {
        if(base.IsCanAcheive() == false)
            return false;
        foreach(var characteristic in baseCharacteristic)
        {
            if(characteristic.IsSatisfyAwaken() == false)
                return false;
        }
        return true;
    }
}
