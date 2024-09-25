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

    public Vector2[] EnemyPositions = new Vector2[4];
    public int EnemyPositionNumber;

    public GameObject[] Characters = new GameObject[3];
    public int AlicePositionNumber;
    public int GretelPositionNumber;
    public int SWPositionNumber;

    public bool isAction = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        CharacterPosition();
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
