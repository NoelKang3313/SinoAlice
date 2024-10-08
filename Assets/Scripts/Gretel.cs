using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gretel : CharacterStats
{
    private Vector3 gretelStartPosition;
    private float moveSpeed = 15.0f;

    private Animator animator;    

    public GameObject ShieldPrefab;
    private GameObject shield;

    [Header("Skills")]
    public GameObject BlazeArrowPrefab;
    private GameObject blazeArrow;
    public GameObject GravelBombPrefab;
    private GameObject gravelBomb;
    public GameObject ShadowBombPrefab;
    private GameObject shadowBomb;
    public GameObject MeteorPrefab;
    private GameObject meteor;
    public GameObject ShadowExplosionPrefab;
    private GameObject shadowExplosion;

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
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GretelAction();        
    }

    void GretelAction()
    {
        if (GameManager.instance.isGretelTurn)
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
                        GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(3.8f, -1.0f), moveSpeed * Time.deltaTime);

                    if (transform.position == new Vector3((GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].x + 3.8f),
                        (GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber].y - 1.0f), 0))
                    {
                        GameManager.instance.isGretelTurn = false;
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

                    GameManager.instance.isGretelTurn = false;
                    GameManager.instance.isAction = false;
                    GameManager.instance.isGuardButtonActive = false;

                    shield = Instantiate(ShieldPrefab, transform.position - new Vector3(1.2f, -1.2f, 0), Quaternion.identity);
                }
            }
            else if (GameManager.instance.isSkillButtonActive)
            {                
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

                    switch(GameManager.instance.SkillButtonNumber)
                    {
                        case 0:
                            if(!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                blazeArrow = Instantiate(BlazeArrowPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(0.5f, 0.5f), Quaternion.identity);
                            }

                            if (blazeArrow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(blazeArrow);
                                isSkillInstantiated = false;

                                GameManager.instance.isGretelTurn = false;
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
                                gravelBomb = Instantiate(GravelBombPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber],
                                    Quaternion.identity);
                            }

                            if (gravelBomb.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(gravelBomb);
                                isSkillInstantiated = false;

                                GameManager.instance.isGretelTurn = false;
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
                                shadowBomb = Instantiate(ShadowBombPrefab,
                                    GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber],
                                    Quaternion.identity);
                            }

                            if (shadowBomb.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(shadowBomb);
                                isSkillInstantiated = false;

                                GameManager.instance.isGretelTurn = false;
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
                                meteor = Instantiate(MeteorPrefab, new Vector3(-5.25f, -1.0f, 0), Quaternion.identity);
                            }

                            if (meteor.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(meteor);
                                isSkillInstantiated = false;

                                GameManager.instance.isGretelTurn = false;
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
                                shadowExplosion = Instantiate(ShadowExplosionPrefab, new Vector3(-5.25f, -1.0f, 0), Quaternion.identity);
                            }

                            if (shadowExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(shadowExplosion);
                                isSkillInstantiated = false;

                                GameManager.instance.isGretelTurn = false;
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
                transform.position = gretelStartPosition;
            }
        }
    }

    void ResetAnimationTrigger(string animation)
    {
        animator.ResetTrigger(animation);
    }
}
