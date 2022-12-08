using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Actor
{
    Action a;
    //public Enemy(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp,attackDamage,moveSpeed, attackDelay) { }
    public Enemy(int hp, int attackDamage) : base(hp, attackDamage) { }

    public float MoveSpeed; //이동 속도
    public float AttackSpeed; //공격 속도
    public float MoveLoadingTime; //이동 및 공격 전 대기 시간

    float onAttackingTime = 0.6f; //대기 모션 전환 시간

    public GameObject Me_GO;
    public Actor Me_Act;
    public Animation Anime;
    public GameObject Knives;

    //[HideInInspector]
    
    [HideInInspector]
    public bool Fire; //화상 버프
    [HideInInspector]
    public int FireStack; //화상 스택

    private void Start()
    {
        
    }

}
