using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Alice : MonoBehaviour
{
    [SerializeField]
    UIManager UIManager;

    [SerializeField]
    StageManager StageManager;

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
    private SpriteRenderer spriteRenderer;

    public GameObject AttackEffectPrefab;
    private GameObject attackEffect;
    private bool isAttacking;
    public Gradient AttackGradient;

    public GameObject ShieldPrefab;
    private GameObject shield;

    private Vector2 enemyPosition;
    private Vector2 attackEffectPosition;

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

    void Awake()
    {
        Name = "Alice";
        HP = 250;
        MP = 250;
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
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(GameObject.Find("UIManager") != null)
        {
            UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        }

        if(GameObject.Find("StageManager") != null)
        {
            StageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        }
    }

    void Update()
    {     
        AliceAction();
        Victory();
    }

    void SetEnemyPosition()
    {
        if (StageManager.EnemyInfo[0].name.StartsWith("Lightning"))
        {
            if (GameManager.instance.isAttackButtonActive)
            {
                enemyPosition = GameManager.instance.BossPosition + new Vector2(1.8f, 0);
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                if (GameManager.instance.SkillButtonNumber == 0)
                {
                    enemyPosition = GameManager.instance.BossPosition + new Vector2(1.0f, 1.0f);
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
                enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(1.8f, 0f);
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                if (GameManager.instance.SkillButtonNumber == 0)
                {
                    enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber] + new Vector2(1.0f, 1.0f);
                }
                else
                {
                    enemyPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber];
                }
            }
        }
    }

    void SortOrder()
    {
        spriteRenderer.sortingOrder = StageManager.MaxSortLayer;

        for(int i = 0; i < StageManager.EnemyInfo.Count; i++)
        {
            if(StageManager.EnemyInfo[i] != null)
            {
                if (StageManager.EnemyInfo[i].name.StartsWith("Lightning"))
                {
                    StageManager.EnemyInfo[i].GetComponent<SpriteRenderer>().sortingOrder = StageManager.MinSortLayer;
                }
                else
                {
                    StageManager.EnemyInfo[i].GetComponent<SpriteRenderer>().sortingOrder = StageManager.MinSortLayer;
                }
            }
        }
    }

    void AliceAction()
    {
        if (GameManager.instance.isBattleOver)
        {
            Victory();
            Destroy(shield);
        }
        else
        {
            if (GameManager.instance.isAliceTurn)
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

                SortOrder();

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

                        UIManager.CharacterMiniGauge.SetActive(false);

                        SetEnemyPosition();

                        animator.SetTrigger("Move");

                        transform.position = Vector2.MoveTowards(transform.position, enemyPosition, moveSpeed * Time.deltaTime);

                        if (transform.position == new Vector3(enemyPosition.x, enemyPosition.y, 0))
                        {
                            spriteRenderer.sortingOrder = StageManager.MaxSortLayer;

                            GameManager.instance.isAliceTurn = false;
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
                                    StageManager.EnemyInfo[0].GetComponent<SpriteRenderer>().sortingOrder = StageManager.MinSortLayer;
                                    attackEffectPosition = GameManager.instance.BossPosition;
                                }
                                else
                                {
                                    StageManager.EnemyInfo[GameManager.instance.EnemyPositionNumber].GetComponent<SpriteRenderer>().sortingOrder = StageManager.MinSortLayer;
                                    attackEffectPosition = GameManager.instance.EnemyPositions[GameManager.instance.EnemyPositionNumber];
                                }

                                attackEffect = Instantiate(AttackEffectPrefab, attackEffectPosition, Quaternion.identity);
                                attackEffect.GetComponent<Animator>().Play("Alice_Attack");

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

                        GameManager.instance.isAliceTurn = false;
                        GameManager.instance.isAction = false;
                        GameManager.instance.isGuardButtonActive = false;

                        shield = Instantiate(ShieldPrefab, transform.position, Quaternion.identity);
                        shield.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;

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
                                    glacialArrow = Instantiate(GlacialArrowPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
                                }

                                break;

                            case 1:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    aquaBomb = Instantiate(AquaBombPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
                                }

                                break;

                            case 2:
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    blizzardBomb = Instantiate(BlizzardBombPrefab, enemyPosition, Quaternion.identity);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageEnemy(10);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    DamageAllEnemy(50);
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

                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;

                                    HealPlayer(10);
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

                        if(!isItemHealInstantiated)
                        {
                            isItemHealInstantiated = true;
                            itemHeal = Instantiate(ItemHealPrefab, GameManager.instance.SelectedCharacterPosition + new Vector2(0, 0.25f), Quaternion.identity);
                        }

                        if(itemHeal.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        {
                            Destroy(itemHeal);
                            isItemHealInstantiated = false;

                            GameManager.instance.isAliceTurn = false;
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
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (animation == "Attack")
            {
                animator.SetBool("Attack", false);
                transform.position = aliceStartPosition;

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
        for(int i = 0; i < StageManager.EnemyInfo.Count; i++)
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

    void HealPlayer(int heal)
    {
        for(int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
        {
            if(GameManager.instance.AlicePositionNumber == i)
            {
                if(GameManager.instance.CharacterSelected[i].GetComponent<Alice>().CurrentHP >= GameManager.instance.CharacterSelected[i].GetComponent<Alice>().HP)
                {
                    GameManager.instance.CharacterSelected[i].GetComponent<Alice>().CurrentHP += 0;
                }
                else
                {
                    GameManager.instance.CharacterSelected[i].GetComponent<Alice>().CurrentHP += 10;
                }
            }
            else if(GameManager.instance.GretelPositionNumber == i)
            {
                if (GameManager.instance.CharacterSelected[i].GetComponent<Gretel>().CurrentHP >= GameManager.instance.CharacterSelected[i].GetComponent<Gretel>().HP)
                {
                    GameManager.instance.CharacterSelected[i].GetComponent<Gretel>().CurrentHP += 0;
                }
                else
                {
                    GameManager.instance.CharacterSelected[i].GetComponent<Gretel>().CurrentHP += 10;
                }
            }
            else if(GameManager.instance.SWPositionNumber == i)
            {
                if (GameManager.instance.CharacterSelected[i].GetComponent<SnowWhite>().CurrentHP >= GameManager.instance.CharacterSelected[i].GetComponent<SnowWhite>().HP)
                {
                    GameManager.instance.CharacterSelected[i].GetComponent<SnowWhite>().CurrentHP += 0;
                }
                else
                {
                    GameManager.instance.CharacterSelected[i].GetComponent<SnowWhite>().CurrentHP += 10;
                }
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
