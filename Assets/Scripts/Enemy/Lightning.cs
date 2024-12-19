using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField]
    StageManager StageManager;

    Animator animator;

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

    public GameObject AttackEffectPrefab;
    private GameObject attackEffect;
    private bool isAttacking;
    public Gradient AttackGradient;

    // Skills
    public GameObject ThunderArrowPrefab;
    private GameObject thunderArrow;
    public GameObject SparkSpherePrefab;
    private GameObject sparkSphere;
    public GameObject VoltageExplosionPrefab;
    private GameObject voltageExplosion;

    // Audios


    int playerRandom;

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

        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        LightningTurn();
        LightningDead();
    }

    void LightningTurn()
    {
        if(isCurrentEnemyTurn)
        {
            playerRandom = Random.Range(0, 3);

            animator.SetTrigger("Move");

            transform.position = Vector2.MoveTowards(transform.position,
                GameManager.instance.Characters[playerRandom].transform.position,
                moveSpeed * Time.deltaTime);

            if(transform.position == GameManager.instance.Characters[playerRandom].transform.position)
            {
                ResetAnimationTrigger("Move");
                animator.SetBool("Attack", true);

                if(!isAttacking)
                {
                    isAttacking = true;
                    attackEffect = Instantiate(AttackEffectPrefab, GameManager.instance.Characters[playerRandom].transform.position, Quaternion.identity);
                    attackEffect.GetComponent<Animator>().Play("Alice_Attack");

                    var colorOverLifeTime = attackEffect.GetComponentInChildren<ParticleSystem>().colorOverLifetime;
                    colorOverLifeTime.color = new ParticleSystem.MinMaxGradient(AttackGradient);
                }
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
                animator.SetBool(animation, false);
                transform.position = GameManager.instance.BossPosition;

                isCurrentEnemyTurn = false;
                isAttacking = false;
                GameManager.instance.isEnemyTurn = false;
                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                isAttacking = false;
                Destroy(attackEffect);

                DamagePlayer(10, playerRandom);
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
