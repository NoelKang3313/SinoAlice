using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    Animator animator;

    [Header("Rat Stats")]
    public string Name;
    public float HP;
    public float MP;
    public float CurrentHP;
    public float CurrentMP;
    public float Attack;
    public float Defense;    
    public float Speed;

    public bool isCurrentEnemyTurn;

    public GameObject AttackEffectPrefab;
    private GameObject attackEffect;
    private bool isAttacking;
    public Gradient AttackGradient;

    void Awake()
    {
        Name = "Rat";
        HP = 100;
        MP = 100;
        CurrentHP = HP;
        CurrentMP = MP;
        Attack = 10;
        Defense = 10;
        Speed = 50;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RatTurn();        
        EndTurn();
    }

    void RatTurn()
    {
        if(isCurrentEnemyTurn)
        {            
            animator.SetBool("isAttack", true);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isCurrentEnemyTurn = false;
                animator.SetBool("isAttack", false);

                if (!isAttacking && !animator.GetBool("isAttack"))
                {
                    isAttacking = true;

                    int random = Random.Range(0, 3);
                    attackEffect = Instantiate(AttackEffectPrefab, GameManager.instance.Characters[random].transform.position, Quaternion.identity);
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
            }
        }
    }
}
