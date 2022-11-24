using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    [SerializeField]
    GameObject eBulletPrefab;
    int poolBulletCnt = 30;
    [HideInInspector]
    public List<EnemyBullet> enemyBullets = new List<EnemyBullet>();
    [HideInInspector]
    public List<PlayerBullet> playerBullets = new List<PlayerBullet>();
    ArrayList useEnemyBullets = new ArrayList();
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Start()
    {
        Init();
    }
    public void Init()
    {
        for (int i = 0; i < poolBulletCnt; i++)
        {
            CreateBullet();
        }
    }

    private EnemyBullet CreateBullet()
    {
        GameObject obj = Instantiate(eBulletPrefab, GameObject.Find("Bullets").transform);
        EnemyBullet bullet = obj.GetComponent<EnemyBullet>();
        enemyBullets.Add(bullet);
        obj.SetActive(false);
        return bullet;
    }

    public EnemyBullet SelectBullet()
    {
        foreach (EnemyBullet bullet in enemyBullets)
        {
            if (bullet.gameObject.activeSelf == false)
            {
                useEnemyBullets.Add((EnemyBullet)bullet);
                return bullet;
            }
        }
        EnemyBullet newBullet = CreateBullet();
        useEnemyBullets.Add(newBullet);
        return newBullet;
    }
    public void FireRadial(Vector3 spawnPos, int moveSpeed, int cnt, Vector2 centerDir, int elapsedDegree = 10)
    {
        if (useEnemyBullets != null)
        {
            useEnemyBullets.Clear();
        }
        for (int i = 0; i < cnt; i++)
        {
            EnemyBullet bullet = SelectBullet();
            float angle = centerDir.ToDeg() - ((cnt - 1) * elapsedDegree / 2) + (elapsedDegree * i);
            bullet.SetModeStaright(spawnPos, angle.ToVec2(), moveSpeed);
        }
    }
    public void FireRadialWithWave(Vector3 spawnPos, int moveSpeed, Vector2 centerDir, int cnt, float changeAmount, int elapsedDegree = 10)
    {
        if (useEnemyBullets != null)
        {
            useEnemyBullets.Clear();
        }
        for (int i = 0; i < cnt; i++)
        {
            EnemyBullet bullet = SelectBullet();
            float angle = centerDir.ToDeg() - ((cnt - 1) * elapsedDegree / 2) + (elapsedDegree * i);
            bullet.SetModeWave(spawnPos, angle.ToVec2(), moveSpeed, changeAmount);
        }
    }
    public void FireTracking(Vector3 spawnPos, int moveSpeed, Vector2 startDir = default(Vector2))
    {
        if (useEnemyBullets == null)
        {
            useEnemyBullets.Clear();
        }
        if (startDir == default(Vector2))
        {
            startDir = spawnPos.DistanceWithTarget();
        }
        EnemyBullet bullet = SelectBullet();
        bullet.SetModeTracking(spawnPos, moveSpeed, startDir);
    }

    public void FireTargeting(Vector3 spawnPos, int moveSpeed)
    {
        if (useEnemyBullets == null)
        {
            useEnemyBullets.Clear();
        }
        EnemyBullet bullet = SelectBullet();
        bullet.SetModeTargeting(spawnPos, moveSpeed);
    }
    public void ExecutePositioning(float distance, float time)
    {
        if (useEnemyBullets == null)
        {
            Debug.LogError("[ERROR:008]used enemy bullet isn't exist.  but call ExecutePositioning");
            return;
        }
        for (int i = 0; i < useEnemyBullets.Count; i++)
        {
            EnemyBullet bullet = useEnemyBullets[i] as EnemyBullet;
            float angle = (360 / useEnemyBullets.Count) * i;
            Vector2 v = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle)).AddDegree(90);
            bullet.SetPositioning(v * distance, time);
        }
        useEnemyBullets.Clear();
    }
}
