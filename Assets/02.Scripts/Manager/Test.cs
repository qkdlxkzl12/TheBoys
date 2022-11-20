using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    int moveSpeed;

    [SerializeField]
    GameObject obj;
    [SerializeField]
    GameObject obj2;

    [SerializeField]
    private Vector3 spawnPos;
    [SerializeField]
    float width;
    [SerializeField]
    float height;
    [SerializeField]
    int sign;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dirVec = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        if (dirVec != Vector3.zero)
        {
            dirVec.Normalize();
            transform.position += dirVec * moveSpeed * Time.deltaTime;
        }
    }

    [ContextMenu("TestBullet")]
    public void TestBullet()
    {
        obj.transform.position = spawnPos;
        StopAllCoroutines();
        StartCoroutine(TestB());
    }

    IEnumerator TestB()
    {
        float mDistance = 0;
        int mSpeed = 2;
        Vector3 center = obj.transform.position - Vector3.up * height;
        while (true)
        {
            mDistance += Time.deltaTime * mSpeed;
            obj.transform.position = center + new Vector3(mDistance * -1, Mathf.Cos(mDistance) * height);
            obj2.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y * - 1, obj.transform.position.z);
            yield return null;
        }
    }
    
}
