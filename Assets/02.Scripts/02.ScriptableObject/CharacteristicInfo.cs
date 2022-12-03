using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CharacteristicType { Magic, Passive }


[System.Serializable, CreateAssetMenu(fileName = "BossInfo", menuName = "Scriptable Object/BossInfo")]
public class CharacteristicInfo : ScriptableObject
{
    [SerializeField]
    private Sprite image;
    private Sprite Image { get { return image; } }
    [SerializeField]
    private string name;
    public string Name { get { return name; }  }
    [SerializeField]
    private CharacteristicType type;
    public string Type
    {
        get
        {
            switch (type)
            {
                case CharacteristicType.Magic:
                    return "마법";
                case CharacteristicType.Passive:
                    return "고유";
                default:
                    return "";
            }
        } 
    }
    private int maxLevel;
    [SerializeField]
    private int curLevel = 0;
    [SerializeField, Multiline(3)]
    private string[] lore;
    public string Lore
    {
        get
        {
            if (curLevel < lore.Length)
                return lore[curLevel];
            else
                return "";
        } 
    }
   
}
