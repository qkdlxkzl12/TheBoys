using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Actor
{
    Action a;
    //public Enemy(int hp, int attackDamage, int moveSpeed, float attackDelay) : base(hp,attackDamage,moveSpeed, attackDelay) { }
    public Enemy(int hp, int attackDamage) : base(hp, attackDamage) { }

    public float MoveSpeed; //�̵� �ӵ�
    public float AttackSpeed; //���� �ӵ�
    public float MoveLoadingTime; //�̵� �� ���� �� ��� �ð�

    float onAttackingTime = 0.6f; //��� ��� ��ȯ �ð�

    public GameObject Me_GO;
    public Actor Me_Act;
    public Animation Anime;
    public GameObject Knives;

    //[HideInInspector]
    
    [HideInInspector]
    public bool Fire; //ȭ�� ����
    [HideInInspector]
    public int FireStack; //ȭ�� ����

    private void Start()
    {
        
    }

}
