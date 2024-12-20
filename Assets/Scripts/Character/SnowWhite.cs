using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowWhite : MonoBehaviour
{
    [SerializeField]
    UIManager UIManager;

    [SerializeField]
    StageManager StageManager;

    [Header("Snow White Stats")]
    public string Name;
    public float HP;
    public float CurrentHP;
    public float MP;
    public float CurrentMP;
    public float Attack;
    public float Defense;
    public float Intell;
    public float Speed;

    private Vector3 swStartPosition;
    private float moveSpeed = 15.0f;

    private Animator animator;

    public GameObject AttackEffectPrefab;
    private GameObject attackEffect;    
    private bool isAttacking;
    public Gradient AttackGradient;

    public GameObject ShieldPrefab;
    private GameObject shield;

    private Vector2 enemyPosition;
    private Vector2 attackEffectPosition;

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
        Name = "Snow White";
        HP = 500;
        MP = 300;        
        Attack = 10;
        Defense = 10;
        Intell = 10;
        Speed = 100;

        swStartPosition = transform.position;

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
        SWAction();        
    }

    void SetEnemyPosition()
    {
        if (StageManager.EnemyInfo[0].name.StartsWith("Lightning"))
        {
            if(GameManager.instance.isAttackButtonActive)
            {
                enemyPosition = GameManager.instance.BossPosition + new Vector2(2.5f, 0.5f);
            }
            else if(GameManager.instance.isSkillButtonActive)
            {
                if(GameManager.instance.SkillButtonNumber == 3)
                {
                    enemyPosition = GameManager.instance.BossPosition + new Vector2(0f, 1.7f);
                }
                else
                {
                    enemyPosition = GameManager.instance.BossPosition;
                }
            }
        }
        else
        {
            if(GameManager.instance.isAttackButtonActive)
            {
                enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(2.5f, 0.5f);
            }
            else if(GameManager.instance.isSkillButtonActive)
            {
                if(GameManager.instance.SkillButtonNumber == 3)
                {
                    enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(0f, 1.7f);
                }
                else
                {
                    enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber];
                }
            }
        }
    }

    void SWAction()
    {
        if (GameManager.instance.isBattleOver)
        {
            Victory();
            Destroy(shield);
        }
        else
        {
            if (GameManager.instance.isSWTurn)
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
                            GameManager.instance.isSWTurn = false;
                            GameManager.instance.isAction = false;
                            GameManager.instance.isAttackButtonActive = false;

                            animator.SetBool("Standby", false);

                            ResetAnimationTrigger("Move");
                            animator.SetBool("Attack", true);

                            if (!isAttacking)
                            {
                                isAttacking = true;                                

                                if (StageManager.EnemyInfo[0].name.StartsWith("Lightning"))
                                {
                                    attackEffectPosition = GameManager.instance.BossPosition;
                                }
                                else
                                {
                                    attackEffectPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber];
                                }

                                attackEffect = Instantiate(AttackEffectPrefab, attackEffectPosition, Quaternion.identity);
                                attackEffect.GetComponent<Animator>().Play("SnowWhite_Attack");

                                var colorOverLifeTime = attackEffect.GetComponentInChildren<ParticleSystem>().colorOverLifetime;
                                colorOverLifeTime.color = new ParticleSystem.MinMaxGradient(AttackGradient);
                            }
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

                        GameManager.instance.isSWTurn = false;
                        GameManager.instance.isAction = false;
                        GameManager.instance.isGuardButtonActive = false;

                        shield = Instantiate(ShieldPrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);

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
                                    doubleSlash = Instantiate(DoubleSlashPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
                                }

                                break;

                            case 1:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    tripleSlash = Instantiate(TripleSlashPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
                                }

                                break;

                            case 2:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    aerialSlash = Instantiate(AerialSlashPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
                                }

                                break;

                            case 3:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    thunderThorn = Instantiate(ThunderThornPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageAllEnemy(100);
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

                            GameManager.instance.isSWTurn = false;
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
                transform.position = swStartPosition;

                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                isAttacking = false;
                Destroy(attackEffect);

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
            if (StageManager.EnemyInfo[i] != null)
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
    }

    void Victory()
    {
        if(GameManager.instance.isBattleOver)
        {
            animator.SetBool("Victory", true);
        }
    }
}
