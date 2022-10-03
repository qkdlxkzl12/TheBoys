using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Element { Iron, Thunder, Battery, Fire, Snowball } //은, 번개, 배터리, 화염, 눈덩이
public enum BulletType { Straight, Target, Round, Tracking, } //일자, 변형, 반유도, 

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

    //유도의 대상을 갱신 시켜줌
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
    //벡터2를 벡터2로 변환
    public static Vector3 ToVec3(this Vector2 vec2, float z = 0)
    {
        return new Vector3(vec2.x, vec2.y, z);
    }
    //벡터3을 벡터2로 변환
    public static Vector2 ToVec2(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }
    //방향벡터에 각도 만큼 더한 방향 벡터를 반환
    public static Vector2 AddDegree(this Vector2 vec2, float degree)
    {
        return Quaternion.AngleAxis(degree, Vector3.forward) * vec2;
    }
}
