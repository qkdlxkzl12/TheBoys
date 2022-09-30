using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instace;

    [SerializeField]
    GameObject eBulletPrefab;
    int poolEbulletCnt = 100;
    [HideInInspector]
    public static List<GameObject> l_enemyBullets = new List<GameObject>();
    [HideInInspector]
    public static List<GameObject> l_playerBullets = new List<GameObject>();
    private void Awake()
    {
        if(instace == null)
            instace = this;
        else
            Destroy(this);
    }

    public void Start()
    {
        Init();
    }
    void Init()
    {
        for (int i = 0; i < poolEbulletCnt; i++)
        {
            var eBullet = Instantiate(eBulletPrefab);
            l_enemyBullets.Add(eBullet);
            eBullet.SetActive(false);
        }
    }
    public void SpawnBulletType1(Vector3 spawnPos, Vector2 moveDir, int moveSpeed)
    {
        Bullet bullet = SelectBullet().GetComponent<Bullet>();
        bullet.InitState(spawnPos, BulletType.STRAIGHT, moveDir, moveSpeed);
    }
    public void SpawnBulletType2(Vector3 spawnPos, int moveSpeed)
    {
        Bullet bullet = SelectBullet().GetComponent<Bullet>();
        Vector3 targetDir = GameManager.instance.target.transform.position - spawnPos;
        targetDir.Normalize();
        bullet.InitState(spawnPos, BulletType.STRAIGHT, targetDir, moveSpeed);
    }

    public void SpawnBulletType3(Vector3 spawnPos, Vector2 moveDir,int moveSpeed, float chageAmount)
    {
        Bullet bullet = SelectBullet().GetComponent<Bullet>();
        bullet.InitState(spawnPos, BulletType.ROUND, moveDir, moveSpeed, chageAmount);
    }

    GameObject SelectBullet()
    {
        foreach(GameObject bullet in l_enemyBullets)
        {
            if (!bullet.activeSelf)
                return bullet;
        }
        var eBullet = Instantiate(eBulletPrefab);
        l_enemyBullets.Add(eBullet);
        eBullet.SetActive(false);
        return eBullet;
    }
}
