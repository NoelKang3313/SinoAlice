using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_1Manager : MonoBehaviour
{    
    public GameObject[] CharacterTurn;
    
    [SerializeField]
    private float[] CharacterSpeed = new float[7];    

    void Start()
    {
        int index = 3;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        CharacterTurn = new GameObject[GameManager.instance.Characters.Length + enemies.Length];

        CharacterTurn[0] = GameManager.instance.AlicePrefab;
        CharacterTurn[1] = GameManager.instance.GretelPrefab;
        CharacterTurn[2] = GameManager.instance.SnowWhitePrefab;

        CharacterSpeed[0] = CharacterTurn[0].GetComponent<Alice>().Speed;
        CharacterSpeed[1] = CharacterTurn[1].GetComponent<Gretel>().Speed;
        CharacterSpeed[2] = CharacterTurn[2].GetComponent<SnowWhite>().Speed;

        foreach (GameObject obj in enemies)
        {
            CharacterTurn[index] = obj;
            CharacterSpeed[index] = obj.GetComponent<Rat>().Speed;
            index++;
        }

        SetCharactersTurn();
    }
    
    void Update()
    {
        StartCharacterTurn(GameManager.instance.TurnNumber);
    }

    void SetCharactersTurn()
    {
        for (int i = 0; i < CharacterSpeed.Length; i++)
        {
            for (int j = i + 1; j < CharacterSpeed.Length; j++)
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

    void StartCharacterTurn(int number)
    {
        if (GameManager.instance.isTurn && GameManager.instance.TurnNumber < 7)
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
            else if (CharacterTurn[number].CompareTag("Enemy"))
            {
                CharacterTurn[number].GetComponent<Rat>().isCurrentEnemyTurn = true;
                GameManager.instance.isEnemyTurn = true;
            }

            GameManager.instance.isTurn = false;
        }
    }
}
