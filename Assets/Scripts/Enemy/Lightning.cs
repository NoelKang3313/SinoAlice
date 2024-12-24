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

    public GameObject AttackEffectPrefab;
    private GameObject attackEffect;
    private bool isAttacking;
    public Gradient AttackGradient;

    [Header("Lightning Stats")]
    public string Name;
    public float HP;
    public float MP;
    public float CurrentHP;
    public float CurrentMP;
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
    private bool isSkillInstantiated;

    // Limit Break
    private int limitBreakNum;

    // Audios


    [SerializeField]
    private int playerRandom;
    private bool isRandom;

    void Awake()
    {
        Name = "Lightning";
        HP = 1000;
        MP = 1000;
        CurrentHP = HP;
        CurrentMP = MP;
        Attack = 10;
        Defense = 10;
        Intell = 10;
        Speed = 120;
    }

    void Start()
    {
        if(GameObject.Find("StageManager") != null)
        {
            StageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        }

        UIManager = FindAnyObjectByType<UIManager>();

        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        LightningTurn();
        SkillStandby();
        LightningDead();
    }

    void LightningTurn()
    {
        if (activateSkillNum != 2)
        {
            if (isCurrentEnemyTurn)
            {
                UIManager.BossHPGauge.gameObject.SetActive(false);

                if (!isRandom)
                {
                    isRandom = true;
                    playerRandom = Random.Range(0, 3);
                }

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
            }

            CheckCurrentAnimationEnd("Attack");
        }
        else if(activateSkillNum == 2)
        {
            if(isCurrentEnemyTurn)
            {
                UIManager.BossHPGauge.gameObject.SetActive(false);

                if(!isSkillRandom)
                {
                    isSkillRandom = true;
                    skillRandom = Random.Range(0, 3);
                }

                animator.SetBool("MagicAttack", true);

                switch(skillRandom)
                {
                    case 0:
                        {
                            if(!isRandom)
                            {
                                isRandom = true;
                                playerRandom = Random.Range(0, 3);
                            }

                            if(!isSkillInstantiated)
                            {
                                isSkillInstantiated = true;
                                thunderArrow = Instantiate(ThunderArrowPrefab, GameManager.instance.Characters[playerRandom].transform.position + new Vector3(1, 1, 0), Quaternion.identity);
                            }

                            if (thunderArrow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                            {
                                Destroy(thunderArrow);
                                isSkillInstantiated = false;

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);

                                isCurrentEnemyTurn = false;
                                isSkillRandom = false;
                                isRandom = false;
                                GameManager.instance.isEnemyTurn = false;
                                GameManager.instance.isTurn = true;
                                GameManager.instance.TurnNumber++;
                                activateSkillNum = 0;

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

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);

                                isCurrentEnemyTurn = false;
                                isSkillRandom = false;
                                isRandom = false;
                                GameManager.instance.isEnemyTurn = false;
                                GameManager.instance.isTurn = true;
                                GameManager.instance.TurnNumber++;
                                activateSkillNum = 0;

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

                                animator.SetBool("MagicAttack", false);
                                animator.SetBool("MagicStandby", false);

                                isCurrentEnemyTurn = false;
                                isSkillRandom = false;
                                isRandom = false;
                                GameManager.instance.isEnemyTurn = false;
                                GameManager.instance.isTurn = true;
                                GameManager.instance.TurnNumber++;
                                activateSkillNum = 0;

                                DamageAllPlayer(10);
                            }

                            break;
                        }
                }
            }
        }

        if(limitBreakNum == 5)
        {
            if (isCurrentEnemyTurn)
            {
                UIManager.BossHPGauge.gameObject.SetActive(false);

                if (!isRandom)
                {
                    isRandom = true;
                    playerRandom = Random.Range(0, 3);
                }

                animator.SetTrigger("Move");

                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(GameManager.instance.Characters[playerRandom].transform.position.x - 2.8f, GameManager.instance.Characters[playerRandom].transform.position.y - 0.5f),
                    moveSpeed * Time.deltaTime);

                if (transform.position == new Vector3(GameManager.instance.Characters[playerRandom].transform.position.x - 2.8f, GameManager.instance.Characters[playerRandom].transform.position.y - 0.5f, 0f))
                {
                    ResetAnimationTrigger("Move");
                    animator.SetBool("LimitBreak", true);
                }
            }

            CheckCurrentAnimationEnd("Limit Break");
        }
    }

    void CheckCurrentAnimationEnd(string animation)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (animation == "Attack")
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

                DamagePlayer(10, playerRandom);
                activateSkillNum++;
                limitBreakNum++;
            }
            if (animation == "Limit Break")
            {
                animator.SetBool(animation, false);
                transform.position = GameManager.instance.BossPosition;

                isCurrentEnemyTurn = false;
                isRandom = false;
                GameManager.instance.isEnemyTurn = false;
                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                DamagePlayer(10, playerRandom);
                activateSkillNum++;
                limitBreakNum = 0;
            }
        }
    }

    void SkillStandby()
    {
        if(activateSkillNum == 2)
        {
            animator.SetBool("MagicStandby", true);
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
            Destroy(gameObject);
            StageManager.CharacterTurns.Remove(gameObject);
            StageManager.CharacterSpeeds.Remove(gameObject.GetComponent<Lightning>().Speed);
            StageManager.EnemyCount--;
        }
    }
}
