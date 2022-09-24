using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Element { Fire, Thunder } //��, ����
public enum BulletType { STRAIGHT, WAVE, TRACKING, } //����, ����, ������, 

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
        //Test. �ӽ� �Ӽ� �żҵ� Action ����
        if(speicalAvility != null)
        {
            speicalAvility(L_enemyBullets[0]);
        }

    }

}
