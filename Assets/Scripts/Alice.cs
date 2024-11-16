using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Alice : MonoBehaviour
{
    [Header("Alice Stats")]
    public string Name;
    public float HP;
    public float CurrentHP;
    public float MP;
    public float CurrentMP;
    public float Attack;
    public float Defense;
    public float Intell;
    public float Speed;

    private Vector3 aliceStartPosition;
    private float moveSpeed = 15.0f;

    private Animator animator;    

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

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] BattleStart = new AudioClip[2];
    public AudioClip[] AttackSelect = new AudioClip[2];
    private int battleStartRandom;
    private int attackSelectRandom;

    private bool isBattleStartAudioPlaying;
    private bool isAttackSelectAudioPlaying;

    void Awake()
    {
        Name = "Alice";
        HP = 100;
        CurrentHP = HP;
        MP = 100;
        CurrentMP = MP;
        Attack = 10;
        Defense = 10;
        Intell = 10;
        Speed = 90;
    }

    void Start()
    {        
        aliceStartPosition = transform.position;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {     
        AliceAction();
    }

    void AliceAction()
    {
        if (GameManager.instance.isAliceTurn)
        {
            if (!isBattleStartAudioPlaying)
            {
                isBattleStartAudioPlaying = true;
                battleStartRandom = Random.Range(0, 2);
                audioSource.PlayOneShot(BattleStart[battleStartRandom]);
            }

            Destroy(shield);
            
            animator.SetBool("Standby", true);

            if (GameManager.instance.isAttackButtonActive)
            {
                animator.SetBool("MagicStandby", false);

                if (GameManager.instance.isAction)
                {
                    if(!isAttackSelectAudioPlaying)
                    {
                        isAttackSelectAudioPlaying = true;
                        attackSelectRandom = Random.Range(0, 2);
                        audioSource.PlayOneShot(AttackSelect[attackSelectRandom]);
                    }                    

                    animator.SetTrigger("Move");
                    transform.position = Vector2.MoveTowards(transform.position,
                        GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(1.8f, 0.5f), moveSpeed * Time.deltaTime);

                    if (transform.position == new Vector3((GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].x + 1.8f),
                        (GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].y + 0.5f), 0))
                    {
                        GameManager.instance.isAliceTurn = false;
                        GameManager.instance.isAction = false;
                        GameManager.instance.isAttackButtonActive = false;

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
                    if (!isAttackSelectAudioPlaying)
                    {
                        isAttackSelectAudioPlaying = true;
                        attackSelectRandom = Random.Range(0, 2);
                        audioSource.PlayOneShot(AttackSelect[attackSelectRandom]);
                    }

                    animator.SetBool("Standby", false);                    

                    GameManager.instance.isAliceTurn = false;
                    GameManager.instance.isAction = false;
                    GameManager.instance.isGuardButtonActive = false;

                    shield = Instantiate(ShieldPrefab, transform.position, Quaternion.identity);
                }
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                animator.SetBool("Standby", false);
                animator.SetBool("MagicStandby", true);                

                if(GameManager.instance.isAction)
                {
                    if (!isAttackSelectAudioPlaying)
                    {
                        isAttackSelectAudioPlaying = true;
                        attackSelectRandom = Random.Range(0, 2);
                        audioSource.PlayOneShot(AttackSelect[attackSelectRandom]);
                    }

                    animator.SetBool("MagicAttack", true);                    

                    switch (GameManager.instance.SkillButtonNumber)
                    {
                        case 0:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                glacialArrow = Instantiate(GlacialArrowPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(1,1), Quaternion.identity);
                            }

                            if (glacialArrow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(glacialArrow);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;
                                GameManager.instance.isSkillButtonActive = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                                animator.SetBool("Standby", false);
                            }

                            break;

                        case 1:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                aquaBomb = Instantiate(AquaBombPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber], Quaternion.identity);
                            }

                            if (aquaBomb.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(aquaBomb);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;
                                GameManager.instance.isSkillButtonActive = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                                animator.SetBool("Standby", false);
                            }

                            break;

                        case 2:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                blizzardBomb = Instantiate(BlizzardBombPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber], Quaternion.identity);
                            }

                            if (blizzardBomb.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(blizzardBomb);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;
                                GameManager.instance.isSkillButtonActive = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                                animator.SetBool("Standby", false);
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
                                GameManager.instance.isSkillButtonActive = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                                animator.SetBool("Standby", false);
                            }

                            break;

                        case 4:
                            if (!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                healingWind = Instantiate(HealingWindPrefab,
                                    GameManager.instance.SelectedCharacterPosition + new Vector2(0, 0.25f), Quaternion.identity);
                            }

                            if (healingWind.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(healingWind);
                                isSkillInstantiated = false;

                                GameManager.instance.isAliceTurn = false;
                                GameManager.instance.isAction = false;
                                GameManager.instance.isSkillButtonActive = false;   

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);
                                animator.SetBool("Standby", false);
                            }

                            break;
                    }
                }
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
            isBattleStartAudioPlaying = false;
            isAttackSelectAudioPlaying = false;
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
}
