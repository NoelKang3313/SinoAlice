using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField]
    StageManager StageManager;

    Animator animator;

    [Header("Wolf Stats")]    
    public string Name;
    public float HP;    
    public float CurrentHP;
    public float Attack;
    public float Defense;
    public float Speed;

    public bool isCurrentEnemyTurn;

    public GameObject AttackEffectPrefab;
    private GameObject attackEffect;
    private bool isAttacking;
    public Gradient AttackGradient;

    private int playerRandom;

    void Awake()
    {
        Name = "Wolf";
        HP = 200;        
        CurrentHP = HP;        
        Attack = 10;
        Defense = 10;
        Speed = 80;
    }

    void Start()
    {
        if (GameObject.Find("StageManager") != null)
        {
            StageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        WolfTurn();
        EndTurn();
        EnemyDead();
    }

    void WolfTurn()
    {
        if (isCurrentEnemyTurn)
        {
            animator.SetBool("isAttack", true);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isCurrentEnemyTurn = false;
                animator.SetBool("isAttack", false);

                if (!isAttacking && !animator.GetBool("isAttack"))
                {
                    isAttacking = true;

                    playerRandom = Random.Range(0, 3);
                    attackEffect = Instantiate(AttackEffectPrefab, GameManager.instance.Characters[playerRandom].transform.position, Quaternion.identity);
                    attackEffect.GetComponent<Animator>().Play("Enemy_Attack");

                    var colorOverLifeTime = attackEffect.GetComponentInChildren<ParticleSystem>().colorOverLifetime;
                    colorOverLifeTime.color = new ParticleSystem.MinMaxGradient(AttackGradient);
                }
            }
        }
    }

    void EndTurn()
    {
        if (attackEffect != null)
        {
            if (attackEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack") && attackEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(attackEffect);

                isAttacking = false;
                GameManager.instance.isEnemyTurn = false;
                GameManager.instance.isTurn = true;
                GameManager.instance.TurnNumber++;

                DamagePlayer(10, playerRandom);
            }
        }
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

    void EnemyDead()
    {
        if (gameObject != null && CurrentHP <= 0)
        {
            Destroy(gameObject);
            StageManager.DeadEnemyPositions.Add(gameObject.transform.position);
            StageManager.CoinAmount.Add(30);
            StageManager.CharacterTurns.Remove(gameObject);
            StageManager.CharacterSpeeds.Remove(gameObject.GetComponent<Wolf>().Speed);
            StageManager.EnemyCount--;
        }
    }
}
