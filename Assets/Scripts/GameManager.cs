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

    [Header("Characters HP & MP Gauge")]
    public float AliceCurrentHP;
    public float AliceCurrentMP;
    public float GretelCurrentHP;
    public float GretelCurrentMP;
    public float SWCurrentHP;
    public float SWCurrentMP;

    public float AliceFullHP;
    public float AliceFullMP;
    public float GretelFullHP;
    public float GretelFullMP;
    public float SWFullHP;
    public float SWFullMP;

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

        AliceFullHP = AlicePrefab.GetComponent<Alice>().HP;
        AliceFullMP = AlicePrefab.GetComponent<Alice>().MP;

        GretelFullHP = GretelPrefab.GetComponent<Gretel>().HP;
        GretelFullMP = GretelPrefab.GetComponent<Gretel>().MP;

        SWFullHP = SnowWhitePrefab.GetComponent<SnowWhite>().HP;
        SWFullMP = SnowWhitePrefab.GetComponent<SnowWhite>().MP;

        AliceCurrentHP = AliceFullHP;
        AliceCurrentMP = AliceFullMP;

        GretelCurrentHP = GretelFullHP;
        GretelCurrentMP = GretelFullMP;

        SWCurrentHP = SWFullHP;
        SWCurrentMP = SWFullMP;
    }

    void Update()
    {
        CharacterPosition();
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
}