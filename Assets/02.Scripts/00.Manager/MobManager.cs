using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobType {None = -1, Monkey, Lazoul, Raven }
public class MobManager : MonoBehaviour
{
    public static MobManager instance;
    [SerializeField]
    GameObject[] Stage1mobs;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void SpawnMob(Vector3 spawnPos, MobType type = MobType.None)
    {
        if (type == MobType.None)
            type = MobType.None + Random.Range(0, 3);
        Instantiate(Stage1mobs[(int)type], spawnPos, Quaternion.identity);
    }
    public void SpawnMob(Vector3 spawnPos, int i = -1)
    {
        Instantiate(Stage1mobs[i], spawnPos, Quaternion.identity);
    }
}
