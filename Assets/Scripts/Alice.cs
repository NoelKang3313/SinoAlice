using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Alice : CharacterStats
{
    private Vector3 aliceStartPosition;
    private float moveSpeed = 15.0f;

    private Animator animator;
    private UIManager uiManager;

    public GameObject ShieldPrefab;
    private GameObject shield;

    void Awake()
    {
        _name = "Alice";
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
        aliceStartPosition = transform.position;

        animator = GetComponent<Animator>();        
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void Update()
    {        
        AliceAction();
    }

    void AliceAction()
    {
        if (GameManager.instance.isAliceTurn)
        {
            Destroy(shield);

            uiManager.ActionPanel.SetActive(true);
            animator.SetBool("Standby", true);

            if (GameManager.instance.isAttackButtonActive)
            {
                animator.SetBool("MagicStandby", false);

                if (GameManager.instance.isAction)
                {
                    uiManager.ActionPanel.SetActive(false);

                    animator.SetTrigger("Move");
                    transform.position = Vector2.MoveTowards(transform.position,
                        GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber], moveSpeed * Time.deltaTime);

                    if (transform.position == new Vector3(GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].x,
                        GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].y, 0))
                    {
                        GameManager.instance.isAliceTurn = false;
                        GameManager.instance.isAction = false;

                        animator.SetBool("Standby", false);

                        ResetAnimationTrigger("Move");
                        animator.SetBool("Attack", true);
                    }
                }
            }
            else if (GameManager.instance.isGuardButtonActive && GameManager.instance.isAction)
            {
                uiManager.ActionPanel.SetActive(false);

                animator.SetBool("MagicStandby", false);
                animator.SetBool("Standby", false);

                GameManager.instance.isAliceTurn = false;
                GameManager.instance.isAction = false;

                shield = Instantiate(ShieldPrefab, transform.position - new Vector3(1.2f, -1.2f, 0), Quaternion.identity);
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                animator.SetBool("MagicStandby", true);
            }
            else if (GameManager.instance.isItemButtonActive)
            {
                Debug.Log("ITEM");
                animator.SetBool("Standby", false);
                animator.SetBool("MagicStandby", false);
                GameManager.instance.isAliceTurn = false;
            }
        }
        else
        {
            ResetButtonActive();
        }

        CheckCurrentAnimationEnd("Attack");
    }

    void CheckCurrentAnimationEnd(string animation)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (animation == "Attack")
            {
                animator.SetBool("Attack", false);
                transform.position = aliceStartPosition;
            }
        }
    }

    void ResetAnimationTrigger(string animation)
    {
        animator.ResetTrigger(animation);
    }

    void ResetButtonActive()
    {
        GameManager.instance.isAttackButtonActive = false;
        GameManager.instance.isGuardButtonActive = false;
        GameManager.instance.isSkillButtonActive = false;
        GameManager.instance.isItemButtonActive = false;
    }
}
