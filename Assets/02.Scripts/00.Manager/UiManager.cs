using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [SerializeField]
    GameObject bossUI;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Slider InitBossUi(string name, int maxHp)
    {
        Slider hpBar = bossUI.transform.GetComponentInChildren<Slider>();
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;
        TextMeshProUGUI tMesh = bossUI.transform.GetComponentInChildren<TextMeshProUGUI>();
        tMesh.text = name;
        bossUI.SetActive(true);
        return hpBar;
    }

    public void ReleaseBossUi()
    {
        bossUI.SetActive(false);
    }
}
