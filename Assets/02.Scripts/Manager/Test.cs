using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    int moveSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dirVec = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0);
        dirVec.Normalize();
        transform.position += dirVec * moveSpeed * Time.deltaTime;
    }
}
