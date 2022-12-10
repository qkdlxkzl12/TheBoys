using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachineManager : MonoBehaviour
{
    public static SlotMachineManager instance;

    [SerializeField]
    Transform selectBoard;
    const float slotHeight = 78;
    readonly int[] selectIndex = { 0,  2, 1 };
    readonly int[] spinAmount = { 15, 22, 29 };
    [SerializeField]
    private RectTransform[] slotAvilityObject; //돌아갈 슬롯들(+이미지)
    [SerializeField]
    private Button[] slots; //슬롯을 클릭
    [SerializeField]
    GameObject[] avilitySelectButton;
    //내용확인 : 테스트용
    [SerializeField]
    private List<CharacteristicInfo> selectAbilityInfo = new List<CharacteristicInfo>(); //특성의 정보들을 갖고 있음

    public List< CharacteristicInfo> abilityInfos; //슬롯의 사진들 .. 룰렛 초기화 할때 이미지 넘겨주기

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        selectAbilityInfo.Add(new CharacteristicInfo());
        selectAbilityInfo.Add(new CharacteristicInfo());
        selectAbilityInfo.Add(new CharacteristicInfo());
    }

    private void InitSlot()
    {
        //중복값을 비교하기 위해 초기화를 해줌
        for(int i = 0; i < selectAbilityInfo.Count; i++)
        {
            selectAbilityInfo[i] = new CharacteristicInfo();
        }
        //슬롯의 이미지를 순차적으로 정함
        for (int i = 0; i < selectAbilityInfo.Count; i++)
        {
            //슬롯에서 멈칠 능력을 정함(선택할 수 있는 능력)
            while (true)
            {
                int rand = Random.Range(0, abilityInfos.Count);
                //중복 선택 안되게 selectAbilityInfo에 contains로 비교 추가(아직 1차 테스트)
                if (abilityInfos[rand].IsCanAcheive() == true && !selectAbilityInfo.Contains(abilityInfos[rand]))
                {
                    //선택 가능한 능력(최대 레벨X, 각성능력은 조건이 맞는가)
                    selectAbilityInfo[i] = abilityInfos[rand];
                    break;
                }
            }
            //슬롯에 들어갈 이미지들을 정함
            List<CharacteristicInfo> list = new List<CharacteristicInfo>();
            for(int j = 0; j < 3; j++)
            {
                Image curImage = slotAvilityObject[i].GetChild(j).GetComponent<Image>();
                if (j == selectIndex[i])
                {
                    curImage.sprite = selectAbilityInfo[i].Image;
                }
                else
                {
                    while (true)
                    {
                        int rand = Random.Range(0, abilityInfos.Count);
                        if (abilityInfos[rand].IsCanAcheive() == true && abilityInfos[rand] != selectAbilityInfo[i] && !list.Contains(abilityInfos[rand]))
                        {
                            curImage.sprite = abilityInfos[rand].Image;
                            list.Add(abilityInfos[rand]);
                            break;
                        }
                    }
                }
            }
            //반복되는 이미지
            Image lastImage = slotAvilityObject[i].GetChild(3).GetComponent<Image>();
            lastImage.sprite = slotAvilityObject[i].GetChild(0).GetComponent<Image>().sprite;
        }
    }
    private void Init()
    {
        Time.timeScale = 0;
        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < slotAvilityObject.Length; i++)
        {
            slots[i].interactable = false;
            avilitySelectButton[i].SetActive(false);
            slotAvilityObject[i].anchoredPosition = Vector3.up * slotHeight * 3;
        }
        InitSlot();
    }
    private void Run(float waitTime)
    {
        StartCoroutine(SpinSlot(0, waitTime));
        StartCoroutine(SpinSlot(1, waitTime));
        StartCoroutine(SpinSlot(2, waitTime));
    }

    //능력 최종 선택
    public void ChooseAbility(int slotIndex)
    {
        selectAbilityInfo[slotIndex].LevelUp();
        selectBoard.gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SelectSlot(int i)
    {
        selectBoard.gameObject.SetActive(true);
        var nameText = selectBoard.GetChild(0).GetComponent<TextMeshProUGUI>();
        var typeText = selectBoard.GetChild(1).GetComponent<TextMeshProUGUI>();
        var loreText = selectBoard.GetChild(2).GetComponent<TextMeshProUGUI>();

        nameText.text = $"<b>{selectAbilityInfo[i].Name}</b>";
        typeText.text = $"<b>{selectAbilityInfo[i].Type}</b>";
        loreText.text = $"<b>{selectAbilityInfo[i].Lore}</b>";
    }

    //슬롯 시작
    public void StartMachine()
    {
        Init();
        Run(1.5f);  
    }
        
    IEnumerator SpinSlot(int slotIndex,float waitTime)
    {
        if(slotIndex >= slotAvilityObject.Length )
        {
            Debug.Log("slotIndex is over");
            yield break;
        }
        yield return new WaitForSecondsRealtime(waitTime);
        for (int i = 0; i < spinAmount[slotIndex] * 2; i++)
        {
            slotAvilityObject[slotIndex].localPosition += Vector3.down * slotHeight * 0.5f;
            if (slotAvilityObject[slotIndex].localPosition.y <= 0)
                slotAvilityObject[slotIndex].localPosition += Vector3.up * slotHeight * 3;
            yield return new WaitForSecondsRealtime(.05f);
        }
        //슬롯이 끝날 때
        if(slotIndex == spinAmount.Length - 1)
            for(int i = 0; i < spinAmount.Length; i++ )
            {
                avilitySelectButton[i].gameObject.SetActive(true);
                slots[i].interactable = true;
            }
    }
}
