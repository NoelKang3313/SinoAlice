using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldmapUIManager : MonoBehaviour
{
    //public Button Stage1Button;
    //public Button Stage2Button;
    //public Button Stage3Button;
    //public GameObject Stage1;
    //public GameObject Stage2;
    //public GameObject Stage3;

    public Button[] StageButton = new Button[3];
    public GameObject[] StageGameObject = new GameObject[3];
    public Button[] StagesButtons = new Button[3];

    public SpriteRenderer Background;
    public Sprite WorldBG;
    public Sprite[] StageBG = new Sprite[3];
    //public Sprite Stage1BG;    

    public GameObject StageSelectionPanel;
    public Button StageSelectionExitButton;

    public Button ReturnButton;
    [SerializeField]
    private bool isLobby;

    public GameObject StagePanel;
    public TextMeshProUGUI StagePanelTitle;
    public GameObject EnemyObjects;
    public Image[] EnemyImages = new Image[4];
    public GameObject BossImage;
    public Sprite RatSprite;
    public GameObject RatPrefab;
    public Sprite WolfSprite;
    public GameObject WolfPrefab;
    public GameObject MagitekArmorPrefab;
    public Sprite MagitekArmorSprite;
    public GameObject ScolopendraPrefab;
    public Sprite ScolopendraSprite;
    public GameObject EpimetheusPrefab;
    public Sprite EpimetheusSprite;
    public GameObject VeritasPrefab;
    public Sprite VeritasSprite;
    public GameObject DarkVeritasPrefab;
    public Sprite DarkVeritasSprite;
    public GameObject SamathaPrefab;
    public Sprite SamathaSprite;

    public GameObject LightningPrefab;
    public GameObject SephirothPrefab;
    public GameObject NoctisPrefab;

    public Vector2[] FourEnemyPosition = new Vector2[4];
    public Vector2[] ThreeEnemyPosition = new Vector2[3];

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

        //Stage1Button.onClick.AddListener(Stage1ButtonClicked);
        ReturnButton.onClick.AddListener(ReturnButtonClicked);
        //Stage1_1Button.onClick.AddListener(Stage1_1ButtonClicked);

        for(int i = 0; i < StageButton.Length; i++)
        {
            int number = i;
            StageButton[i].onClick.AddListener(() => StageButtonClicked(number));
        }

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

        for (int i = 0; i < StagesButtons.Length; i++)
        {
            int number = i;
            StagesButtons[i].onClick.AddListener(() => StagesButtonsClicked(number));
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
                GameManager.instance.CharacterSelected[i] = null;

                //if (GameManager.instance.CharacterSelected[i].name == "Alice")
                //{
                //    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                //    //CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[0];
                //    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("Alice_Idle");
                //    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                //}
                //else if (GameManager.instance.CharacterSelected[i].name == "Gretel")
                //{
                //    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                //    //CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[1];
                //    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("Gretel_Idle");
                //    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                //}
                //else if (GameManager.instance.CharacterSelected[i].name == "Snow White")
                //{
                //    CharacterLocationButtons[i].transform.parent.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                //    //CharacterLocationButtons[i].transform.parent.GetComponent<Image>().sprite = CharactersIdle[2];
                //    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("SnowWhite_Idle");
                //    CharacterLocationButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
                //}
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

    //void Stage1ButtonClicked()
    //{
    //    Stage1.SetActive(false);

    //    StageSelectionPanel.SetActive(true);

    //    Background.GetComponent<Transform>().localScale = new Vector3(1.67f, 1, 1);
    //    Background.sprite = Stage1BG;

    //    ReturnButton.gameObject.SetActive(false);
    //}

    void StageButtonClicked(int number)
    {
        for (int i = 0; i < StageGameObject.Length; i++)
        {
            StageGameObject[i].SetActive(false);
        }

        for (int i = 0; i < StagesButtons.Length; i++)
        {
            StagesButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "스테이지 " + (number + 1) + " - " + (i + 1);
        }

        StagePanelTitle.text = "스테이지 " + (number + 1) + " - ";

        switch (number)
        {
            case 0:
                {
                    GameManager.instance.isStage1 = true;
                    GameManager.instance.isStage2 = false;
                    GameManager.instance.isStage3 = false;

                    StageSelectionPanel.SetActive(true);

                    Background.GetComponent<Transform>().localScale = new Vector3(1.67f, 1, 1);
                    Background.sprite = StageBG[number];

                    ReturnButton.gameObject.SetActive(false);

                    break;
                }

            case 1:
                {
                    GameManager.instance.isStage1 = false;
                    GameManager.instance.isStage2 = true;
                    GameManager.instance.isStage3 = false;

                    StageSelectionPanel.SetActive(true);

                    Background.GetComponent<Transform>().localScale = new Vector3(1.67f, 1, 1);
                    Background.sprite = StageBG[number];

                    ReturnButton.gameObject.SetActive(false);

                    break;
                }

            case 2:
                {
                    GameManager.instance.isStage1 = false;
                    GameManager.instance.isStage2 = false;
                    GameManager.instance.isStage3 = true;

                    StageSelectionPanel.SetActive(true);

                    Background.GetComponent<Transform>().localScale = new Vector3(1.67f, 1, 1);
                    Background.sprite = StageBG[number];

                    ReturnButton.gameObject.SetActive(false);

                    break;
                }
        }
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
        //Stage1.SetActive(true);

        for(int i = 0; i < StageGameObject.Length; i++)
        {
            StageGameObject[i].SetActive(true);
        }

        ReturnButton.gameObject.SetActive(true);
    }

    void StagePanelExitButtonClicked()
    {
        StagePanel.SetActive(false);
        CharacterSelectionScroll.SetActive(false);
        StageSelectionPanel.SetActive(true);
    }

    void CheckCharacterSelected()
    {
        for(int i = 0; i < GameManager.instance.CharacterSelected.Length; i++)
        {
            if (GameManager.instance.CharacterSelected[i] != null)
            {
                if (GameManager.instance.CharacterSelected[i].name == "Alice")
                {
                    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("Alice_Idle");
                }
                else if (GameManager.instance.CharacterSelected[i].name == "Gretel")
                {
                    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("Gretel_Idle");
                }
                else if (GameManager.instance.CharacterSelected[i].name == "Snow White")
                {
                    CharacterLocationButtons[i].transform.parent.GetComponent<Animator>().Play("SnowWhite_Idle");
                }
            }
            else
                continue;
        }
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

    void StagesButtonsClicked(int number)
    {
        StageSelectionPanel.SetActive(false);
        StagePanel.SetActive(true);        

        if(GameManager.instance.EnemySelected.Count != 0)
        {
            GameManager.instance.EnemySelected.Clear();
        }

        CheckCharacterSelected();
        
        StagePanelTitle.text = StagePanelTitle.text + (number + 1);

        switch (number)
        {
            case 0:
                {
                    EnemyObjects.SetActive(true);
                    BossImage.SetActive(false);

                    GameManager.instance.isBossStage = false;

                    if (GameManager.instance.isStage1)
                    {
                        for (int i = 0; i < EnemyImages.Length; i++)
                        {
                            EnemyImages[i].sprite = RatSprite;                            
                            EnemyImages[i].GetComponent<RectTransform>().anchoredPosition = FourEnemyPosition[i];
                            GameManager.instance.EnemySelected.Add(RatPrefab);
                        }

                        EnemyImages[3].gameObject.SetActive(true);
                    }
                    else if (GameManager.instance.isStage2)
                    {
                        EnemyImages[0].sprite = MagitekArmorSprite;
                        EnemyImages[1].sprite = MagitekArmorSprite;
                        EnemyImages[2].sprite = ScolopendraSprite;

                        GameManager.instance.EnemySelected.Add(MagitekArmorPrefab);
                        GameManager.instance.EnemySelected.Add(MagitekArmorPrefab);
                        GameManager.instance.EnemySelected.Add(ScolopendraPrefab);

                        for(int i = 0; i < 3; i++)
                        {
                            EnemyImages[i].GetComponent<RectTransform>().anchoredPosition = ThreeEnemyPosition[i];
                        }

                        EnemyImages[3].gameObject.SetActive(false);
                    }
                    else if (GameManager.instance.isStage3)
                    {
                        EnemyImages[0].sprite = VeritasSprite;
                        EnemyImages[1].sprite = SamathaSprite;
                        EnemyImages[2].sprite = SamathaSprite;

                        GameManager.instance.EnemySelected.Add(VeritasPrefab);
                        GameManager.instance.EnemySelected.Add(SamathaPrefab);
                        GameManager.instance.EnemySelected.Add(SamathaPrefab);

                        for (int i = 0; i < 3; i++)
                        {
                            EnemyImages[i].GetComponent<RectTransform>().anchoredPosition = ThreeEnemyPosition[i];
                        }

                        EnemyImages[3].gameObject.SetActive(false);
                    }

                    break;
                }

            case 1:
                EnemyObjects.SetActive(true);
                BossImage.SetActive(false);

                GameManager.instance.isBossStage = false;

                if (GameManager.instance.isStage1)
                {
                    for (int i = 0; i < EnemyImages.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            EnemyImages[i].sprite = WolfSprite;                            
                            GameManager.instance.EnemySelected.Add(WolfPrefab);
                        }
                        else
                        {
                            EnemyImages[i].sprite = RatSprite;                            
                            GameManager.instance.EnemySelected.Add(RatPrefab);
                        }

                        EnemyImages[i].GetComponent<RectTransform>().anchoredPosition = FourEnemyPosition[i];
                    }
                }
                else if (GameManager.instance.isStage2)
                {
                    EnemyImages[0].sprite = ScolopendraSprite;
                    EnemyImages[1].sprite = ScolopendraSprite;
                    EnemyImages[2].sprite = EpimetheusSprite;

                    GameManager.instance.EnemySelected.Add(ScolopendraPrefab);
                    GameManager.instance.EnemySelected.Add(ScolopendraPrefab);
                    GameManager.instance.EnemySelected.Add(EpimetheusPrefab);

                    for (int i = 0; i < 3; i++)
                    {
                        EnemyImages[i].GetComponent<RectTransform>().anchoredPosition = ThreeEnemyPosition[i];
                    }

                    EnemyImages[3].gameObject.SetActive(false);
                }
                else if (GameManager.instance.isStage3)
                {
                    EnemyImages[0].sprite = VeritasSprite;
                    EnemyImages[1].sprite = SamathaSprite;
                    EnemyImages[2].sprite = DarkVeritasSprite;                    

                    GameManager.instance.EnemySelected.Add(VeritasPrefab);
                    GameManager.instance.EnemySelected.Add(SamathaPrefab);
                    GameManager.instance.EnemySelected.Add(DarkVeritasPrefab);

                    for (int i = 0; i < 3; i++)
                    {
                        EnemyImages[i].GetComponent<RectTransform>().anchoredPosition = ThreeEnemyPosition[i];
                    }

                    EnemyImages[3].gameObject.SetActive(false);
                }

                break;

            case 2:
                {
                    EnemyObjects.SetActive(false);
                    BossImage.SetActive(true);

                    GameManager.instance.isBossStage = true;

                    if(GameManager.instance.isStage1)
                    {
                        BossImage.GetComponent<Animator>().Play("Lightning_Idle");
                        BossImage.GetComponent<RectTransform>().sizeDelta = new Vector3(350, 350);
                        GameManager.instance.EnemySelected.Add(LightningPrefab);
                    }
                    else if(GameManager.instance.isStage2)
                    {
                        BossImage.GetComponent<Animator>().Play("Sephiroth_Idle");
                        BossImage.GetComponent<RectTransform>().sizeDelta = new Vector3(350, 350);
                        GameManager.instance.EnemySelected.Add(SephirothPrefab);
                    }
                    else if(GameManager.instance.isStage3)
                    {                        
                        BossImage.GetComponent<Animator>().Play("Noctis_Idle");
                        BossImage.GetComponent<RectTransform>().sizeDelta = new Vector3(300, 300);
                        GameManager.instance.EnemySelected.Add(NoctisPrefab);
                    }

                    break;
                }
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
                GameManager.instance.LoadScene("Stage");
            }
        }
    }
}