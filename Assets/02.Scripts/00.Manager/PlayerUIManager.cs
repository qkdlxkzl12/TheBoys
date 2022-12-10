using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [SerializeField]
    private int[] levelUpExps;
    int NeedLevelUpExp { get { return levelUpExps[curLevel-1]; } }
    int curLevel;
    int curExp;
    Slider expBar;
    TextMeshProUGUI levelText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        expBar = transform.GetChild(0).GetComponent<Slider>();
        levelText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        InitData();
    }

    public void GetExp(int exp)
    {
        curExp += exp;
        if (curExp >= NeedLevelUpExp)
        {
            curExp -= NeedLevelUpExp;
            curLevel++;
            levelText.text = $"<b>Lv.{curLevel}";
            expBar.maxValue = NeedLevelUpExp;
            SlotMachineManager.instance.StartMachine();
        }
        expBar.value = curExp;
    }

    void InitData()
    {
        curLevel = 1;
        levelText.text = $"<b>Lv.{curLevel}";
        expBar.maxValue = NeedLevelUpExp;
        expBar.value = curExp;
    }
}
