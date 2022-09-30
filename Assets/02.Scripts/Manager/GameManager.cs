using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Element { Iron, Thunder, Battery, Fire, Snowball } //은, 번개, 배터리, 화염, 눈덩이
public enum BulletType { STRAIGHT, ROUND, TRACKING, } //일자, 변형, 반유도, 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action<GameObject> speicalAvility;
    public GameObject target { get; set; }
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
        ReloadTarget();
    }
    void Start()
    {
    }

    void Update()
    {
    }

    //유도의 대상을 갱신 시켜줌
    public void ReloadTarget()
    {
        GameObject _target = GameObject.FindGameObjectWithTag("Target");
        target = _target != null ? _target : player;
    }
    public void Test()
    {
        Vector3 vec3 = new Vector3(0, 5, 0);
        Vector2 vec2 = new Vector2(1, -2);
        int speed = 7;

        BulletManager.instace.SpawnBulletType1(vec3, vec2, speed);
        BulletManager.instace.SpawnBulletType2(vec3, speed);
        BulletManager.instace.SpawnBulletType3(vec3, vec2, speed, 30);
    }
}

public static class GameManagerExtensions
{ 
    public static Vector3 ToVec3(this Vector2 vec2)
    {
        return new Vector3(vec2.x, vec2.y, 0);
    }
    public static Vector2 ToVec3(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }

    public static float AngleBetween(this Vector2 vec, Vector2 standard)
    {
        Vector2 v = vec - standard;
        float angle = 2 * Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        
        return angle > 180 ? angle - 360 : angle;
    }
}
