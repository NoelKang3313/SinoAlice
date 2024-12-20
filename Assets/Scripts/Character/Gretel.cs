using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gretel : MonoBehaviour
{
    [SerializeField]
    UIManager UIManager;

    [SerializeField]
    StageManager StageManager;

    [Header("Gretel Stats")]
    public string Name;
    public float HP;
    public float CurrentHP;
    public float MP;
    public float CurrentMP;
    public float Attack;
    public float Defense;
    public float Intell;
    public float Speed;

    private Vector3 gretelStartPosition;
    private float moveSpeed = 15.0f;

    private Animator animator;

    public GameObject ShieldPrefab;
    private GameObject shield;

    private Vector2 enemyPosition;

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

    [Header("Item")]
    public GameObject ItemHealPrefab;
    private GameObject itemHeal;
    private bool isItemHealInstantiated;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] BattleStart = new AudioClip[2];
    public AudioClip[] AttackSelect = new AudioClip[2];
    private int battleStartRandom;
    private int attackSelectRandom;

    private bool isBattleStartAudioPlaying;
    private bool isAttackSelectAudioPlaying;

    void Start()
    {
        Name = "Gretel";
        HP = 300;        
        MP = 500;        
        Attack = 10;
        Defense = 10;
        Intell = 10;
        Speed = 70;

        gretelStartPosition = transform.position;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (GameObject.Find("UIManager") != null)
        {
            UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        }

        if (GameObject.Find("StageManager") != null)
        {
            StageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        }
    }

    void Update()
    {
        GretelAction(); 
    }

    void SetEnemyPosition()
    {
        if (StageManager.EnemyInfo[0].name.StartsWith("Lightning"))
        {
            if (GameManager.instance.isAttackButtonActive)
            {
                enemyPosition = GameManager.instance.BossPosition + new Vector2(2.6f, 0.25f);
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                if (GameManager.instance.SkillButtonNumber == 0)
                {
                    enemyPosition = GameManager.instance.BossPosition + new Vector2(0.5f, 0.5f);
                }
                else
                {
                    enemyPosition = GameManager.instance.BossPosition;
                }
            }
        }
        else
        {
            if (GameManager.instance.isAttackButtonActive)
            {
                enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(2.6f, 0.25f);
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                if (GameManager.instance.SkillButtonNumber == 0)
                {
                    enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(0.5f, 0.5f);
                }
                else if(GameManager.instance.SkillButtonNumber == 3 || GameManager.instance.SkillButtonNumber == 4)
                {
                    enemyPosition = new Vector2(-5.25f, -1.0f);
                }
                else
                {
                    enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber];
                }
            }
        }
    }

    void GretelAction()
    {
        if (GameManager.instance.isBattleOver)
        {
            Victory();
            Destroy(shield);
        }
        else
        {
            if (GameManager.instance.isGretelTurn)
            {
                UIManager.CharacterMiniGauge.SetActive(true);

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
                        UIManager.CharacterMiniGauge.SetActive(false);

                        if (!isAttackSelectAudioPlaying)
                        {
                            isAttackSelectAudioPlaying = true;
                            attackSelectRandom = Random.Range(0, 2);
                            audioSource.PlayOneShot(AttackSelect[attackSelectRandom]);
                        }

                        SetEnemyPosition();

                        animator.SetTrigger("Move");                        

                        transform.position = Vector2.MoveTowards(transform.position, enemyPosition, moveSpeed * Time.deltaTime);

                        if (transform.position == new Vector3(enemyPosition.x, enemyPosition.y, 0))
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
                        animator.SetBool("Standby", false);

                        GameManager.instance.isGretelTurn = false;
                        GameManager.instance.isAction = false;
                        GameManager.instance.isGuardButtonActive = false;

                        shield = Instantiate(ShieldPrefab, transform.position, Quaternion.identity);

                        GameManager.instance.isTurn = true;
                        GameManager.instance.TurnNumber++;
                    }
                }
                else if (GameManager.instance.isSkillButtonActive)
                {                    
                    animator.SetBool("MagicStandby", true);

                    if (GameManager.instance.isAction)
                    {
                        UIManager.CharacterMiniGauge.SetActive(false);

                        if (!isAttackSelectAudioPlaying)
                        {
                            isAttackSelectAudioPlaying = true;
                            attackSelectRandom = Random.Range(0, 2);
                            audioSource.PlayOneShot(AttackSelect[attackSelectRandom]);
                        }

                        SetEnemyPosition();

                        animator.SetBool("MagicAttack", true);

                        switch (GameManager.instance.SkillButtonNumber)
                        {
                            case 0:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    blazeArrow = Instantiate(BlazeArrowPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
                                }

                                break;

                            case 1:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    gravelBomb = Instantiate(GravelBombPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
                                }

                                break;

                            case 2:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    shadowBomb = Instantiate(ShadowBombPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
                                }

                                break;

                            case 3:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    meteor = Instantiate(MeteorPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageAllEnemy(50);
                                }

                                break;

                            case 4:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    shadowExplosion = Instantiate(ShadowExplosionPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageAllEnemy(50);
                                }

                                break;

                        }
                    }
                }
                else if (GameManager.instance.isItemButtonActive)
                {                    
                    animator.SetBool("MagicStandby", true);

                    if (GameManager.instance.isAction)
                    {
                        UIManager.CharacterMiniGauge.SetActive(false);

                        animator.SetBool("MagicAttack", true);

                        if (!isItemHealInstantiated)
                        {
                            isItemHealInstantiated = true;
                            itemHeal = Instantiate(ItemHealPrefab, GameManager.instance.SelectedCharacterPosition + new Vector2(0, 0.25f), Quaternion.identity);
                        }

                        if (itemHeal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        {
                            Destroy(itemHeal);
                            isItemHealInstantiated = false;

                            GameManager.instance.isGretelTurn = false;
                            GameManager.instance.isAction = false;
                            GameManager.instance.isItemButtonActive = false;

                            animator.SetBool("MagicAttack", false);
                            animator.SetBool("MagicStandby", false);
                            animator.SetBool("Standby", false);

                            GameManager.instance.isTurn = true;
                            GameManager.instance.TurnNumber++;
                        }
                    }
                }
            }
            else
            {
                isBattleStartAudioPlaying = false;
                isAttackSelectAudioPlaying = false;
            }
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

                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                DamageEnemy(10);
            }
        }
    }

    void ResetAnimationTrigger(string animation)
    {
        animator.ResetTrigger(animation);
    }

    void DamageEnemy(int damage)
    {
        if (StageManager.EnemyInfo[GameManager.instance.EnemyPositionNumber].name.StartsWith("Rat"))
        {
            StageManager.EnemyInfo[GameManager.instance.EnemyPositionNumber].GetComponent<Rat>().CurrentHP -= damage;
        }
        else if (StageManager.EnemyInfo[GameManager.instance.EnemyPositionNumber].name.StartsWith("Wolf"))
        {
            StageManager.EnemyInfo[GameManager.instance.EnemyPositionNumber].GetComponent<Wolf>().CurrentHP -= damage;
        }
        else if (StageManager.EnemyInfo[0].name.StartsWith("Lightning"))
        {
            StageManager.EnemyInfo[0].GetComponent<Lightning>().CurrentHP -= damage;
        }
    }

    void DamageAllEnemy(int damage)
    {
        for (int i = 0; i < StageManager.EnemyInfo.Count; i++)
        {
            if (StageManager.EnemyInfo[i].name.StartsWith("Rat"))
            {
                StageManager.EnemyInfo[i].GetComponent<Rat>().CurrentHP -= damage;
            }
            else if (StageManager.EnemyInfo[i].name.StartsWith("Wolf"))
            {
                StageManager.EnemyInfo[i].GetComponent<Wolf>().CurrentHP -= damage;
            }
            else if (StageManager.EnemyInfo[i].name.StartsWith("Lightning"))
            {
                StageManager.EnemyInfo[i].GetComponent<Lightning>().CurrentHP -= damage;
            }
        }
    }

    void Victory()
    {
        if (GameManager.instance.isBattleOver)
        {
            animator.SetBool("Victory", true);            
        }
    }
}
