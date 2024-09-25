using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowWhite : CharacterStats
{
    //Animation Related
    private string currentAnimation;

    const string SW_IDLE = "Idle";
    const string SW_STANDBY = "Standby";
    const string SW_ATTACK = "Attack";
    const string SW_MAGICSTANDBY = "MagicStandby";
    const string SW_MAGICATTACK = "MagicAttack";
    const string SW_MOVE = "Move";
    const string SW_DYING = "Dying";
    const string SW_DEAD = "Dead";
    const string SW_VICTORYBEFORE = "VictoryBefore";
    const string SW_VICTORY = "Victory";

    void Awake()
    {
        _name = "Snow White";
        _level = 1;
        _exp = 0;
        _hp = 100;
        _mp = 100;
        _attack = 10;
        _defense = 10;
        _speed = 90;
    }

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SWAction();
    }

    void SWAction()
    {
        if (GameManager.instance.isSWTurn)
        {
            ChangeAnimationState(SW_STANDBY);
        }
        else
        {
            ChangeAnimationState(SW_IDLE);
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
