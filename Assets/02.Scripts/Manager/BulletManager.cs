using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instace;

    [SerializeField]
    GameObject eBulletPrefab;
    int poolBulletCnt = 10;
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
        EnemyBullet newBullet = Instantiate(eBulletPrefab).GetComponent<EnemyBullet>();
        enemyBullets.Add(newBullet);
        newBullet.gameObject.SetActive(false);
        return newBullet;
    }

    public void FireRadial(Vector3 spawnPos, int moveSpeed, Vector2 centerDir,int cnt, int elapsedDgree = 15)
    {
        for (int i = 0; i < cnt; i++)
        {
            EnemyBullet bullet = SelectBullet();
            //bullet.SetModeStaright(spawnPos,centerDir + elapsedDgree * i,moveSpeed);
        }
        //bullet.SpawnBulletType1();
    }

    public void ExecutePattern(string patterName)
    {

    }
}
