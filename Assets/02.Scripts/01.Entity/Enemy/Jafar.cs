using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

enum JarfarAnimState { Idle, Teleport, TeleportAttack, Illusion, AdvanceSummon, WieldAttack, OnDie }
enum TeleportType {Default, Target, Up, Down}
public class Jafar : Boss
{
    readonly Vector3[] spanwPos = { new Vector3(7, 3, 0), new Vector3(7, -3, 0)};
    [SerializeField]
    JarfarFace face;
    [SerializeField]
    JafarLazer lazer;

    const float InitHp = 3500;
    public Jafar() : base(InitHp, 10, "사악한 마법사 : 자파") { }
    float thinkTime;
    Animator animator;
    Action action;
    [SerializeField]
    Vector3 fireOffset;
    int repeatCnt;
    int attackType;
    Vector3 firePos { get { return transform.position + fireOffset; } }

    protected override void Start()
    {
        Init();
        base.Start();
        animator = GetComponent<Animator>();
    }

    private void Init()
    {
        maxHp = InitHp;
        curHp = InitHp;
        attackDamage = 10;
    }

    void EndAction()
    {
        if (repeatCnt <= 0)
        {
            repeatCnt = 0;
            SetAnimation(JarfarAnimState.Idle);
            StartCoroutine(ChoseAction());
        }
    }

    protected override IEnumerator ChoseAction()
    {
        SetAnimation(JarfarAnimState.Idle);
        int chose = UnityEngine.Random.Range(0,100);
        if(chose < 17)
        {
            action = () => SetAnimation(JarfarAnimState.TeleportAttack);
            repeatCnt = 3;
            thinkTime = 2.5f;
        }
        else if(chose < 39)
        {
            action = () => SetAnimation(JarfarAnimState.Illusion);
            thinkTime = 4f;
        }
        else if(chose < 54)
        {
            action = () => SetAnimation(JarfarAnimState.AdvanceSummon);
            thinkTime = 3.5f;
        }
        else
        {
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
            thinkTime = 3f;
        }
        yield return new WaitForSeconds(thinkTime);
        animator.SetInteger("SkillRepeat", repeatCnt);
        action();
    }

    void SetAnimation(JarfarAnimState state)
    {
        switch (state)
        {
            case JarfarAnimState.Idle:
                animator.SetInteger("SkillNumber", 0);
                break;
            case JarfarAnimState.TeleportAttack:
                animator.SetInteger("SkillNumber", 1);
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
            case JarfarAnimState.Teleport:
                animator.SetInteger("SkillNumber", 5);
                break;
            case JarfarAnimState.OnDie:
                animator.SetBool("IsDie", true);
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
                if (UnityEngine.Random.Range(0, 2) == 0)
                    StartCoroutine(GameManager.instance.Repeat(() => BulletManager.instance.FireRadial(firePos, 10 * increaseAttackSpeed, 5, Vector2.left, 15,isBroken), 2, 0.1f));
                else
                    StartCoroutine(GameManager.instance.Repeat(() => BulletManager.instance.FireRadial(firePos, 10 * increaseAttackSpeed, 4, Vector2.left, 15, isBroken), 2, 0.1f));
                break;
            case 1:
                BulletManager.instance.FireTargeting(firePos, 18 * increaseAttackSpeed, isBroken);
                break;
            case 2:
                float dir = UnityEngine.Random.Range(-3, 4) * 30;
                StartCoroutine(GameManager.instance.Repeat(() => BulletManager.instance.FireTracking(firePos, 10 * increaseAttackSpeed, Vector2.left + dir.ToVec2(), isBroken), 2, 0.1f));
                break;
            case 3:
                //+환각
                dir = UnityEngine.Random.Range(-3, 4) * 30;
                StartCoroutine(GameManager.instance.Repeat(() => BulletManager.instance.FireTracking(firePos, 10 * increaseAttackSpeed, Vector2.left + dir.ToVec2(), isBroken), 2, 0.1f));
                break;
        }
        repeatCnt--;
        animator.SetInteger("SkillRepeat", repeatCnt);
    }

    void DebuffIllusion()
    {
        face.Use();
        var player = GameManager.instance.player.GetComponent<PlayerGlobal>();
        if (player != null)
            BuffManager.instance.ApplyBuff(Buffs.Illusion, 8, player);
        repeatCnt--;
        animator.SetInteger("SkillRepeat", repeatCnt);
    }

    IEnumerator TelleportAttack(float time)
    {
        animator.SetBool("IsFireLazer", true);
        float remainTime = time;
        while(remainTime > 0)
        {
            remainTime -= Time.deltaTime;
            yield return null;
        }
        repeatCnt--;
        animator.SetInteger("SkillRepeat", repeatCnt);
        animator.SetBool("IsFireLazer", false);
    }

    void SpawnMob()
    {
        int rand = UnityEngine.Random.Range(0,3);
        foreach (var pos in spanwPos)
        {
            MobManager.instance.SpawnMob(pos, rand);
        }
    }

    void Telleport(TeleportType type)
    {
        switch (type)
        {
            case TeleportType.Default:
                transform.position = initPos;
                break;
            case TeleportType.Target:
                Vector3 vec = new Vector3(transform.position.x, GameManager.instance.target.transform.position.y, 0);
                transform.position = vec;
                break;
            case TeleportType.Up:
                break;
            case TeleportType.Down:
                break;
            default:
                break;
        }
    }

    void FireLazer()
    {
        float durationTime = 1.4f;
        StartCoroutine(TelleportAttack(durationTime));
        lazer.Fire(durationTime);
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        SetAnimation(JarfarAnimState.OnDie);
    }

    


}
