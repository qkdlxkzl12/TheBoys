using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CharacteristicType { Magic, Passive, Awakening }


[System.Serializable, CreateAssetMenu(fileName = "CharacteristicInfo", menuName = "Scriptable Object/CharacteristicInfo")]
public class CharacteristicInfo : ScriptableObject
{
    [SerializeField]
    protected int id;
    public int Id { get { return id; } }
    [SerializeField]
    protected Sprite image;
    public Sprite Image { get { return image; } }
    [SerializeField]
    protected string name;
    public string Name { get { return (curLevel + 1 == maxLevel ? "Max " : $"Lv{curLevel+1} ") + name; }  }
    [SerializeField]
    protected CharacteristicType type;
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
                case CharacteristicType.Awakening:
                    return "초월";
                default:
                    return "";
            }
        }
    }
    protected int maxLevel;
    [SerializeField]
    protected int curLevel = 0;
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

    protected void Awake()
    {
        switch (type)
        {
            case CharacteristicType.Magic:
                maxLevel = 5;
                break;
            case CharacteristicType.Passive:
                maxLevel = 1;
                break;
            case CharacteristicType.Awakening:
                maxLevel = 1;
                break;
            default:
                maxLevel = -1;
                break;
        }
    }

    public void Init()
    {
        curLevel = 0;
    }
    public bool IsSatisfyAwaken()
    {
        switch (type)
        {
            case CharacteristicType.Magic:
                    return curLevel >= 3 ? true : false;
            case CharacteristicType.Passive:
                return curLevel >= 1 ? true : false;
            case CharacteristicType.Awakening:
                Debug.LogError("얘기치않은 접근이 발견되었습니다.(특성)");
                return false;
            default:
                return false;

        }
    }

    public virtual bool IsCanAcheive()
    {
            return curLevel < maxLevel;
    }

    public void LevelUp()
    {
        curLevel++;
        CharacteristicManager.instance.GainTrait(this,curLevel);
        //특성 레벨 효과 부여
    }
}
