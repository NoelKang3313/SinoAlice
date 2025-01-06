using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{    
    public List<GameObject> CharacterTurns = new List<GameObject>();

    //public GameObject[] EnemyGameObject = new GameObject[4];     // For Enemy Mini Gauge
    public List<GameObject> EnemyInfo = new List<GameObject>();

    public List<float> CharacterSpeeds;

    public int EnemyCount = 0;  // Check All Enemies are Eliminated    

    public List<Vector2> DeadEnemyPositions = new List<Vector2>();
    public GameObject CoinPrefab;
    private GameObject coin;
    private bool isCoinInstantiated;

    public int MaxSortLayer = 4;
    public int MinSortLayer = 2;

    void Start()
    {
        GameManager.instance.InstantiatedEnemy.Clear();

        GameManager.instance.isBattleStart = true;

        for (int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
        {
            GameManager.instance.Characters[i] = Instantiate(GameManager.instance.CharacterSelected[i], GameManager.instance.CharacterPositions[i], Quaternion.identity);
        }

        for (int i = 0; i < GameManager.instance.EnemySelected.Count; i++)
        {
            GameObject enemy;

            if(GameManager.instance.EnemySelected[i].name == "Lightning")
            {
                enemy = Instantiate(GameManager.instance.EnemySelected[i], GameManager.instance.BossPosition, Quaternion.identity);
            }
            else
            {
                enemy = Instantiate(GameManager.instance.EnemySelected[i], GameManager.instance.EnemyPositions[i], Quaternion.identity);
            }

            if(enemy.name.StartsWith("Rat"))
            {
                enemy.name = "Rat";
            }
            else if(enemy.name.StartsWith("Wolf"))
            {
                enemy.name = "Wolf";
            }
            else if(enemy.name.StartsWith("Lightning"))
            {
                enemy.name = "Lightning";
            }

            GameManager.instance.InstantiatedEnemy.Add(enemy);
            EnemyInfo.Add(enemy);

            EnemyCount++;
        }

        CharacterTurns.Add(GameManager.instance.AlicePrefab);
        CharacterTurns.Add(GameManager.instance.GretelPrefab);
        CharacterTurns.Add(GameManager.instance.SnowWhitePrefab);

        CharacterSpeeds.Add(CharacterTurns[0].GetComponent<Alice>().Speed);
        CharacterSpeeds.Add(CharacterTurns[1].GetComponent<Gretel>().Speed);
        CharacterSpeeds.Add(CharacterTurns[2].GetComponent<SnowWhite>().Speed);

        for (int i = 0; i < GameManager.instance.InstantiatedEnemy.Count; i++)
        {
            CharacterTurns.Add(GameManager.instance.InstantiatedEnemy[i]);

            if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Rat"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Rat>().Speed);
            }
            else if(GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Wolf"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Wolf>().Speed);
            }
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Lightning"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Lightning>().Speed);
            }
        }

        SetCharactersTurn();

        GameManager.instance.isBattleStart = false;
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

        EnemyDeadCoinDrop();
    }

    void SetCharactersTurn()
    {
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
            if (CharacterTurns[number].name.StartsWith("Alice"))
            {
                GameManager.instance.isAliceTurn = true;
            }
            else if (CharacterTurns[number].name.StartsWith("Gretel"))
            {
                GameManager.instance.isGretelTurn = true;
            }
            else if (CharacterTurns[number].name.StartsWith("Snow White"))
            {
                GameManager.instance.isSWTurn = true;
            }
            else if (CharacterTurns[number].CompareTag("Enemy"))
            {
                if(CharacterTurns[number].name.StartsWith("Rat"))
                {
                    CharacterTurns[number].GetComponent<Rat>().isCurrentEnemyTurn = true;
                }
                else if(CharacterTurns[number].name.StartsWith("Wolf"))
                {
                    CharacterTurns[number].GetComponent<Wolf>().isCurrentEnemyTurn = true;
                }
                else if (CharacterTurns[number].name.StartsWith("Lightning"))
                {
                    CharacterTurns[number].GetComponent<Lightning>().isCurrentEnemyTurn = true;
                }

                GameManager.instance.isEnemyTurn = true;
            }            

            GameManager.instance.isTurn = false;
        }        
    }

    void EnemyDeadCoinDrop()
    {
        if(DeadEnemyPositions.Count != 0)
        {
            for(int i = 0; i < DeadEnemyPositions.Count; i++)
            {
                if(!isCoinInstantiated)
                {
                    isCoinInstantiated = true;
                    coin = Instantiate(CoinPrefab, DeadEnemyPositions[i], Quaternion.identity);
                }

                if (coin.activeSelf && coin.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    isCoinInstantiated = false;
                    DeadEnemyPositions.RemoveAt(i);
                    Destroy(coin);
                }
            }
        }
    }
}
