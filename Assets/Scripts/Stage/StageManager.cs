using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public List<int> CoinAmount = new List<int>();
    public GameObject CoinPrefab;
    public List<GameObject> coin = new List<GameObject>();
    private bool isCoinInstantiated;
    public int ObtainedCoin;
    private bool isCoinObtained;

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

            if(GameManager.instance.EnemySelected[i].name == "Lightning" || GameManager.instance.EnemySelected[i].name == "Sephiroth" || GameManager.instance.EnemySelected[i].name == "Noctis")
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
            else if(enemy.name.StartsWith("Magitek Armor"))
            {
                enemy.name = "Magitek Armor";
            }
            else if(enemy.name.StartsWith("Scolopendra"))
            {
                enemy.name = "Scolopendra";
            }
            else if (enemy.name.StartsWith("Epimetheus"))
            {
                enemy.name = "Epimetheus";
            }
            else if(enemy.name.StartsWith("Veritas"))
            {
                enemy.name = "Veritas";
            }
            else if (enemy.name.StartsWith("Dark Veritas"))
            {
                enemy.name = "Dark Veritas";
            }
            else if(enemy.name.StartsWith("Samatha"))
            {
                enemy.name = "Samatha";
            }
            else if(enemy.name.StartsWith("Lightning"))
            {
                enemy.name = "Lightning";
            }
            else if(enemy.name.StartsWith("Sephiroth"))
            {
                enemy.name = "Sephiroth";
            }
            else if(enemy.name.StartsWith("Noctis"))
            {
                enemy.name = "Noctis";
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
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Magitek Armor"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<MagitekArmor>().Speed);
            }
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Scolopendra"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Scolopendra>().Speed);
            }
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Epimetheus"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Epimetheus>().Speed);
            }
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Veritas"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Veritas>().Speed);
            }
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Dark Veritas"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<DarkVeritas>().Speed);
            }
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Samatha"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Samatha>().Speed);
            }
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Lightning"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Lightning>().Speed);
            }
            else if(GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Sephiroth"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Sephiroth>().Speed);
            }
            else if (GameManager.instance.InstantiatedEnemy[i].name.StartsWith("Noctis"))
            {
                CharacterSpeeds.Add(GameManager.instance.InstantiatedEnemy[i].GetComponent<Noctis>().Speed);
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

        if(isCoinInstantiated)
        {
            for(int i = 0; i < coin.Count; i++)
            {
                Vector2 currentPosition = coin[i].transform.position;
                Vector2 targetPosition = coin[i].transform.position + new Vector3(0, 0.3f, 0);
                coin[i].transform.position = Vector2.MoveTowards(currentPosition, targetPosition, 0.5f * Time.deltaTime);
            }
        }

        if(GameManager.instance.isBattleOver && !isCoinObtained)
        {
            isCoinObtained = true;
            GameManager.instance.Gald += ObtainedCoin;
        }
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
                else if (CharacterTurns[number].name.StartsWith("Magitek Armor"))
                {
                    CharacterTurns[number].GetComponent<MagitekArmor>().isCurrentEnemyTurn = true;
                }
                else if (CharacterTurns[number].name.StartsWith("Scolopendra"))
                {
                    CharacterTurns[number].GetComponent<Scolopendra>().isCurrentEnemyTurn = true;
                }
                else if (CharacterTurns[number].name.StartsWith("Epimetheus"))
                {
                    CharacterTurns[number].GetComponent<Epimetheus>().isCurrentEnemyTurn = true;
                }
                else if (CharacterTurns[number].name.StartsWith("Veritas"))
                {
                    CharacterTurns[number].GetComponent<Veritas>().isCurrentEnemyTurn = true;
                }
                else if (CharacterTurns[number].name.StartsWith("Dark Veritas"))
                {
                    CharacterTurns[number].GetComponent<DarkVeritas>().isCurrentEnemyTurn = true;
                }
                else if (CharacterTurns[number].name.StartsWith("Samatha"))
                {
                    CharacterTurns[number].GetComponent<Samatha>().isCurrentEnemyTurn = true;
                }
                else if (CharacterTurns[number].name.StartsWith("Lightning"))
                {
                    CharacterTurns[number].GetComponent<Lightning>().isCurrentEnemyTurn = true;
                }
                else if(CharacterTurns[number].name.StartsWith("Sephiroth"))
                {
                    CharacterTurns[number].GetComponent<Sephiroth>().isCurrentEnemyTurn = true;
                }
                else if (CharacterTurns[number].name.StartsWith("Noctis"))
                {
                    CharacterTurns[number].GetComponent<Noctis>().isCurrentEnemyTurn = true;
                }

                GameManager.instance.isEnemyTurn = true;
            }            

            GameManager.instance.isTurn = false;
        }        
    }

    void EnemyDeadCoinDrop()
    {
        if(DeadEnemyPositions.Count != 0 && !isCoinInstantiated)
        {
            isCoinInstantiated = true;

            for(int i = 0; i < DeadEnemyPositions.Count; i++)
            {
                coin.Add(Instantiate(CoinPrefab, DeadEnemyPositions[i], Quaternion.identity));
                coin[i].GetComponentInChildren<TextMeshPro>().text = "+" + CoinAmount[i].ToString();
                ObtainedCoin += CoinAmount[i];
            }
        }

        if(coin.Count != 0)
        {
            for(int i = 0; i < coin.Count; i++)
            {
                if (coin[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Destroy(coin[i]);
                    DeadEnemyPositions.RemoveAt(i);
                    coin.RemoveAt(i);
                    CoinAmount.RemoveAt(i);
                }
            }
        }

        if (DeadEnemyPositions.Count == 0)
        {
            isCoinInstantiated = false;
        }
    }
}
