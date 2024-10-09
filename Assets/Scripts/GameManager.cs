using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject Transition;
    public bool isTransition;
    private Animator transitionAnimator;

    [Header("Lobby NPCs Buttons Active")]
    public bool isExitButtonActive;
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

        DontDestroyOnLoad(Transition);
    }

    void Start()
    {
        transitionAnimator = Transition.GetComponent<Animator>();
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

        TransitionActivate();
    }

    void TransitionActivate()
    {
        if(isTransition)
        {
            transitionAnimator.SetBool("isActive", true);

            if(transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Transition") &&
                transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                LoadScene("Worldmap");
            }
        }

        if (SceneManager.GetActiveScene().name == "Lobby")
            return;
        else
        {
            transitionAnimator.SetBool("isActive", false);

            if (transitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("-Transition") &&
                transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isTransition = false;
            }
        }
    }

    void LoadScene(string sceneName)
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
