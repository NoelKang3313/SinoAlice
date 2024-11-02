using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isTransition;

    [Header("Character Selection")]
    public GameObject[] CharacterSelected = new GameObject[3];

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

    [Header("Action Buttons Active")]
    public bool isAttackButtonActive;
    public bool isGuardButtonActive;
    public bool isSkillButtonActive;
    public bool isItemButtonActive;

    public Vector2[] CharacterPositions = new Vector2[3];
    public Vector2 SelectedCharacterPosition;     //To Instantiate Skill to Selceted Character Position (Ex. Heal)

    public Vector2[] EnemyPositions = new Vector2[4];
    public int EnemyPositionNumber;

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
        }
        else
        {
            Destroy(this.gameObject);
        }        
    }

    void Update()
    {
        CharacterPosition();
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            isAliceTurn = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            isGretelTurn = true;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            isSWTurn = true;
        }

        Debug.Log(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void CharacterPosition()
    {        
        for(int i = 0; i < Characters.Length; i++)
        {
            if (Characters[i] != null)
            {
                if (Characters[i].name == "Alice")
                {
                    AlicePositionNumber = i;
                }
                else if (Characters[i].name == "Gretel")
                {
                    GretelPositionNumber = i;
                }
                else if (Characters[i].name == "Snow White")
                {
                    SWPositionNumber = i;
                }
            }
            else
                break;
        }
    }
}
