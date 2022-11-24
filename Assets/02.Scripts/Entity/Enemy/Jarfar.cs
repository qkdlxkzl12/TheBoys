using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


enum JarfarAnimState {Idle, Teleport, TeleportAttack, Illusion, AdvanceSummon, WieldAttack }
public class Jarfar : Enemy
{
    Jarfar() : base(0,0,0,0) { }
    float thinkTime;
    Animator animator;
    Action action;
    [SerializeField]
    Vector3 fireOffset;
    int repeatCnt;
    int attackType;
    Vector3 firePos { get { return transform.position + fireOffset; } }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ChoseAction());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void EndAction()
    {
        if(repeatCnt <= 0)
        {
            repeatCnt = 0;
            StartCoroutine(ChoseAction());
        }
    }

    IEnumerator ChoseAction()
    {
        SetAnimation(JarfarAnimState.Idle);
        int chose = 3; //UnityEngine.Random.Range(1,100);
        switch(chose)
        {
            case 0: action = () => SetAnimation(JarfarAnimState.TeleportAttack);
                thinkTime = 2f;
                break;
            case 1: action = () => SetAnimation(JarfarAnimState.Illusion);
                thinkTime = 2f;
                break;
            case 2 : action = () => SetAnimation(JarfarAnimState.AdvanceSummon);
                thinkTime = 2f;
                break;
            case 3:
                action = () => SetAnimation(JarfarAnimState.WieldAttack);
                attackType = UnityEngine.Random.Range(0, 4);
                switch (attackType)
                {
                    case 0:
                        repeatCnt = 5;
                        break;
                    case 1: 
                        repeatCnt = 3;
                        break;
                    case 2:
                        repeatCnt = 3;
                        break;
                    case 3:
                        repeatCnt = 3;
                        break;
                }
                thinkTime = 2f;
                action += () =>
                {
                    animator.SetInteger("SkillRepeat", repeatCnt);
                };
                break;
        }
        while(thinkTime > 0)
        {
            thinkTime -= Time.deltaTime;
            yield return null;
        }
        action();
    }

    void SetAnimation(JarfarAnimState state )
    {
        switch (state)
        {
            case JarfarAnimState.Idle:
                animator.SetInteger("SkillNumber", 0);
                break;
            case JarfarAnimState.Teleport:
                animator.SetInteger("SkillNumber", 1);
                break;
            case JarfarAnimState.TeleportAttack:
                break;
            case JarfarAnimState.Illusion:
                animator.SetInteger("SkillNumber", 2);
                break;
            case JarfarAnimState.AdvanceSummon:
                animator.SetInteger("SkillNumber", 3);
                break;
            case JarfarAnimState.WieldAttack:
                animator.SetInteger("SkillNumber", 4);
                break;
            default:
                break;
        }
    }

    void WieldAttack()
    {
        switch (attackType)
        { 
            case 0:
                if(UnityEngine.Random.Range(0,2) == 0)
                    StartCoroutine(GameManager.instance.Repeat(() => BulletManager.instance.FireRadial(firePos, 10, 5, Vector2.left, 15),2,0.1f));
                else
                    StartCoroutine(GameManager.instance.Repeat(() => BulletManager.instance.FireRadial(firePos, 10, 4, Vector2.left, 15), 2, 0.1f));
                break;
            case 1:BulletManager.instance.FireTargeting(firePos, 14);
                break;
            case 2:
                float dir = UnityEngine.Random.Range(-3, 4) * 30;
                StartCoroutine(GameManager.instance.Repeat(() => BulletManager.instance.FireTracking(firePos, 10, Vector2.left + dir.ToVec2()), 2, 0.1f));
                break;
            case 3:
                //+È¯°¢
                dir = UnityEngine.Random.Range(-3, 4) * 30;
                StartCoroutine(GameManager.instance.Repeat(() => BulletManager.instance.FireTracking(firePos, 10, Vector2.left + dir.ToVec2()), 2, 0.1f));
                break;
        }
        repeatCnt--;
        animator.SetInteger("SkillRepeat", repeatCnt);
    }
}
