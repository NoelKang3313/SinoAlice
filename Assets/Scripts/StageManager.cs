using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    //public GameObject[] CharacterTurn;

    public List<GameObject> CharacterTurns;

    public GameObject[] EnemyGameObject = new GameObject[4];     // For Enemy Mini Gauge

    //[SerializeField]
    //private float[] CharacterSpeed = new float[7];

    public List<float> CharacterSpeeds;

    public int EnemyCount = 0;  // Check All Enemies are Eliminated

    void Start()
    {
        //// Array
        //int index = 3;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //CharacterTurn = new GameObject[GameManager.instance.Characters.Length + enemies.Length];

        //CharacterTurn[0] = GameManager.instance.AlicePrefab;
        //CharacterTurn[1] = GameManager.instance.GretelPrefab;
        //CharacterTurn[2] = GameManager.instance.SnowWhitePrefab;

        //CharacterSpeed[0] = CharacterTurn[0].GetComponent<Alice>().Speed;
        //CharacterSpeed[1] = CharacterTurn[1].GetComponent<Gretel>().Speed;
        //CharacterSpeed[2] = CharacterTurn[2].GetComponent<SnowWhite>().Speed;

        //foreach (GameObject obj in enemies)
        //{
        //    CharacterTurn[index] = obj;
        //    CharacterSpeed[index] = obj.GetComponent<Rat>().Speed;
        //    index++;
        //}

        // List
        CharacterTurns = new List<GameObject>();

        CharacterTurns.Add(GameManager.instance.AlicePrefab);
        CharacterTurns.Add(GameManager.instance.GretelPrefab);
        CharacterTurns.Add(GameManager.instance.SnowWhitePrefab);

        CharacterSpeeds.Add(CharacterTurns[0].GetComponent<Alice>().Speed);
        CharacterSpeeds.Add(CharacterTurns[1].GetComponent<Gretel>().Speed);
        CharacterSpeeds.Add(CharacterTurns[2].GetComponent<SnowWhite>().Speed);

        foreach(GameObject obj in enemies)
        {
            CharacterTurns.Add(obj);
            CharacterSpeeds.Add(obj.GetComponent<Rat>().Speed);
            EnemyCount++;
        }

        SetCharactersTurn();
    }
    
    void Update()
    {
        StartCharacterTurn(GameManager.instance.TurnNumber);

        if(EnemyCount == 0)
        {
            GameManager.instance.isBattleOver = true;
        }        

        if(GameManager.instance.TurnNumber == CharacterTurns.Count)
        {
            GameManager.instance.TurnNumber = 0;
        }
    }

    void SetCharactersTurn()
    {
        //// Array
        //for (int i = 0; i < CharacterSpeed.Length; i++)
        //{
        //    for (int j = i + 1; j < CharacterSpeed.Length; j++)
        //    {
        //        if (CharacterSpeed[i] < CharacterSpeed[j])
        //        {
        //            GameObject GO = CharacterTurn[i];
        //            CharacterTurn[i] = CharacterTurn[j];
        //            CharacterTurn[j] = GO;

        //            float speed = CharacterSpeed[i];
        //            CharacterSpeed[i] = CharacterSpeed[j];
        //            CharacterSpeed[j] = speed;
        //        }
        //    }
        //}

        // List
        for (int i = 0; i < CharacterSpeeds.Count; i++)
        {
            for (int j = i + 1; j < CharacterSpeeds.Count; j++)
            {
                if (CharacterSpeeds[i] < CharacterSpeeds[j])
                {
                    GameObject GO = CharacterTurns[i];
                    CharacterTurns[i] = CharacterTurns[j];
                    CharacterTurns[j] = GO;

                    float speed = CharacterSpeeds[i];
                    CharacterSpeeds[i] = CharacterSpeeds[j];
                    CharacterSpeeds[j] = speed;
                }
            }
        }        
    }

    void StartCharacterTurn(int number)
    {
        if (GameManager.instance.isTurn && GameManager.instance.TurnNumber < CharacterTurns.Count)
        {
            if (CharacterTurns[number].name == "Alice")
            {
                GameManager.instance.isAliceTurn = true;
            }
            else if (CharacterTurns[number].name == "Gretel")
            {
                GameManager.instance.isGretelTurn = true;
            }
            else if (CharacterTurns[number].name == "Snow White")
            {
                GameManager.instance.isSWTurn = true;
            }
            else if (CharacterTurns[number].CompareTag("Enemy"))
            {
                CharacterTurns[number].GetComponent<Rat>().isCurrentEnemyTurn = true;
                GameManager.instance.isEnemyTurn = true;
            }            

            GameManager.instance.isTurn = false;
        }        
    }
}
