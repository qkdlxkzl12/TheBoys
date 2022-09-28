using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Element { Iron, Thunder, Battery, Fire, Snowball } //��, ����, ���͸�, ȭ��, ������
public enum BulletType { STRAIGHT, ROUND, TRACKING, } //����, ����, ������, 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action<GameObject> speicalAvility;
    List<GameObject> l_enemyBullets;
    List<GameObject> l_playerBullets;
    GameObject target;
    GameObject player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        ReloadTarget();
    }

    void Update()
    { 
    }

    public void Init()
    {
        
    }

    //������ ����� ���� ������
    public void ReloadTarget()
    {
        GameObject _target = GameObject.FindGameObjectWithTag("target");
        target = _target != null ? _target : player;
    }
}

public static class GameManagerExtensions
{ 
    public static Vector3 ToVec3(this Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y, 0);
    }
}
