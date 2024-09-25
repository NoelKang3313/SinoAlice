using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gretel : CharacterStats
{
    //Animation Related
    private string currentAnimation;

    const string GRETEL_IDLE = "Idle";
    const string GRETEL_STANDBY = "Standby";
    const string GRETEL_ATTACK = "Attack";
    const string GRETEL_MAGICSTANDBY = "MagicStandby";
    const string GRETEL_MAGICATTACK = "MagicAttack";
    const string GRETEL_MOVE = "Move";
    const string GRETEL_DYING = "Dying";
    const string GRETEL_DEAD = "Dead";
    const string GRETEL_VICTORYBEFORE = "VictoryBefore";
    const string GRETEL_VICTORY = "Victory";

    private Animator animator;

    void Awake()
    {
        _name = "Gretel";
        _level = 1;
        _exp = 0;
        _hp = 100;
        _mp = 100;
        _attack = 10;
        _defense = 10;
        _speed = 90;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GretelAction();
    }

    void GretelAction()
    {
        if (GameManager.instance.isGretelTurn)
        {
            ChangeAnimationState(GRETEL_STANDBY);
        }
        else
        {
            ChangeAnimationState(GRETEL_IDLE);
        }
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation)
            return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
