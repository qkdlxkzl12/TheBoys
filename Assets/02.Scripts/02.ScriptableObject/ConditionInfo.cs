using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType { Buff, Debuff }

[System.Serializable, CreateAssetMenu(fileName = "ConditionInfo", menuName = "Scriptable Object/ConditionInfo")]
public class ConditionInfo : ScriptableObject
{

    [SerializeField]
    protected string name;
    [SerializeField]
    protected Sprite image;
    [SerializeField]
    protected string lore;
    [SerializeField]
    protected ConditionType type;
    public string Name { get { return name; } }
    public Sprite Image { get { return image; } }
    public string Lore { get { return lore; } }
    public string Type
    {
        get
        {
            switch (type)
            {
                case ConditionType.Buff:
                    return "버프";
                case ConditionType.Debuff:
                    return "디버프";
                default:
                    return "";
            }; 
        } 
    }
}
