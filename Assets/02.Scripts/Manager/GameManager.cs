using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Element { Iron, Thunder, Battery, Fire, Snowball } //��, ����, ���͸�, ȭ��, ������
public enum BulletType { Straight, Target, Round, Tracking, } //����, ����, ������, 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action<GameObject> speicalAvility;
    public GameObject target { get; set; }
    public GameObject player { get; set; }

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
        ReloadTarget();
    }
    void Start()
    {
        Test();
    }

    void Update()
    {
    }

    //������ ����� ���� ������
    public void ReloadTarget()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Target");
        this.target = target != null ? target : player;
    }
    public void Test()
    {
        Vector3 vec3 = new Vector3(8, 0, 0);
        Vector2 vec2 = new Vector2(1, -2);
        int speed = 13;
        //for (int i = 0; i < 10; i++)
        //{
        //    var b = BulletManager.instace.SelectBullet().SetModeTargeting(vec3, speed);
        //    Vector2 v = new Vector2(Mathf.Sin(Mathf.Deg2Rad * 36 * i), Mathf.Cos(Mathf.Deg2Rad * 36 * i));
        //    b.SetPositioning(v * 3, 1f); 
        //}
        //BulletManager.instace.SelectBullet().SpawnBulletStaright(vec3, vec2, speed);
        BulletManager.instace.SelectBullet().SetModetWave(vec3, vec2, speed, 30);
    }
}

public static class GameManagerExtensions
{
    //����2�� ����2�� ��ȯ
    public static Vector3 ToVec3(this Vector2 vec2, float z = 0)
    {
        return new Vector3(vec2.x, vec2.y, z);
    }
    //����3�� ����2�� ��ȯ
    public static Vector2 ToVec2(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }
    //���⺤�Ϳ� ���� ��ŭ ���� ���� ���͸� ��ȯ
    public static Vector2 AddDegree(this Vector2 vec2, float degree)
    {
        return Quaternion.AngleAxis(degree, Vector3.forward) * vec2;
    }
}
