using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gretel : CharacterStats
{
    private Vector3 gretelStartPosition;
    private float moveSpeed = 15.0f;

    private Animator animator;
    private UIManager uiManager;

    public GameObject ShieldPrefab;
    private GameObject shield;

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
        gretelStartPosition = transform.position;

        animator = GetComponent<Animator>();        
    }

    void Update()
    {
        GretelAction();

        if (GameObject.Find("UIManager") == null)
            return;
        else
            uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void GretelAction()
    {
        if (GameManager.instance.isGretelTurn)
        {
            Destroy(shield);

            uiManager.ActionButtons.GetComponent<Animator>().SetBool("isActive", true);
            animator.SetBool("Standby", true);

            if (GameManager.instance.isAttackButtonActive)
            {
                animator.SetBool("MagicStandby", false);

                if (GameManager.instance.isAction)
                {
                    uiManager.ActionButtons.GetComponent<Animator>().SetBool("isActive", false);

                    animator.SetTrigger("Move");
                    transform.position = Vector2.MoveTowards(transform.position,
                        GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(3.8f, -1.0f), moveSpeed * Time.deltaTime);

                    if (transform.position == new Vector3((GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].x + 3.8f),
                        (GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].y - 1.0f), 0))
                    {
                        GameManager.instance.isGretelTurn = false;
                        GameManager.instance.isAction = false;

                        animator.SetBool("Standby", false);

                        ResetAnimationTrigger("Move");
                        animator.SetBool("Attack", true);
                    }
                }
            }
            else if (GameManager.instance.isGuardButtonActive)
            {
                animator.SetBool("MagicStandby", false);
                animator.SetBool("Standby", true);

                if (GameManager.instance.isAction)
                {
                    animator.SetBool("Standby", false);

                    uiManager.ActionButtons.GetComponent<Animator>().SetBool("isActive", false);

                    GameManager.instance.isGretelTurn = false;
                    GameManager.instance.isAction = false;

                    shield = Instantiate(ShieldPrefab, transform.position - new Vector3(1.2f, -1.2f, 0), Quaternion.identity);
                }
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                Debug.Log("SKILL");
            }
            else if (GameManager.instance.isItemButtonActive)
            {
                Debug.Log("ITEM");

                //uiManager.ActionButtons.GetComponent<Animator>().SetBool("isActive", false);

                //animator.SetBool("Standby", false);
                //animator.SetBool("MagicStandby", false);
                //GameManager.instance.isAliceTurn = false;
            }
        }
        else
        {
            //ResetButtonActive();
        }

        CheckCurrentAnimationEnd("Attack");
    }

    void CheckCurrentAnimationEnd(string animation)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (animation == "Attack")
            {
                animator.SetBool("Attack", false);
                transform.position = gretelStartPosition;
            }
        }
    }

    void ResetAnimationTrigger(string animation)
    {
        animator.ResetTrigger(animation);
    }

    //void ResetButtonActive()
    //{
    //    GameManager.instance.isAttackButtonActive = false;
    //    GameManager.instance.isGuardButtonActive = false;
    //    GameManager.instance.isSkillButtonActive = false;
    //    GameManager.instance.isItemButtonActive = false;
    //}

}
