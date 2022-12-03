using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachineManager : MonoBehaviour
{
    [SerializeField]
    Transform selectBoard;
    const float slotHeight = 78;
    readonly int[] spinAmount = { 15, 26, 34 };
    [SerializeField]
    private RectTransform[] slotAvilityObject; //���ư� ����
    [SerializeField]
    private Button[] slots; //������ Ŭ��\
    [SerializeField]
    GameObject[] avilitySelectButton;
    int[] slotAvilityIndex = new int[3]; //������ Ư�� �ε���

    public CharacteristicInfo[] abilityInfo; //Ư���� �������� ���� ����

    [System.Serializable]
    public class DisplayItemSlot
    {
        public Image[] image;
    }
    public DisplayItemSlot[] displayItemSlots; //������ ������ .. �귿 �ʱ�ȭ �Ҷ� �̹��� �Ѱ��ֱ�
    public Image testImage;


    public void Init()
    {
        Time.timeScale = 0;
        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < slotAvilityObject.Length; i++)
        {
            slotAvilityObject[i].anchoredPosition = Vector3.up * slotHeight * 3;
            //�ӽ�(�������� �ε��� �ο��ؾ���)
            slotAvilityIndex[i] = i;
        }

    }

    public void SelectSlot(int i)
    {
        selectBoard.gameObject.SetActive(true);
        var nameText = selectBoard.GetChild(0).GetComponent<TextMeshProUGUI>();
        var typeText = selectBoard.GetChild(1).GetComponent<TextMeshProUGUI>();
        var loreText = selectBoard.GetChild(2).GetComponent<TextMeshProUGUI>();

        nameText.text = $"<b>{abilityInfo[i].Name}</b>";
        typeText.text = $"<b>{abilityInfo[i].Type}</b>";
        loreText.text = $"<b>{abilityInfo[i].Lore}</b>";
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    public void Run()
    {
        StartCoroutine(SpinSlot(0));
        StartCoroutine(SpinSlot(1));
        StartCoroutine(SpinSlot(2));
    }
        
    IEnumerator SpinSlot(int slotIndex)
    {
        if(slotIndex >= slotAvilityObject.Length )
        {
            Debug.Log("slotIndex is over");
            yield break;
        }
        for (int i = 0; i < spinAmount[slotIndex] * 2; i++)
        {
            slotAvilityObject[slotIndex].localPosition += Vector3.down * slotHeight * 0.5f;
            if (slotAvilityObject[slotIndex].localPosition.y <= 0)
                slotAvilityObject[slotIndex].localPosition += Vector3.up * slotHeight * 3;
            yield return new WaitForSecondsRealtime(.05f); ;
        }

        if(slotIndex == spinAmount.Length - 1)
            for(int i = 0; i < spinAmount.Length; i++ )
            avilitySelectButton[i].gameObject.SetActive(true);
    }
}
