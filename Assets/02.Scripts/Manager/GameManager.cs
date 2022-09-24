using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Element { Fire, Thunder } //불, 번개
public enum BulletType { STRAIGHT, WAVE, TRACKING, } //일자, 파형, 반유도, 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action<GameObject> speicalAvility;
    [SerializeField]
    List<GameObject> L_enemyBullets;
    [SerializeField]
    GameObject Test1;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Test. 임시 속성 매소드 Action 실행
        if(speicalAvility != null)
        {
            speicalAvility(L_enemyBullets[0]);
        }

    }

}
