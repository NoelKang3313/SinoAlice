using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isAliceTurn;
    public bool isGretelTurn;
    public bool isSWTurn;

    public bool isAttackButtonActive;
    public bool isGuardButtonActive;
    public bool isSkillButtonActive;
    public bool isItemButtonActive;

    public Vector2[] CharacterPositions = new Vector2[3];
    public Vector2 SelectedCharacterPosition;     //To Instantiate Skill to Selceted Character Position (Ex. Heal)

    public Vector2[] AttackEnemyPositions = new Vector2[4];
    public Vector2[] SkillEnemyPositions = new Vector2[4];
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
    }

    void CharacterPosition()
    {
        for(int i = 0; i < Characters.Length; i++)
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
    }
}
