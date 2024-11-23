using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    Animator animator;

    [Header("Rat Stats")]
    public string Name;
    public float HP;
    public float MP;
    public float CurrentHP;
    public float CurrentMP;
    public float Attack;
    public float Defense;    
    public float Speed;

    public bool isCurrentEnemyTurn;

    void Awake()
    {
        Name = "Rat";
        HP = 100;
        MP = 100;
        CurrentHP = HP;
        CurrentMP = MP;
        Attack = 10;
        Defense = 10;
        Speed = 50;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RatTurn();
        ResetAnimation();
    }

    void RatTurn()
    {
        if(isCurrentEnemyTurn)
        {
            animator.SetBool("isAttack", true);
        }
    }

    void ResetAnimation()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            animator.SetBool("isAttack", false);

            isCurrentEnemyTurn = false;
            GameManager.instance.isEnemyTurn = false;
            GameManager.instance.isTurn = true;
            GameManager.instance.TurnNumber++;
        }
    }
}
