using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowWhite : CharacterStats
{
    private Vector3 swStartPosition;
    private float moveSpeed = 15.0f;

    private Animator animator;    

    public GameObject ShieldPrefab;
    private GameObject shield;

    [Header("Skills")]
    public GameObject DoubleSlashPrefab;
    private GameObject doubleSlash;
    public GameObject TripleSlashPrefab;
    private GameObject tripleSlash;
    public GameObject AerialSlashPrefab;
    private GameObject aerialSlash;
    public GameObject ThunderThornPrefab;
    private GameObject thunderThorn;
    public GameObject StormBladePrefab;
    private GameObject stormBlade;

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
        _name = "Snow White";
        _hp = 100;
        _mp = 100;
        _attack = 10;
        _defense = 10;
        _intell = 10;
        _speed = 90;
    }

    void Start()
    {
        swStartPosition = transform.position;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        SWAction();
    }

    void SWAction()
    {
        if (GameManager.instance.isSWTurn)
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
                    if (!isAttackSelectAudioPlaying)
                    {
                        isAttackSelectAudioPlaying = true;
                        attackSelectRandom = Random.Range(0, 2);
                        audioSource.PlayOneShot(AttackSelect[attackSelectRandom]);
                    }                    

                    animator.SetTrigger("Move");
                    transform.position = Vector2.MoveTowards(transform.position,
                        GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(4.0f, -0.8f), moveSpeed * Time.deltaTime);

                    if (transform.position == new Vector3((GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].x + 4.0f),
                        (GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].y - 0.8f), 0))
                    {
                        GameManager.instance.isSWTurn = false;
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

                    GameManager.instance.isSWTurn = false;
                    GameManager.instance.isAction = false;
                    GameManager.instance.isGuardButtonActive = false;

                    shield = Instantiate(ShieldPrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
                }
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                animator.SetBool("Standby", false);
                animator.SetBool("MagicStandby", true);

                if (GameManager.instance.isAction)
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
                                doubleSlash = Instantiate(DoubleSlashPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber], Quaternion.identity);
                            }

                            if (doubleSlash.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(doubleSlash);
                                isSkillInstantiated = false;

                                GameManager.instance.isSWTurn = false;
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
                                tripleSlash = Instantiate(TripleSlashPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber], Quaternion.identity);
                            }

                            if (tripleSlash.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(tripleSlash);
                                isSkillInstantiated = false;

                                GameManager.instance.isSWTurn = false;
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
                                aerialSlash = Instantiate(AerialSlashPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber], Quaternion.identity);
                            }

                            if (aerialSlash.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(aerialSlash);
                                isSkillInstantiated = false;

                                GameManager.instance.isSWTurn = false;
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
                                thunderThorn = Instantiate(ThunderThornPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(0, 1.7f), Quaternion.identity);
                            }

                            if (thunderThorn.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(thunderThorn);
                                isSkillInstantiated = false;

                                GameManager.instance.isSWTurn = false;
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
                                stormBlade = Instantiate(StormBladePrefab);
                            }

                            if (stormBlade.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(stormBlade);
                                isSkillInstantiated = false;

                                GameManager.instance.isSWTurn = false;
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (animation == "Attack")
            {
                animator.SetBool("Attack", false);
                transform.position = swStartPosition;
            }
        }
    }

    void ResetAnimationTrigger(string animation)
    {
        animator.ResetTrigger(animation);
    }
}
