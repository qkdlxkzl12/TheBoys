using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    [SerializeField]
    private Sprite[] bulletSprites;
    [SerializeField]
    private GameObject eBulletPrefab;
    [SerializeField]
    private GameObject pBulletPrefab;
    const int poolEBulletCnt = 30;
    const int poolPBulletCnt = 20;
    [HideInInspector]
    public List<EnemyBullet> enemyBullets = new List<EnemyBullet>();
    [HideInInspector]
    public List<PlayerBullet> playerBullets = new List<PlayerBullet>();
    public List<Element> playerMagazine = new List<Element>(); //실제로 플레이어의 탄창
    int playerCurIndex;
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
        for (int i = 0; i < poolEBulletCnt; i++)
        {
            CreateBullet();
        }
        for (int i = 0; i < poolPBulletCnt * 5; i++)
        {
            GameObject obj = Instantiate(pBulletPrefab, GameObject.Find("Bullets").transform);
            PlayerBullet bullet = obj.GetComponent<PlayerBullet>();
            obj.GetComponent<SpriteRenderer>().sprite = bulletSprites[i / poolPBulletCnt];
            playerBullets.Add(bullet);
            switch (i/ poolPBulletCnt)
            {
                case 0:
                    bullet.Init(Element.Normal);
                    break;
                case 1:
                    bullet.Init(Element.Battery);
                    break;
                case 2:
                    bullet.Init(Element.Silver);
                    break;
                case 3:
                    bullet.Init(Element.Snowball);
                    break;
                case 4:
                    bullet.Init(Element.Fire);
                    break;
                default:
                    break;
            }
            obj.name = $"PlayerBullet{bullet.element}";
            obj.SetActive(false);

        }
        PlayerBulletAdd(Element.Normal,cnt:5);
        PlayerBulletAdd(Element.Snowball, cnt: 3);
    }

    #region 적총알 관련
    private EnemyBullet CreateBullet()
    {
        GameObject obj = Instantiate(eBulletPrefab, GameObject.Find("Bullets").transform);
        EnemyBullet bullet = obj.GetComponent<EnemyBullet>();
        enemyBullets.Add(bullet);
        obj.SetActive(false);
        return bullet;
    }

    private EnemyBullet SelectBullet()
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
    #endregion

    public void PlayerBulletFire(Vector3 pos)
    {
        Debug.Log(playerMagazine[playerCurIndex]);
        for (int i = 0; i < playerBullets.Count; i++)
        {
            if(playerBullets[i].element == playerMagazine[playerCurIndex] && playerBullets[i].gameObject.activeSelf == false)
            {
                var fireBullet = playerBullets[i];
                fireBullet.gameObject.SetActive(true);
                fireBullet.transform.position = pos;
                fireBullet.Init(fireBullet.element);
                playerCurIndex = (playerCurIndex + 1) % playerMagazine.Count;
                break;
            }
        }
    }

    public void PlayerBulletAdd(Element element = Element.Normal, int cnt = 1)
    {
        for (int i = 0; i < cnt; i++)
        {
            playerMagazine.Add(element);
            var newBullet = Instantiate(pBulletPrefab).GetComponent<PlayerBullet>();
            playerBullets.Add(newBullet);
            newBullet.Init(element);
            newBullet.gameObject.SetActive(false);
            Debug.Log($"{element}타입의 총알 생성");
        }
    }

    public void Test()
    {
        PlayerBulletAdd(Element.Snowball);
    }
}
