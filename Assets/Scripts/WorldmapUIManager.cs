using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldmapUIManager : MonoBehaviour
{
    public Button Stage1Button;
    public GameObject BackgroundGameObject;
    public Sprite WorldBG;
    public Sprite StageBG;

    public Button ReturnButton;
    private bool isWorldmap;      // Check if current map is worldmap or stage map
    public Button Stage1_1Button;

    public GameObject StagePanel;
    public Button StagePanelExitButton;

    public Button[] CharacterLocationButtons = new Button[3];
    [SerializeField] private int CharacterLocationIndex;    
    public GameObject CharacterSelectionScroll;
    public Button[] CharacterSelectionButtons = new Button[3];

    public GameObject SelectedCharacter;
    //public GameObject[] CharacterPrefabs = new GameObject[3];
    public Sprite[] CharactersIdle = new Sprite[3];
    [SerializeField] private int changeIndex;

    public Image[] EnemyImages = new Image[4];
    public Sprite RatSprite;

    [SerializeField]
    private int characterCount;
    public Button BattleStartButton;

    public Image TransitionImage;

    void Start()
    {
        //for(int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
        //{
        //    GameManager.instance.CharacterSelected[i] = null;
        //}

        isWorldmap = true;

        Stage1Button.onClick.AddListener(Stage1ButtonClicked);
        ReturnButton.onClick.AddListener(ReturnButtonClicked);
        Stage1_1Button.onClick.AddListener(Stage1_1ButtonClicked);

        StagePanelExitButton.onClick.AddListener(StagePanelExitButtonClicked);

        for(int i = 0; i < CharacterLocationButtons.Length; i++)
        {
            int number = i;
            CharacterLocationButtons[i].onClick.AddListener(() => CharacterLocationButtonClicked(number));
        }

        for(int i = 0; i < CharacterSelectionButtons.Length; i++)
        {
            int number = i;
            CharacterSelectionButtons[i].onClick.AddListener(() => CharacterSelectionButtonClicked(number));
        }

        BattleStartButton.onClick.AddListener(BattleStartButtonClicked);

        for (int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
        {
            if (GameManager.instance.CharacterSelected[i] == null)
            {
                break;
            }
            else
            {
                if (GameManager.instance.CharacterSelected[i].name == "Alice")
                {
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[0];
                    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
                else if (GameManager.instance.CharacterSelected[i].name == "Gretel")
                {
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[1];
                    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
                else if (GameManager.instance.CharacterSelected[i].name == "Snow White")
                {
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[2];
                    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
            }
        }
    }

    void Update()
    {
        ActivateTransition(1.0f);

        if(TransitionImage.fillAmount == 0)
        {
            TransitionImage.gameObject.SetActive(false);
        }

        StartCoroutine(DelaySceneChange());
    }

    void Stage1ButtonClicked()
    {
        Stage1Button.gameObject.SetActive(false);
        isWorldmap = false;

        BackgroundGameObject.GetComponent<SpriteRenderer>().sprite = StageBG;
        BackgroundGameObject.transform.localScale = new Vector3(1, 0.53f, 0);
        
        Stage1_1Button.gameObject.SetActive(true);
    }

    void ReturnButtonClicked()
    {
        if(isWorldmap)
        {
            //Return Lobby
            TransitionImage.gameObject.SetActive(true);
            GameManager.instance.isTransition = true;   
        }
        else
        {
            //Return Worldmap
            BackgroundGameObject.GetComponent<SpriteRenderer>().sprite = WorldBG;
            BackgroundGameObject.transform.localScale = new Vector3(1.3f, 1, 1);

            isWorldmap = true;
        }
    }

    void Stage1_1ButtonClicked()
    {
        StagePanel.SetActive(true);
        ReturnButton.gameObject.SetActive(false);

        for(int i = 0; i < EnemyImages.Length; i++)
        {
            EnemyImages[i].sprite = RatSprite;
        }
    }

    void StagePanelExitButtonClicked()
    {
        StagePanel.SetActive(false);
        ReturnButton.gameObject.SetActive(true);
    }

    void CharacterLocationButtonClicked(int number)
    {
        CharacterSelectionScroll.SetActive(true);
        CharacterLocationIndex = number;        
    }

    void SwitchCharacterLocation(int oldNum, int newNum)
    {
        GameManager.instance.CharacterSelected[CharacterLocationIndex] = GameManager.instance.CharacterSelected[oldNum];
        GameManager.instance.CharacterSelected[oldNum] = null;

        CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().sprite = CharactersIdle[newNum];
        CharacterLocationButtons[CharacterLocationIndex].GetComponent<Image>().color = new Color(0, 0, 0, 0);

        CharacterLocationButtons[oldNum].transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        CharacterLocationButtons[oldNum].transform.parent.GetComponent<Image>().sprite = null;
        CharacterLocationButtons[oldNum].GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    void CharacterSelectionButtonClicked(int number)
    {
        switch(number)
        {
            case 0:
                SelectedCharacter = GameManager.instance.AlicePrefab;
                break;

            case 1:
                SelectedCharacter = GameManager.instance.GretelPrefab;
                break;

            case 2:
                SelectedCharacter = GameManager.instance.SnowWhitePrefab;
                break;
        }

        if (GameManager.instance.CharacterSelected[CharacterLocationIndex] == null)
        {
            if(GameManager.instance.CharacterSelected[0] == SelectedCharacter)
            {
                SwitchCharacterLocation(0, number);
            }
            else if(GameManager.instance.CharacterSelected[1] == SelectedCharacter)
            {
                SwitchCharacterLocation(1, number);
            }
            else if(GameManager.instance.CharacterSelected[2] == SelectedCharacter)
            {
                SwitchCharacterLocation(2, number);
            }
            else
            {
                GameManager.instance.CharacterSelected[CharacterLocationIndex] = SelectedCharacter;
                CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().sprite = CharactersIdle[number];
                CharacterLocationButtons[CharacterLocationIndex].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            for(int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
            {
                if (GameManager.instance.CharacterSelected[i] == SelectedCharacter)
                {
                    GameObject GO = GameManager.instance.CharacterSelected[CharacterLocationIndex];
                    Sprite GOS = CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().sprite;

                    GameManager.instance.CharacterSelected[CharacterLocationIndex] = SelectedCharacter;
                    CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().sprite = CharactersIdle[number];

                    GameManager.instance.CharacterSelected[i] = GO;
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = GOS;

                    break;
                }
                else
                    continue;
            }
        }

        CharacterSelectionScroll.SetActive(false);
    }

    void BattleStartButtonClicked()
    {
        characterCount = 0;

        for(int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
        {
            if(GameManager.instance.CharacterSelected[i] != null)
            {
                characterCount++;
            }
            else
            {
                break;
            }
        }

        if(characterCount == 3)
        {            
            TransitionImage.gameObject.SetActive(true);
            GameManager.instance.isTransition = true;            
        }
        else
        {
            Debug.Log("Cannot Enter");
        }
    }

    void ActivateTransition(float transitionSpeed)
    {
        if (GameManager.instance.isTransition)
        {
            TransitionImage.fillAmount += transitionSpeed / 1.0f * Time.deltaTime;
        }
        else
        {
            TransitionImage.fillAmount -= transitionSpeed / 1.0f * Time.deltaTime;
        }
    }

    IEnumerator DelaySceneChange()
    {
        if (TransitionImage.fillAmount == 1.0f)
        {
            yield return new WaitForSeconds(2.0f);

            GameManager.instance.isTransition = false;

            if(isWorldmap)
            {
                GameManager.instance.LoadScene("Lobby");
            }
            else
            {
                GameManager.instance.isBattleStart = true;
                GameManager.instance.LoadScene("Stage1-1");
            }
        }
    }
}
