using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{    
    public static GameManager instance;

    public bool isTransition;
    public bool isBattleStart;

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
        StageEnterInstantiateCharacter();

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

    void StageEnterInstantiateCharacter()
    {
        if(isBattleStart)
        {
            for(int i = 0; i < CharacterSelected.Length; i++)
            {
                Characters[i] = Instantiate(CharacterSelected[i], CharacterPositions[i], Quaternion.identity);
            }

            isBattleStart = false;
        }
    }
}
