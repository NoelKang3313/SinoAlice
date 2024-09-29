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

    [Header("Skills")]
    public GameObject GlacialArrowPrefab;
    private GameObject glacialArrow;
    public GameObject AquaBombPrefab;
    private GameObject aquaBomb;
    public GameObject BlizzardBombPrefab;
    private GameObject blizzardBomb;
    public GameObject TidalWavePrefab;
    private GameObject tidalWave;
    public GameObject HealingWindPrefab;
    private GameObject healingWind;

    private bool isSkillInstantiated;

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
                        GameManager.instance.AttackEnemyPositions[GameManager.instance.EnemyPositionNumber], moveSpeed * Time.deltaTime);

                    if (transform.position == new Vector3(GameManager.instance.AttackEnemyPositions[GameManager.instance.EnemyPositionNumber].x,
                        GameManager.instance.AttackEnemyPositions[GameManager.instance.EnemyPositionNumber].y, 0))
                    {
                        GameManager.instance.isAliceTurn = false;
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

                    uiManager.ActionPanel.SetActive(false);

                    GameManager.instance.isAliceTurn = false;
                    GameManager.instance.isAction = false;

                    shield = Instantiate(ShieldPrefab, transform.position - new Vector3(1.2f, -1.2f, 0), Quaternion.identity);
                }
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                animator.SetBool("Standby", false);
                animator.SetBool("MagicStandby", true);                

                if(GameManager.instance.isAction)
                {                    
                    animator.SetBool("MagicAttack", true);

                    uiManager.ActionPanel.SetActive(false);

                    switch(GameManager.instance.SkillButtonNumber)
                    {
                        case 0:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                glacialArrow = Instantiate(GlacialArrowPrefab,
                                    GameManager.instance.SkillEnemyPositions[GameManager.instance.EnemyPositionNumber], Quaternion.identity);
                            }

                            if (glacialArrow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(glacialArrow);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                            }

                            break;
                        case 1:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                aquaBomb = Instantiate(AquaBombPrefab,
                                    GameManager.instance.SkillEnemyPositions[GameManager.instance.EnemyPositionNumber], Quaternion.identity);
                            }

                            if (aquaBomb.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(aquaBomb);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                            }

                            break;
                        case 2:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                blizzardBomb = Instantiate(BlizzardBombPrefab,
                                    GameManager.instance.SkillEnemyPositions[GameManager.instance.EnemyPositionNumber], Quaternion.identity);
                            }

                            if (blizzardBomb.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(blizzardBomb);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                            }

                            break;
                        case 3:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                tidalWave = Instantiate(TidalWavePrefab);
                            }

                            if (tidalWave.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(tidalWave);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                            }

                            break;
                        case 4:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                healingWind = Instantiate(HealingWindPrefab,
                                    GameManager.instance.CharacterPositions[GameManager.instance.AlicePositionNumber], Quaternion.identity);
                            }

                            if (healingWind.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(healingWind);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                            }

                            break;
                    }
                }
            }
            else if (GameManager.instance.isItemButtonActive)
            {
                Debug.Log("ITEM");

                uiManager.ActionPanel.SetActive(false);

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
