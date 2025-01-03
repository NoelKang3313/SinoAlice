using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Inventory Inventory;

    public bool isTransition;
    public bool isBattleStart;
    public bool isTurn;
    public int TurnNumber;    
    public bool isBattleOver;
    public bool isBossStage;

    public int Gald;

    [Header("Character Selection")]
    public GameObject[] CharacterSelected = new GameObject[3];
    public GameObject AlicePrefab;
    public GameObject GretelPrefab;
    public GameObject SnowWhitePrefab;

    [Header("Selected Enemy")]
    public List<GameObject> EnemySelected = new List<GameObject>();
    public List<GameObject> InstantiatedEnemy = new List<GameObject>();    

    [Header("Characters Equipment Data")]
    public EquipmentData[] AliceEquipments = new EquipmentData[4];
    public EquipmentData[] GretelEquipments = new EquipmentData[4];
    public EquipmentData[] SWEquipments = new EquipmentData[4];

    [Header("Lobby NPCs Buttons Active")]
    public bool isReturnButtonActive;
    public bool isCharlotteButtonActive;
    public bool isWinryButtonActive;
    public bool isLidButtonActive;

    [Header("Character Turn")]
    public bool isAliceTurn;
    public bool isGretelTurn;
    public bool isSWTurn;
    public bool isEnemyTurn;

    [Header("Action Buttons Active")]
    public bool isAttackButtonActive;
    public bool isGuardButtonActive;
    public bool isSkillButtonActive;
    public bool isItemButtonActive;

    public Vector2[] CharacterPositions = new Vector2[3];
    public Vector2 SelectedCharacterPosition;     //To Instantiate Skill to Selceted Character Position (Ex. Heal)

    public Vector2[] EnemyPositions = new Vector2[4];
    public int EnemyPositionNumber;
    public Vector2 BossPosition;

    [Header("Battle Start")]
    public GameObject[] Characters = new GameObject[3];
    public int AlicePositionNumber;
    public int GretelPositionNumber;
    public int SWPositionNumber;

    public int SkillButtonNumber;     //Check Skill Button Number and Instantiate Skill Prefab

    public bool isAction = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(Inventory.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            Destroy(Inventory.gameObject);
        }

        Gald = 50000;

        AlicePrefab.GetComponent<Alice>().CurrentHP = AlicePrefab.GetComponent<Alice>().HP;
        AlicePrefab.GetComponent<Alice>().CurrentMP = AlicePrefab.GetComponent<Alice>().MP;

        GretelPrefab.GetComponent<Gretel>().CurrentHP = GretelPrefab.GetComponent<Gretel>().HP;
        GretelPrefab.GetComponent<Gretel>().CurrentMP = GretelPrefab.GetComponent<Gretel>().MP;

        SnowWhitePrefab.GetComponent<SnowWhite>().CurrentHP = SnowWhitePrefab.GetComponent<SnowWhite>().HP;
        SnowWhitePrefab.GetComponent<SnowWhite>().CurrentMP = SnowWhitePrefab.GetComponent<SnowWhite>().MP;
    }

    void Update()
    {
        CharacterPosition();

        GetDamage(10);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void CharacterPosition()
    {        
        for(int i = 0; i < CharacterSelected.Length; i++)
        {            
            if (CharacterSelected[i] != null)
            {
                if (CharacterSelected[i].name == "Alice")
                {
                    AlicePositionNumber = i;                    
                }
                else if (CharacterSelected[i].name == "Gretel")
                {
                    GretelPositionNumber = i;                    
                }
                else if (CharacterSelected[i].name == "Snow White")
                {
                    SWPositionNumber = i;                    
                }
            }
            else
                break;
        }
    }    

    void GetDamage(int damage)
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AlicePrefab.GetComponent<Alice>().CurrentHP -= damage;
        }
    }
}