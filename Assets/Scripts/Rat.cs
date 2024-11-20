using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    [Header("Rat Stats")]
    public string Name;
    public float HP;
    public float CurrentHP;    
    public float Attack;
    public float Defense;    
    public float Speed;

    void Awake()
    {
        Name = "Rat";
        HP = 100;
        CurrentHP = HP;
        Attack = 10;
        Defense = 10;
        Speed = 50;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
