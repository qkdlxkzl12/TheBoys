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
    private RectTransform[] slotAvilityObject; //���ư� ���Ե�(+�̹���)
    [SerializeField]
    private Button[] slots; //������ Ŭ��
    [SerializeField]
    GameObject[] avilitySelectButton;
    //����Ȯ�� : �׽�Ʈ��
    [SerializeField]
    private List<CharacteristicInfo> selectAbilityInfo = new List<CharacteristicInfo>(); //Ư���� �������� ���� ����

    public List< CharacteristicInfo> abilityInfos; //������ ������ .. �귿 �ʱ�ȭ �Ҷ� �̹��� �Ѱ��ֱ�

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
        //�ߺ����� ���ϱ� ���� �ʱ�ȭ�� ����
        for(int i = 0; i < selectAbilityInfo.Count; i++)
        {
            selectAbilityInfo[i] = new CharacteristicInfo();
        }
        //������ �̹����� ���������� ����
        for (int i = 0; i < selectAbilityInfo.Count; i++)
        {
            //���Կ��� ��ĥ �ɷ��� ����(������ �� �ִ� �ɷ�)
            while (true)
            {
                int rand = Random.Range(0, abilityInfos.Count);
                //�ߺ� ���� �ȵǰ� selectAbilityInfo�� contains�� �� �߰�(���� 1�� �׽�Ʈ)
                if (abilityInfos[rand].IsCanAcheive() == true && !selectAbilityInfo.Contains(abilityInfos[rand]))
                {
                    //���� ������ �ɷ�(�ִ� ����X, �����ɷ��� ������ �´°�)
                    selectAbilityInfo[i] = abilityInfos[rand];
                    break;
                }
            }
            //���Կ� �� �̹������� ����
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
            //�ݺ��Ǵ� �̹���
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

    //�ɷ� ���� ����
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

    //���� ����
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
        //������ ���� ��
        if(slotIndex == spinAmount.Length - 1)
            for(int i = 0; i < spinAmount.Length; i++ )
            {
                avilitySelectButton[i].gameObject.SetActive(true);
                slots[i].interactable = true;
            }
    }
}
