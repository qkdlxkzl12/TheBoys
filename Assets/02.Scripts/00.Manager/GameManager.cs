using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Element { Silver, Thunder, Battery, Fire, Snowball, Normal } //��, ����, ���͸�, ȭ��, ������, �Ϲ�
public enum BulletType { Straight, Target, Round, Tracking, } //����, ����, ������, 

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

    //������ ����� ���� ������
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
    public static Vector2 ToVec2(this float f)
    {
        return Quaternion.AngleAxis(f, Vector3.forward) * Vector2.left;
    }
    //(-1,0)�� �������� �ð������ ������ ��ȯ
    public static float ToDeg(this Vector2 vec2)
    {
        return Mathf.Sign(-1 * vec2.y) * Vector2.Angle(Vector2.left, vec2);
    }
    static public Vector3 DistanceWithTarget(this Vector3 vec3)
    {
        return GameManager.instance.target.transform.position - vec3;
    }
}
