using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField]
    StageManager StageManager;
    [SerializeField]
    UIManager UIManager;

    Animator animator;
    SpriteRenderer spriteRenderer;

    public GameObject AttackEffectPrefab;
    private GameObject attackEffect;
    private bool isAttacking;
    public Gradient AttackGradient;
    public Gradient BS_AttackGradient;

    [Header("Lightning Stats")]
    public string Name;
    public float HP;    
    public float CurrentHP;    
    public float Attack;
    public float Defense;
    public float Intell;
    public float Speed;

    private float moveSpeed = 15.0f;

    public bool isCurrentEnemyTurn;

    // Skills
    [SerializeField]
    private int activateSkillNum;
    private int skillRandom;
    private bool isSkillRandom;

    public GameObject ThunderArrowPrefab;
    private GameObject thunderArrow;
    public GameObject SparkSpherePrefab;
    private GameObject sparkSphere;
    public GameObject VoltageExplosionPrefab;
    private GameObject voltageExplosion;
    //public GameObject ZephyrStrikePrefab;
    //private GameObject zephyrStrike;
    //public GameObject HolyArrowPrefab;
    //private GameObject holyArrow;
    //public GameObject JudgementPrefab;
    //private GameObject judgement;
    private bool isSkillInstantiated;

    // Audios
    private AudioSource audioSource;
    public AudioClip[] AttackClips = new AudioClip[3];
    private int attackClipRandom;
    private bool isAudioPlaying;

    [SerializeField]
    private int playerRandom;
    private bool isRandom;

    private bool isBraveShift;

    void Awake()
    {
        Name = "Lightning";
        HP = 500;        
        CurrentHP = HP;        
        Attack = 10;
        Defense = 10;
        Intell = 10;
        Speed = 120;
    }

    void Start()
    {        
        StageManager = FindObjectOfType<StageManager>();
        UIManager = FindObjectOfType<UIManager>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        LightningTurn();
        LightningAudioPlay();
        SkillStandby();
        LightningDead();
    }

    void SortOrder()
    {
        spriteRenderer.sortingOrder = StageManager.MaxSortLayer;

        for(int i = 0; i < GameManager.instance.Characters.Length; i++)
        {
            GameManager.instance.Characters[i].GetComponent<SpriteRenderer>().sortingOrder = StageManager.MinSortLayer;
        }
    }

    void LightningAudioPlay()
    {
        if(isCurrentEnemyTurn)
        {
            if(!isAudioPlaying)
            {
                isAudioPlaying = true;
                attackClipRandom = Random.Range(0, 3);
                audioSource.PlayOneShot(AttackClips[attackClipRandom]);
            }
        }
        else
        {
            isAudioPlaying = false;
        }
    }

    void LightningTurn()
    {
        if (activateSkillNum % 2 != 0 || activateSkillNum == 0)
        {
            if (isCurrentEnemyTurn)
            {
                //UIManager.CharacterMiniGauge.SetActive(false);

                if (!isRandom)
                {
                    isRandom = true;
                    playerRandom = Random.Range(0, 3);
                }

                SortOrder();

                if (!isBraveShift)
                {
                    animator.SetTrigger("Move");

                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(GameManager.instance.Characters[playerRandom].transform.position.x - 3.2f, GameManager.instance.Characters[playerRandom].transform.position.y - 0.1f),
                        moveSpeed * Time.deltaTime);

                    if (transform.position == new Vector3(GameManager.instance.Characters[playerRandom].transform.position.x - 3.2f, GameManager.instance.Characters[playerRandom].transform.position.y - 0.1f, 0f))
                    {
                        ResetAnimationTrigger("Move");
                        animator.SetBool("Attack", true);

                        if (!isAttacking)
                        {
                            isAttacking = true;

                            attackEffect = Instantiate(AttackEffectPrefab, GameManager.instance.Characters[playerRandom].transform.position, Quaternion.identity);
                            attackEffect.GetComponent<Animator>().Play("Lightning_Attack");

                            var colorOverLifeTime = attackEffect.GetComponentInChildren<ParticleSystem>().colorOverLifetime;
                            colorOverLifeTime.color = new ParticleSystem.MinMaxGradient(AttackGradient);
                        }
                    }

                    CheckCurrentAnimation("Attack");
                }
                else
                {
                    animator.SetTrigger("BS_Move");

                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(GameManager.instance.Characters[playerRandom].transform.position.x - 2.5f, GameManager.instance.Characters[playerRandom].transform.position.y - 0.1f),
                        moveSpeed * Time.deltaTime);

                    if (transform.position == new Vector3(GameManager.instance.Characters[playerRandom].transform.position.x - 2.5f, GameManager.instance.Characters[playerRandom].transform.position.y - 0.1f, 0f))
                    {
                        ResetAnimationTrigger("BS_Move");
                        animator.SetBool("BS_Attack", true);

                        if (!isAttacking)
                        {
                            isAttacking = true;

                            attackEffect = Instantiate(AttackEffectPrefab, GameManager.instance.Characters[playerRandom].transform.position, Quaternion.identity);
                            attackEffect.GetComponent<Animator>().Play("BS_Lightning_Attack");

                            var colorOverLifeTime = attackEffect.GetComponentInChildren<ParticleSystem>().colorOverLifetime;
                            colorOverLifeTime.color = new ParticleSystem.MinMaxGradient(BS_AttackGradient);
                        }
                    }

                    CheckCurrentAnimation("BS_Attack");
                }
            }
        }
        else if(activateSkillNum % 2 == 0)
        {
            if(isCurrentEnemyTurn)
            {
                //UIManager.CharacterMiniGauge.SetActive(false);

                if (activateSkillNum == 2 || activateSkillNum == 4)
                {
                    if (!isSkillRandom)
                    {
                        isSkillRandom = true;
                        skillRandom = Random.Range(0, 3);
                    }

                    if (!isBraveShift)
                    {
                        animator.SetBool("MagicAttack", true);
                    }
                    else
                    {
                        animator.SetBool("BS_MagicAttack", true);
                    }

                    switch (skillRandom)
                    {
                        case 0:
                            {
                                if (!isRandom)
                                {
                                    isRandom = true;
                                    playerRandom = Random.Range(0, 3);
                                }

                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    thunderArrow = Instantiate(ThunderArrowPrefab, GameManager.instance.Characters[playerRandom].transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                                }

                                if (thunderArrow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                                {
                                    Destroy(thunderArrow);
                                    isSkillInstantiated = false;

                                    if(!isBraveShift)
                                    {
                                        animator.SetBool("MagicAttack", false);
                                        animator.SetBool("MagicStandby", false);
                                    }
                                    else
                                    {
                                        animator.SetBool("BS_MagicAttack", false);
                                        animator.SetBool("BS_MagicStandby", false);
                                    }

                                    isCurrentEnemyTurn = false;
                                    isSkillRandom = false;
                                    isRandom = false;
                                    GameManager.instance.isEnemyTurn = false;
                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;
                                    activateSkillNum++;

                                    //UIManager.CharacterMiniGauge.SetActive(true);

                                    DamagePlayer(10, playerRandom);
                                }

                                break;
                            }

                        case 1:
                            {
                                if (!isRandom)
                                {
                                    isRandom = true;
                                    playerRandom = Random.Range(0, 3);
                                }

                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    sparkSphere = Instantiate(SparkSpherePrefab, GameManager.instance.Characters[playerRandom].transform.position, Quaternion.identity);
                                }

                                if (sparkSphere.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                                {
                                    Destroy(sparkSphere);
                                    isSkillInstantiated = false;

                                    if (!isBraveShift)
                                    {
                                        animator.SetBool("MagicAttack", false);
                                        animator.SetBool("MagicStandby", false);
                                    }
                                    else
                                    {
                                        animator.SetBool("BS_MagicAttack", false);
                                        animator.SetBool("BS_MagicStandby", false);
                                    }

                                    isCurrentEnemyTurn = false;
                                    isSkillRandom = false;
                                    isRandom = false;
                                    GameManager.instance.isEnemyTurn = false;
                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;
                                    activateSkillNum++;

                                    //UIManager.CharacterMiniGauge.SetActive(true);

                                    DamagePlayer(10, playerRandom);
                                }

                                break;
                            }

                        case 2:
                            {
                                if (!isSkillInstantiated)
                                {
                                    isSkillInstantiated = true;
                                    voltageExplosion = Instantiate(VoltageExplosionPrefab, new Vector2(5, -1.33f), Quaternion.identity);
                                }

                                if (voltageExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                                {
                                    Destroy(voltageExplosion);
                                    isSkillInstantiated = false;

                                    if (!isBraveShift)
                                    {
                                        animator.SetBool("MagicAttack", false);
                                        animator.SetBool("MagicStandby", false);
                                    }
                                    else
                                    {
                                        animator.SetBool("BS_MagicAttack", false);
                                        animator.SetBool("BS_MagicStandby", false);
                                    }

                                    isCurrentEnemyTurn = false;
                                    isSkillRandom = false;
                                    isRandom = false;
                                    GameManager.instance.isEnemyTurn = false;
                                    GameManager.instance.isTurn = true;
                                    GameManager.instance.TurnNumber++;
                                    activateSkillNum++;

                                    //UIManager.CharacterMiniGauge.SetActive(true);

                                    DamageAllPlayer(10);
                                }

                                break;
                            }
                    }                    
                    //else
                    //{
                    //    animator.SetBool("BS_MagicAttack", true);

                    //    switch (skillRandom)
                    //    {
                    //        case 0:
                    //            if (!isRandom)
                    //            {
                    //                isRandom = true;
                    //                playerRandom = Random.Range(0, 3);
                    //            }

                    //            if (!isSkillInstantiated)
                    //            {
                    //                isSkillInstantiated = true;
                    //                zephyrStrike = Instantiate(ZephyrStrikePrefab, GameManager.instance.Characters[playerRandom].transform.position, Quaternion.identity);
                    //            }

                    //            if (zephyrStrike.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    //            {
                    //                Destroy(zephyrStrike);
                    //                isSkillInstantiated = false;

                    //                animator.SetBool("BS_MagicAttack", false);
                    //                animator.SetBool("BS_MagicStandby", false);

                    //                isCurrentEnemyTurn = false;
                    //                isSkillRandom = false;
                    //                isRandom = false;
                    //                GameManager.instance.isEnemyTurn = false;
                    //                GameManager.instance.isTurn = true;
                    //                GameManager.instance.TurnNumber++;
                    //                activateSkillNum++;

                    //                //UIManager.CharacterMiniGauge.SetActive(true);

                    //                DamagePlayer(10, playerRandom);
                    //            }

                    //            break;

                    //        case 1:
                    //            if (!isRandom)
                    //            {
                    //                isRandom = true;
                    //                playerRandom = Random.Range(0, 3);
                    //            }

                    //            if (!isSkillInstantiated)
                    //            {
                    //                isSkillInstantiated = true;
                    //                holyArrow = Instantiate(HolyArrowPrefab, GameManager.instance.Characters[playerRandom].transform.position, Quaternion.identity);
                    //            }

                    //            if (holyArrow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    //            {
                    //                Destroy(holyArrow);
                    //                isSkillInstantiated = false;

                    //                animator.SetBool("BS_MagicAttack", false);
                    //                animator.SetBool("BS_MagicStandby", false);

                    //                isCurrentEnemyTurn = false;
                    //                isSkillRandom = false;
                    //                isRandom = false;
                    //                GameManager.instance.isEnemyTurn = false;
                    //                GameManager.instance.isTurn = true;
                    //                GameManager.instance.TurnNumber++;
                    //                activateSkillNum++;

                    //                //UIManager.CharacterMiniGauge.SetActive(true);

                    //                DamagePlayer(10, playerRandom);
                    //            }

                    //            break;

                    //        case 2:
                    //            if (!isRandom)
                    //            {
                    //                isRandom = true;
                    //                playerRandom = Random.Range(0, 3);
                    //            }

                    //            if (!isSkillInstantiated)
                    //            {
                    //                isSkillInstantiated = true;
                    //                judgement = Instantiate(JudgementPrefab, new Vector2(5, -1.33f), Quaternion.identity);
                    //            }

                    //            if (judgement.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    //            {
                    //                Destroy(judgement);
                    //                isSkillInstantiated = false;

                    //                animator.SetBool("BS_MagicAttack", false);
                    //                animator.SetBool("BS_MagicStandby", false);

                    //                isCurrentEnemyTurn = false;
                    //                isSkillRandom = false;
                    //                isRandom = false;
                    //                GameManager.instance.isEnemyTurn = false;
                    //                GameManager.instance.isTurn = true;
                    //                GameManager.instance.TurnNumber++;
                    //                activateSkillNum++;

                    //                //UIManager.CharacterMiniGauge.SetActive(true);

                    //                DamageAllPlayer(10);
                    //            }

                    //            break;
                    //    }
                    //}
                }
                else if(activateSkillNum == 6)
                {
                    SortOrder();

                    if(!isBraveShift)
                    {
                        animator.SetTrigger("Move");

                        transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(GameManager.instance.CharacterPositions[0].x - 4.0f, GameManager.instance.CharacterPositions[0].y), moveSpeed * Time.deltaTime);

                        if (transform.position == new Vector3(GameManager.instance.CharacterPositions[0].x - 4.0f, GameManager.instance.CharacterPositions[0].y, 0))
                        {
                            ResetAnimationTrigger("Move");
                            animator.SetBool("LimitBreak", true);

                            CheckCurrentAnimation("LimitBreak");
                        }
                    }
                    else
                    {
                        animator.SetTrigger("BS_Move");

                        if (!isRandom)
                        {
                            isRandom = true;
                            playerRandom = Random.Range(0, 3);
                        }

                        transform.position = Vector2.MoveTowards(transform.position,
                            new Vector2(GameManager.instance.Characters[playerRandom].transform.position.x - 3.0f, GameManager.instance.Characters[playerRandom].transform.position.y - 0.1f),
                            moveSpeed * Time.deltaTime);

                        if (transform.position == new Vector3(GameManager.instance.Characters[playerRandom].transform.position.x - 3.0f, GameManager.instance.Characters[playerRandom].transform.position.y - 0.1f, 0f))
                        {
                            ResetAnimationTrigger("BS_Move");
                            animator.SetBool("BS_LimitBreak", true);

                            CheckCurrentAnimation("BS_LimitBreak");
                        }
                    }
                }
            }

            CheckCurrentAnimation("BraveShift");
            CheckCurrentAnimation("-BraveShift");
        }
    }

    void CheckCurrentAnimation(string animation)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation))
        {
            if (animation == "Attack" && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(attackEffect);

                animator.SetBool(animation, false);
                transform.position = GameManager.instance.BossPosition;

                isCurrentEnemyTurn = false;   
                isAttacking = false;
                isRandom = false;
                GameManager.instance.isEnemyTurn = false;
                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                //UIManager.CharacterMiniGauge.SetActive(true);

                DamagePlayer(10, playerRandom);
                activateSkillNum++;                
            }
            else if (animation == "BS_Attack" && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(attackEffect);

                animator.SetBool(animation, false);
                transform.position = GameManager.instance.BossPosition;

                isCurrentEnemyTurn = false;
                isAttacking = false;
                isRandom = false;
                GameManager.instance.isEnemyTurn = false;
                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                //UIManager.CharacterMiniGauge.SetActive(true);

                DamagePlayer(10, playerRandom);
                activateSkillNum++;
            }
            else if(animation == "LimitBreak" && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isCurrentEnemyTurn = false;
                isRandom = false;
                isBraveShift = true;

                animator.SetBool("LimitBreak", false);
                transform.position = GameManager.instance.BossPosition;
            }
            else if(animation == "BraveShift" && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                GameManager.instance.isEnemyTurn = false;
                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                //UIManager.CharacterMiniGauge.SetActive(true);

                DamageAllPlayer(10);
                activateSkillNum = 0;
            }
            else if(animation == "BS_LimitBreak" && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isCurrentEnemyTurn = false;
                isRandom = false;
                isBraveShift = false;

                animator.SetBool("BS_LimitBreak", false);
                transform.position = GameManager.instance.BossPosition;
            }
            else if (animation == "-BraveShift" && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                GameManager.instance.isEnemyTurn = false;
                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                //UIManager.CharacterMiniGauge.SetActive(true);

                DamagePlayer(10, playerRandom);
                activateSkillNum = 0;
            }
        }
    }

    void SkillStandby()
    {
        if (activateSkillNum % 2 == 0)
        {
            if (activateSkillNum == 0 || activateSkillNum == 6)
            {
                if(!isBraveShift)
                {
                    animator.SetBool("MagicStandby", false);
                }
                else
                {
                    animator.SetBool("BS_MagicStandby", false);
                }
            }
            else if (activateSkillNum != 0 || activateSkillNum != 6)
            {
                if(!isBraveShift)
                {
                    animator.SetBool("MagicStandby", true);
                }
                else
                {
                    animator.SetBool("BS_MagicStandby", true);
                }
            }
        }
    }

    void ResetAnimationTrigger(string animation)
    {
        animator.ResetTrigger(animation);
    }

    void DamagePlayer(int damage, int random)
    {
        if (GameManager.instance.CharacterSelected[random].name == "Alice")
        {
            GameManager.instance.CharacterSelected[random].GetComponent<Alice>().CurrentHP -= damage;
        }
        else if (GameManager.instance.CharacterSelected[random].name == "Gretel")
        {
            GameManager.instance.CharacterSelected[random].GetComponent<Gretel>().CurrentHP -= damage;
        }
        else if (GameManager.instance.CharacterSelected[random].name == "Snow White")
        {
            GameManager.instance.CharacterSelected[random].GetComponent<SnowWhite>().CurrentHP -= damage;
        }
    }

    void DamageAllPlayer(int damage)
    {
        GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP -= damage;
        GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP -= damage;
        GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP -= damage;
    }

    void LightningDead()
    {
        if(gameObject != null && CurrentHP <= 0)
        {
            StageManager.CharacterTurns.Remove(gameObject);
            StageManager.CharacterSpeeds.Remove(gameObject.GetComponent<Lightning>().Speed);
            StageManager.EnemyCount--;
        }
    }
}
