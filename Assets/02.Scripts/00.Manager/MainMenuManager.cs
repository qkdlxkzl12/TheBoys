using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    RectTransform titleUI;
    [SerializeField]
    Transform playerUI;

    [SerializeField]
    float[] playerStartY;
    [SerializeField]
    float[] playerEndY;
    int startIndex, endIndex;
    float playerWatTime;


    Sequence playerMoveSeq;
    Sequence titleShakeSeq;

    private void Awake()
    {
        DOTween.Init();
    }

    private void Start()
    {
        playerMoveSeq = DOTween.Sequence()
        .SetAutoKill(false)
        .OnPlay(() =>
        {
        startIndex = Random.Range(0, playerStartY.Length);
        endIndex = Random.Range(0, playerStartY.Length);
            playerWatTime = Random.Range(1,4);
            Debug.Log(startIndex);
            playerUI.position = new Vector3(-13, playerStartY[startIndex], 0);
        })
        .Append(playerUI.DOMoveY(playerEndY[endIndex], 2f).SetEase(RandomEase()))
        .Join(playerUI.DOMoveX(13, 2f).SetEase(Ease.InSine))
        .AppendInterval(playerWatTime * 5 + 7)
        .OnComplete(() => { playerMoveSeq.Restart(); });
        //.SetLoops(-1, LoopType.Restart);

        titleShakeSeq = DOTween.Sequence()
        .Append(DOTween.Sequence()
            .SetAutoKill(false)
            .Append(DOTween.To(() => 0, rot => titleUI.rotation = Quaternion.Euler(new Vector3(0, 0, rot)), 8, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo))
            .Append(DOTween.To(() => 0, rot => titleUI.rotation = Quaternion.Euler(new Vector3(0, 0, rot)), -8, 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo))
            .SetLoops(2, LoopType.Restart))
        .AppendInterval(6)
        .SetLoops(-1, LoopType.Restart);

        //titleUI.transform.DOShakeRotation(1f,vibrato: 3,randomness: 30).SetDelay(1);
    }

    Ease RandomEase(bool isOut = false)
    {
        int rand = Random.Range(1, 11);
        if(isOut == true)
            return Ease.Linear + rand * 3;
        else
            return Ease.Linear + rand * 3 + 1;
    }


}
