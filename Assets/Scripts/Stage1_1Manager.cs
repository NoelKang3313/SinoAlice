using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_1Manager : MonoBehaviour
{
    public GameObject[] Enemies = new GameObject[4];

    public GameObject[] CharacterTurn = new GameObject[7];
    [SerializeField]
    private float[] CharacterSpeed = new float[7];
    private int turn = 0;

    void Start()
    {
        CharacterTurn[0] = GameManager.instance.AlicePrefab;
        CharacterTurn[1] = GameManager.instance.GretelPrefab;
        CharacterTurn[2] = GameManager.instance.SnowWhitePrefab;
        CharacterTurn[3] = Enemies[0];
        CharacterTurn[4] = Enemies[1];
        CharacterTurn[5] = Enemies[2];
        CharacterTurn[6] = Enemies[3];

        CharacterSpeed[0] = CharacterTurn[0].GetComponent<Alice>().Speed;
        CharacterSpeed[1] = CharacterTurn[1].GetComponent<Gretel>().Speed;
        CharacterSpeed[2] = CharacterTurn[2].GetComponent<SnowWhite>().Speed;
        CharacterSpeed[3] = CharacterTurn[3].GetComponent<Rat>().Speed;
        CharacterSpeed[4] = CharacterTurn[3].GetComponent<Rat>().Speed;
        CharacterSpeed[5] = CharacterTurn[3].GetComponent<Rat>().Speed;
        CharacterSpeed[6] = CharacterTurn[3].GetComponent<Rat>().Speed;

        SetCharactersTurn();
    }
    
    void Update()
    {
        
    }

    void SetCharactersTurn()
    {
        for(int i = 0; i < CharacterSpeed.Length; i++)
        {
            for(int j = i + 1; j < CharacterSpeed.Length; j++)
            {
                if (CharacterSpeed[i] < CharacterSpeed[j])                    
                {
                    GameObject GO = CharacterTurn[i];
                    CharacterTurn[i] = CharacterTurn[j];
                    CharacterTurn[j] = GO;

                    float speed = CharacterSpeed[i];
                    CharacterSpeed[i] = CharacterSpeed[j];
                    CharacterSpeed[j] = speed;
                }
            }
        }
    }

    void StartCharacterTurn()
    {
        CheckCharacterTurn(turn);        
    }

    void CheckCharacterTurn(int number)
    {
        if (CharacterTurn[number].CompareTag("Character"))
        {
            if (CharacterTurn[number].name == "Alice")
            {
                GameManager.instance.isAliceTurn = true;
            }
            else if (CharacterTurn[number].name == "Gretel")
            {
                GameManager.instance.isGretelTurn = true;
            }
            else if (CharacterTurn[number].name == "Snow White")
            {
                GameManager.instance.isSWTurn = true;
            }
        }
        else if(CharacterTurn[number].CompareTag("Enemy"))
        {
            GameManager.instance.isEnemyTurn = true;
        }
    }
}
