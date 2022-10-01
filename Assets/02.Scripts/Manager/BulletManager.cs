using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instace;

    [SerializeField]
    GameObject eBulletPrefab_;
    int poolEbulletCnt_ = 100;
    [HideInInspector]
    public List<EnemyBullet> enemyBullets = new List<EnemyBullet>();
    [HideInInspector]
    public List<PlayerBullet> playerBullets = new List<PlayerBullet>();
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
    public void Init()
    {
        for (int i = 0; i < poolEbulletCnt_; i++)
        {
            var Bullet = Instantiate(eBulletPrefab_);
            enemyBullets.Add(Bullet.GetComponent<EnemyBullet>());
            Bullet.SetActive(false);
        }
    }

    public void SpawnBulletType1(Vector3 spawnPos, Vector2 moveDir, int moveSpeed)
    {
        Bullet bullet = SelectBullet().GetComponent<Bullet>();
        bullet.InitState(spawnPos, BulletType.Straight, moveDir, moveSpeed);
    }
    public void SpawnBulletType2(Vector3 spawnPos, int moveSpeed)
    {
        Bullet bullet = SelectBullet().GetComponent<Bullet>();
        Vector3 targetDir = GameManager.instance.target.transform.position - spawnPos;
        targetDir.Normalize();
        bullet.InitState(spawnPos, BulletType.Straight, targetDir, moveSpeed);
    }

    public void SpawnBulletType3(Vector3 spawnPos, Vector2 moveDir,int moveSpeed, float chageAmount)
    {
        Bullet bullet = SelectBullet().GetComponent<Bullet>();
        bullet.InitState(spawnPos, BulletType.Round, moveDir, moveSpeed, chageAmount);
    }

    public EnemyBullet SelectBullet()
    {
        foreach(EnemyBullet bullet in enemyBullets)
        {
            if (!bullet.gameObject.activeSelf)
                return bullet;
        }
        var Bullet = Instantiate(eBulletPrefab_).GetComponent<EnemyBullet>();
        enemyBullets.Add(Bullet);
        Bullet.gameObject.SetActive(false);
        return Bullet;
    }
}
