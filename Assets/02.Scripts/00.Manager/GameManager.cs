using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Element { Silver, Thunder, Battery, Fire, Snowball, Normal } //은, 번개, 배터리, 화염, 눈덩이, 일반
public enum BulletType { Straight, Target, Round, Tracking, } //일자, 변형, 반유도, 

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance;
    [HideInInspector]
    public GameObject target { get; set; }
    [HideInInspector]
    public GameObject player { get; set; }
    public int testIndex;

    int y;
    Vector3 vec3 = new Vector3(8,0,0);
    Vector2 vec2 = new Vector2(-2, 0);
    int speed = 13;
    float test = 18f;
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
        GameObject target = GameObject.FindGameObjectWithTag("Target");
        this.target = target != null ? target : player;
    }

    public IEnumerator Repeat(Action func, int repeat, float delay)
    {
        int curRepeat = 0;
        while (curRepeat < repeat)
        {
            func();
            curRepeat++;
            yield return new WaitForSeconds(delay);
        }
    }

    public static Vector3 RandomDir()
    {
        float degree = UnityEngine.Random.Range(0, 360);
        return degree.ToVec2().normalized;
    }
}

public static class GameManagerExtensions
{
    //벡터2를 벡터2로 반환
    public static Vector3 ToVec3(this Vector2 vec2, float z = 0)
    {
        return new Vector3(vec2.x, vec2.y, z);
    }
    //벡터3을 벡터2로 반환
    public static Vector2 ToVec2(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }
    //방향벡터에 각도 만큼 더한 방향 벡터를 반환
    public static Vector2 AddDegree(this Vector2 vec2, float degree)
    {
        return Quaternion.AngleAxis(degree, Vector3.forward) * vec2;
    }
    public static Vector2 ToVec2(this float f)
    {
        return Quaternion.AngleAxis(f, Vector3.forward) * Vector2.left;
    }
    //(-1,0)을 기준으로 시계방향의 각도를 반환
    public static float ToDeg(this Vector2 vec2)
    {
        return Mathf.Sign(-1 * vec2.y) * Vector2.Angle(Vector2.left, vec2);
    }
    static public Vector3 DistanceWithTarget(this Vector3 vec3)
    {
        return GameManager.instance.target.transform.position - vec3;
    }
}
