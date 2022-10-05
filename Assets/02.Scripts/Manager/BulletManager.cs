using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            var Bullet = Instantiate(eBulletPrefab);
            enemyBullets.Add(Bullet.GetComponent<EnemyBullet>());
            Bullet.SetActive(false);
        }
    }
    //

    public EnemyBullet SelectBullet()
    {
        foreach (EnemyBullet bullet in enemyBullets)
        {
            if (bullet.gameObject.activeSelf == false)
            {
                return bullet;
            }
        }
        EnemyBullet newBullet = Instantiate(eBulletPrefab).GetComponent<EnemyBullet>();
        useEnemyBullets.Add((EnemyBullet)newBullet);
        enemyBullets.Add(newBullet);
        newBullet.gameObject.SetActive(false);
        return newBullet;
    }

    public void FireRadial(Vector3 spawnPos, int moveSpeed, Vector2 centerDir, int cnt, int elapsedDegree = 10)
    {
        if (useEnemyBullets != null)
            useEnemyBullets.Clear();
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
            useEnemyBullets.Clear();
        for (int i = 0; i < cnt; i++)
        {
            EnemyBullet bullet = SelectBullet();
            float angle = centerDir.ToDeg() - ((cnt - 1) * elapsedDegree / 2) + (elapsedDegree * i);
            bullet.SetModeWave(spawnPos, angle.ToVec2(), moveSpeed, changeAmount);
        }
    }

    public void AddPositioning(float distance, float second)
    {
        if(useEnemyBullets == null)
        {
            Debug.LogError("[ERROR:008]used enemy bullet isn't exist.  but call AddPositioning");
            return;
        }    
        for (int i = 0; i < useEnemyBullets.Count; i++)
        {
            EnemyBullet bullet = useEnemyBullets[i] as EnemyBullet;
            float angle = (360 / useEnemyBullets.Count) * i;
            Vector2 v = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle)).AddDegree(90);
            Debug.Log(v.ToDeg());
            bullet.SetPositioning(v * distance, second);
        }
        useEnemyBullets.Clear();
    }
}
