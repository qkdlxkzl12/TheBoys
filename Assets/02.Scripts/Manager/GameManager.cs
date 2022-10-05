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

    [ContextMenu("Test")]
    public void Test()
    {
        Vector3 vec3 = new Vector3(8, 0, 0);
        Vector2 vec2 = new Vector2(1, -2);
        int speed = 13;
        float test = 18f;

        //BulletManager.instance.SelectBullet().SetModeTracking(vec3, 8);
        //BulletManager.instance.FireRadial(vec3, speed, vec3.DistanceWithTarget(), 5, 10);

        BulletManager.instance.FireRadialWithWave(vec3, 5, vec3.DistanceWithTarget(), 10, 30, 15);
        BulletManager.instance.FireRadialWithWave(vec3, 5, vec3.DistanceWithTarget() + test.ToVec2().ToVec3(), 10, -30, 15);
        Invoke("Test2", 1f);
        Invoke("Test2", 1.7f);
        Invoke("Test2", 2.4f);

        //BulletManager.instance.FireRadialWithWave(vec3, speed, vec3.DistanceWithTarget(), 10, 30, 10);
        //BulletManager.instance.AddPositioning(3,1);
        //BulletManager.instance.FireRadialWithWave(vec3, speed, vec3.DistanceWithTarget() + test.ToVec2().ToVec3(), 10, -30, 10);
        //BulletManager.instance.AddPositioning(1.5f,1);
    }

    public void Test2()
    {
        Vector3 vec3 = new Vector3(8, 0, 0);
        int speed = 13;
        BulletManager.instance.FireRadial(vec3, speed, vec3.DistanceWithTarget(), 7, 8);
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

    public static float ToDeg(this Vector2 vec2)
    {
        return Mathf.Sign(-1 * vec2.y) * Vector2.Angle(Vector2.left, vec2);
    }
    static public Vector3 DistanceWithTarget(this Vector3 vec3)
    {
        return GameManager.instance.target.transform.position - vec3;
    }
}
