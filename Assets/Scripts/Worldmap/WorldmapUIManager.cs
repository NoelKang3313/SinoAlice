using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldmapUIManager : MonoBehaviour
{
    public Button Stage1Button;
    public GameObject Stage1;
    public SpriteRenderer Background;
    public Sprite WorldBG;
    public Sprite Stage1BG;

    public GameObject StageSelectionPanel;
    public Button StageSelectionExitButton;

    public Button ReturnButton;
    [SerializeField]
    private bool isLobby;    

    public Button[] Stage1Buttons = new Button[3];

    public GameObject StagePanel;
    public TextMeshProUGUI StagePanelTitle;
    public GameObject EnemyObjects;
    public Image[] EnemyImages = new Image[4];
    public GameObject BossImage;
    public Sprite RatSprite;
    public GameObject RatPrefab;
    public Sprite WolfSprite;
    public GameObject WolfPrefab;
    public Sprite BossSprite;
    public GameObject BossPrefab;
    public Button StagePanelExitButton;

    public Button[] CharacterLocationButtons = new Button[3];
    [SerializeField] private int CharacterLocationIndex;
    public GameObject CharacterSelectionScroll;
    public Button[] CharacterSelectionButtons = new Button[3];

    public GameObject SelectedCharacter;
    //public GameObject[] CharacterPrefabs = new GameObject[3];
    //public Sprite[] CharactersIdle = new Sprite[3];
    [SerializeField] private int changeIndex;

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

        Stage1Button.onClick.AddListener(Stage1ButtonClicked);
        ReturnButton.onClick.AddListener(ReturnButtonClicked);
        //Stage1_1Button.onClick.AddListener(Stage1_1ButtonClicked);

        StageSelectionExitButton.onClick.AddListener(StageSelectionExitButtonClicked);

        StagePanelExitButton.onClick.AddListener(StagePanelExitButtonClicked);

        for (int i = 0; i < CharacterLocationButtons.Length; i++)
        {
            int number = i;
            CharacterLocationButtons[i].onClick.AddListener(() => CharacterLocationButtonClicked(number));
        }

        for (int i = 0; i < CharacterSelectionButtons.Length; i++)
        {
            int number = i;
            CharacterSelectionButtons[i].onClick.AddListener(() => CharacterSelectionButtonClicked(number));
        }

        for (int i = 0; i < Stage1Buttons.Length; i++)
        {
            int number = i;
            Stage1Buttons[i].onClick.AddListener(() => Stage1ButtonsClicked(number));
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
                    //CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[0];
                    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("Alice_Idle");
                    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
                else if (GameManager.instance.CharacterSelected[i].name == "Gretel")
                {
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    //CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[1];
                    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("Gretel_Idle");
                    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
                else if (GameManager.instance.CharacterSelected[i].name == "Snow White")
                {
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    //CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[2];
                    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("SnowWhite_Idle");
                    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
            }
        }
    }

    string CharacterImageAnimation(int number)
    {
        if (GameManager.instance.CharacterSelected[number].name == "Alice")
        {
            return "Alice_Idle";
        }
        else if (GameManager.instance.CharacterSelected[number].name == "Gretel")
        {
            return "Gretel_Idle";
        }
        else if (GameManager.instance.CharacterSelected[number].name == "Snow White")
        {
            return "SnowWhite_Idle";
        }
        else
        {
            return null;
        }
    }

    void Update()
    {
        ActivateTransition(1.0f);

        if (TransitionImage.fillAmount == 0)
        {
            TransitionImage.gameObject.SetActive(false);
        }

        StartCoroutine(DelaySceneChange());
    }

    void Stage1ButtonClicked()
    {
        Stage1.SetActive(false);

        StageSelectionPanel.SetActive(true);

        Background.GetComponent<Transform>().localScale = new Vector3(1.67f, 1, 1);
        Background.sprite = Stage1BG;

        ReturnButton.gameObject.SetActive(false);
    }

    void ReturnButtonClicked()
    {
        isLobby = true;

        TransitionImage.gameObject.SetActive(true);
        GameManager.instance.isTransition = true;
    }

    void StageSelectionExitButtonClicked()
    {
        Background.GetComponent<Transform>().localScale = new Vector3(1.3f, 1, 1);
        Background.sprite = WorldBG;

        StageSelectionPanel.SetActive(false);
        Stage1.SetActive(true);

        ReturnButton.gameObject.SetActive(true);
    }

    void StagePanelExitButtonClicked()
    {
        StagePanel.SetActive(false);
        CharacterSelectionScroll.SetActive(false);
        StageSelectionPanel.SetActive(true);
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
        //CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().sprite = CharactersIdle[newNum];
        CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Animator>().Play(CharacterImageAnimation(newNum));
        CharacterLocationButtons[CharacterLocationIndex].GetComponent<Image>().color = new Color(0, 0, 0, 0);

        CharacterLocationButtons[oldNum].transform.parent.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        CharacterLocationButtons[oldNum].transform.parent.GetComponent<Image>().sprite = null;
        CharacterLocationButtons[oldNum].GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    void CharacterSelectionButtonClicked(int number)
    {
        switch (number)
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
            if (GameManager.instance.CharacterSelected[0] == SelectedCharacter)
            {
                SwitchCharacterLocation(0, number);
            }
            else if (GameManager.instance.CharacterSelected[1] == SelectedCharacter)
            {
                SwitchCharacterLocation(1, number);
            }
            else if (GameManager.instance.CharacterSelected[2] == SelectedCharacter)
            {
                SwitchCharacterLocation(2, number);
            }
            else
            {
                GameManager.instance.CharacterSelected[CharacterLocationIndex] = SelectedCharacter;
                CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                //CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().sprite = CharactersIdle[number];
                CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Animator>().Play(CharacterImageAnimation(number));
                CharacterLocationButtons[CharacterLocationIndex].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
            {
                if (GameManager.instance.CharacterSelected[i] == SelectedCharacter)
                {
                    GameObject GO = GameManager.instance.CharacterSelected[CharacterLocationIndex];
                    Sprite GOS = CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().sprite;

                    GameManager.instance.CharacterSelected[CharacterLocationIndex] = SelectedCharacter;
                    //CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Image>().sprite = CharactersIdle[number];
                    CharacterLocationButtons[CharacterLocationIndex].transform.parent.GetComponent<Animator>().Play(CharacterImageAnimation(CharacterLocationIndex));

                    GameManager.instance.CharacterSelected[i] = GO;
                    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = GOS;
                    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play(CharacterImageAnimation(i));

                    break;
                }
            }
        }

        CharacterSelectionScroll.SetActive(false);
    }

    void Stage1ButtonsClicked(int number)
    {
        StageSelectionPanel.SetActive(false);
        StagePanel.SetActive(true);

        if(GameManager.instance.EnemySelected.Count != 0)
        {
            GameManager.instance.EnemySelected.Clear();
        }

        switch (number)
        {
            case 0:
                StagePanelTitle.text = "????????? 1 - " + (number + 1);

                EnemyObjects.SetActive(true);
                BossImage.SetActive(false);

                for (int i = 0; i < EnemyImages.Length; i++)
                {
                    EnemyImages[i].sprite = RatSprite;
                    EnemyImages[i].GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
                    GameManager.instance.EnemySelected.Add(RatPrefab);
                }
                break;

            case 1:
                StagePanelTitle.text = "????????? 1 - " + (number + 1);

                EnemyObjects.SetActive(true);
                BossImage.SetActive(false);

                for (int i = 0; i < EnemyImages.Length; i++)
                {
                    if(i % 2 == 0)
                    {
                        EnemyImages[i].sprite = WolfSprite;
                        EnemyImages[i].GetComponent<RectTransform>().sizeDelta = new Vector2(250, 150);
                        GameManager.instance.EnemySelected.Add(WolfPrefab);
                    }
                    else
                    {
                        EnemyImages[i].sprite = RatSprite;
                        EnemyImages[i].GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
                        GameManager.instance.EnemySelected.Add(RatPrefab);
                    }
                }
                break;

            case 2:
                StagePanelTitle.text = "????????? 1 - " + (number + 1);

                EnemyObjects.SetActive(false);
                BossImage.SetActive(true);

                BossImage.GetComponent<Image>().sprite = BossSprite;
                BossImage.GetComponent<RectTransform>().sizeDelta = new Vector3(350, 350);
                GameManager.instance.EnemySelected.Add(BossPrefab);

                break;
        }
    }

    void BattleStartButtonClicked()
    {
        characterCount = 0;

        for (int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
        {
            if (GameManager.instance.CharacterSelected[i] != null)
            {
                characterCount++;
            }
            else
            {
                break;
            }
        }

        if (characterCount == 3)
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

            if (isLobby)
            {
                isLobby = false;
                GameManager.instance.LoadScene("Lobby");
            }
            else
            {
                GameManager.instance.isBattleStart = true;
                GameManager.instance.LoadScene("Stage1");
            }
        }
    }
}