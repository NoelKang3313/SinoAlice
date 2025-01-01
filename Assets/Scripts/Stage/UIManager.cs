using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Managers")]
    public StageManager StageManager;
    public StageAudioManager StageAudioManager;

    [Header("Battle Start")]
    public GameObject BattleIntro;
    public GameObject Transition;
    public Animator TransitionAnimator;

    [Header("Gauge Panel")]
    public GameObject GaugePanel;
    public Image CharacterImage;
    public TextMeshProUGUI CharacterName;
    public Image CharacterHPGauge;
    public Image CharacterMPGauge;
    public TextMeshProUGUI CurrentHPText;
    public TextMeshProUGUI CurrentMPText;

    [Header("Character Mini HP Gauge")]
    public GameObject CharacterMiniGauge;
    public Image Player1HPGauge;
    public Image Player2HPGauge;
    public Image Player3HPGauge;
    public Image[] EnemyHPGauge = new Image[4];
    public Image BossHPGauge;

    [Header("Character Sprites")]
    public Sprite AliceSprite;
    public Sprite GretelSprite;
    public Sprite SnowWhiteSprite;
    public Sprite RatSprite;
    public Sprite WolfSprite;
    public Sprite LightningSprite;

    [Header("Action Buttons")]
    public GameObject ActionButtons;
    private Animator actionButtonsAnimator;

    public Button AttackButton;
    public Button GuardButton;
    public Button SkillButton;
    public Button ItemButton;

    public GameObject EnemySelectButtons;
    public Button[] EnemySelectButton = new Button[4];
    public Button BossSelectButton;

    public Button[] CharacterSelectButton = new Button[3];

    public GameObject SkillButtonArrow;
    public GameObject SkillButtonViewport;
    public GameObject SkillViewportContent;

    public Button[] SkillButtons = new Button[5];
    public SkillData[] SkillInfo = new SkillData[15];
    [SerializeField]private SkillData[] CurrentSkillData = new SkillData[5];
    [SerializeField] private SkillData selectedSkillData;

    [Header("Skill Information Panel")]
    public GameObject SkillInformationPanel;
    public Image SkillIcon;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillDescription;
    public TextMeshProUGUI SkillCostMP;    

    public GameObject ItemButtonArrow;
    public GameObject ItemButtonViewport;
    public GameObject ItemViewportContent;

    [Header("Inventory Items")]
    [SerializeField]
    private Inventory Inventory;
    public Button ItemSlot;
    [SerializeField]
    private List<Button> ItemButtons = new List<Button>();
    [SerializeField]
    private int selectedItemID;
    [SerializeField]
    private ItemData SelectedItemData;

    public GameObject ItemInformationPanel;
    public Image ItemImage;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;

    [Header("Pause Panel")]
    public Button PauseButton;
    public GameObject PausePanel;
    public GameObject PauseObjects;
    public GameObject SelectionObjects;
    public GameObject SettingObjects;
    public Button RestartButton;
    public TextMeshProUGUI SelectionText;
    public Button OKButton;
    public Button CancelButton;
    public Button SettingButton;
    public Button ExitSettingButton;
    public Button ReturnLobbyButton;
    public Button ResumeButton;

    private bool isRestart;
    private bool isLobby;
    private bool isWorldmap;

    [Header("Battle Over Related")]
    public GameObject GameCompleteButtons;
    public Button GameCompleteReturnWorldmapButton;
    public Button GameCompleteReturnLobbyButton;

    private float aliceStartHP;
    private float aliceStartMP;
    private float gretelStartHP;
    private float gretelStartMP;
    private float swStartHP;
    private float swStartMP;

    void Start()
    {
        Inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

        actionButtonsAnimator = ActionButtons.GetComponent<Animator>();

        AttackButton.onClick.AddListener(AttackButtonClicked);
        GuardButton.onClick.AddListener(GuardButtonClicked);
        SkillButton.onClick.AddListener(SkillButtonClicked);
        ItemButton.onClick.AddListener(ItemButtonClicked);

        for(int i = 0; i < EnemySelectButton.Length; i++)
        {
            int number = i;
            EnemySelectButton[i].onClick.AddListener(() => EnemySelectButtonClicked(number));
        }

        BossSelectButton.onClick.AddListener(BossSelectButtonClicked);

        for(int i = 0; i < CharacterSelectButton.Length; i++)
        {
            int number = i;

            CharacterSelectButton[i].onClick.AddListener(() => OnCharacterSelectButtonClicked(number));
        }

        for (int i = 0; i < SkillButtons.Length; i++)
        {
            int number = i;            

            SkillButtons[i].onClick.AddListener(() => OnSkillButtonClicked(number));
        }

        PauseButton.onClick.AddListener(PauseButtonClicked);
        RestartButton.onClick.AddListener(RestartButtonClicked);
        OKButton.onClick.AddListener(ConfirmButtonClicked);
        CancelButton.onClick.AddListener(CancelButtonClicked);
        SettingButton.onClick.AddListener(SettingButtonClicked);
        ExitSettingButton.onClick.AddListener(ExitSettingButtonClicked);
        ReturnLobbyButton.onClick.AddListener(ReturnLobbyButtonClicked);
        ResumeButton.onClick.AddListener(ResumeButtonClicked);

        GameCompleteReturnWorldmapButton.onClick.AddListener(GameCompleteReturnWorldmapButtonClicked);
        GameCompleteReturnLobbyButton.onClick.AddListener(GameCompleteReturnLobbyButtonClicked);

        GetItemFromInventory();

        for(int i = 0; i < ItemButtons.Count; i++)
        {
            int number = i;

            ItemButtons[i].onClick.AddListener(() => ItemButtonsClicked(number));
        }

        aliceStartHP = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP;
        aliceStartMP = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP;
        gretelStartHP = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP;
        gretelStartMP = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP;
        swStartHP = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP;
        swStartMP = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP;
    }
    
    void Update()
    {
        BattleStart();
        SetGauge();
        SetMiniGauge();
        ActionButtonsActivate();
        CheckCharacterTurnChangeSkillIcon();
        StartCoroutine(ConfirmButtonPressed());
        CheckItemAmount();
        BattleOver();

        if(GameManager.instance.isEnemyTurn)
        {
            CharacterMiniGauge.SetActive(true);
        }
    }

    void CheckCharacterTurnChangeSkillIcon()
    {
        if (GameManager.instance.isAliceTurn)
        {
            ChangeSkillButtonSprites(0, 4);
        }
        else if (GameManager.instance.isGretelTurn)
        {
            ChangeSkillButtonSprites(5, 9);
        }
        else if (GameManager.instance.isSWTurn)
        {
            ChangeSkillButtonSprites(10, 14);
        }
    }

    void BattleStart()
    {
        if(!StageAudioManager.AudioSource.isPlaying && StageAudioManager.AudioSource.clip.name == "HeavenORHell")
        {
            StageAudioManager.AudioSource.Stop();
            StageAudioManager.AudioSource.clip = StageAudioManager.LetsRockClip;
            StageAudioManager.AudioSource.Play();
        }

        if(!StageAudioManager.AudioSource.isPlaying && StageAudioManager.AudioSource.clip.name == "Let's Rock")
        {
            StageAudioManager.AudioSource.Stop();

            if(!GameManager.instance.isBossStage)
            {
                StageAudioManager.AudioSource.clip = StageAudioManager.BattleBGM;
            }
            else
            {
                StageAudioManager.AudioSource.clip = StageAudioManager.LightningBGM;
            }

            StageAudioManager.AudioSource.Play();

            Transition.SetActive(false);
            PauseButton.gameObject.SetActive(true);

            StageAudioManager.AudioSource.loop = true;

            GaugePanel.SetActive(true);
            CharacterMiniGauge.SetActive(true);

            StartCoroutine(DelayBattleStart());
        }
    }

    IEnumerator DelayBattleStart()
    {
        yield return new WaitForSeconds(0.1f);

        GameManager.instance.isTurn = true;
    }

    void ActionButtonsActivate()
    {
        if(GameManager.instance.isAliceTurn || GameManager.instance.isGretelTurn || GameManager.instance.isSWTurn)
        {
            actionButtonsAnimator.SetBool("isActive", true);
        }
        else if(!GameManager.instance.isAliceTurn || !GameManager.instance.isGretelTurn || !GameManager.instance.isSWTurn)
        {
            actionButtonsAnimator.SetBool("isActive", false);
        }

        if(GameManager.instance.isAction)
        {
            actionButtonsAnimator.SetBool("isActive", false);
        }
    }

    void AttackButtonClicked()
    {
        GameManager.instance.isAttackButtonActive = true;
        GameManager.instance.isGuardButtonActive = false;
        GameManager.instance.isSkillButtonActive = false;
        GameManager.instance.isItemButtonActive = false;

        if(StageManager.EnemyInfo[0].name.StartsWith("Lightning"))
        {
            EnemySelectButtons.SetActive(false);
            BossSelectButton.gameObject.SetActive(true);
        }
        else
        {
            EnemySelectButtons.SetActive(true);
            BossSelectButton.gameObject.SetActive(false);
        }

        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        ItemButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        ItemButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        for(int i = 0; i < CharacterSelectButton.Length; i++)
        {
            CharacterSelectButton[i].gameObject.SetActive(false);
        }

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);
        ItemInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

        ResetViewportPosition();
    }

    void GuardButtonClicked()
    {
        GameManager.instance.isAttackButtonActive = false;
        GameManager.instance.isGuardButtonActive = true;
        GameManager.instance.isSkillButtonActive = false;
        GameManager.instance.isItemButtonActive = false;

        if(GameManager.instance.isAliceTurn)
        {
            CharacterSelectButton[GameManager.instance.AlicePositionNumber].gameObject.SetActive(true);
            CharacterSelectButton[GameManager.instance.GretelPositionNumber].gameObject.SetActive(false);
            CharacterSelectButton[GameManager.instance.SWPositionNumber].gameObject.SetActive(false);
        }
        else if(GameManager.instance.isGretelTurn)
        {
            CharacterSelectButton[GameManager.instance.AlicePositionNumber].gameObject.SetActive(false);
            CharacterSelectButton[GameManager.instance.GretelPositionNumber].gameObject.SetActive(true);
            CharacterSelectButton[GameManager.instance.SWPositionNumber].gameObject.SetActive(false);
        }
        else if(GameManager.instance.isSWTurn)
        {
            CharacterSelectButton[GameManager.instance.AlicePositionNumber].gameObject.SetActive(false);
            CharacterSelectButton[GameManager.instance.GretelPositionNumber].gameObject.SetActive(false);
            CharacterSelectButton[GameManager.instance.SWPositionNumber].gameObject.SetActive(true);
        }

        EnemySelectButtons.SetActive(false);

        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

        ItemButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        ItemButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        ItemInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

        ResetViewportPosition();
    }

    void SkillButtonClicked()
    {
        GameManager.instance.isAttackButtonActive = false;
        GameManager.instance.isGuardButtonActive = false;
        GameManager.instance.isSkillButtonActive = true;
        GameManager.instance.isItemButtonActive = false;

        EnemySelectButtons.SetActive(false);
        BossSelectButton.gameObject.SetActive(false);

        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", true);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", true);

        ItemButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        ItemButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        ItemInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

        for (int i = 0; i < CharacterSelectButton.Length; i++)
        {
            CharacterSelectButton[i].gameObject.SetActive(false);
        }

        ResetViewportPosition();
    }

    void OnSkillButtonClicked(int number)
    {
        GameManager.instance.SkillButtonNumber = number;

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", true);

        selectedSkillData = CurrentSkillData[number];

        SkillIcon.sprite = CurrentSkillData[number].SkillIcon;
        SkillName.text = CurrentSkillData[number].SkillName;
        SkillDescription.text = CurrentSkillData[number].SkillDescription;
        SkillCostMP.text = "MP : " + CurrentSkillData[number].CostMP.ToString();        

        if (number == 4)
        {
            if(GameManager.instance.isAliceTurn)
            {
                EnemySelectButtons.SetActive(false);
                BossSelectButton.gameObject.SetActive(false);

                for (int i = 0; i < CharacterSelectButton.Length; i++)
                {
                    CharacterSelectButton[i].gameObject.SetActive(true);
                }
            }
            else if (GameManager.instance.isGretelTurn || GameManager.instance.isSWTurn)
            {
                if (StageManager.EnemyInfo[0].name.StartsWith("Lightning"))
                {
                    EnemySelectButtons.SetActive(false);
                    BossSelectButton.gameObject.SetActive(true);
                }
                else
                {
                    EnemySelectButtons.SetActive(true);
                    BossSelectButton.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (StageManager.EnemyInfo[0].name.StartsWith("Lightning"))
            {
                EnemySelectButtons.SetActive(false);
                BossSelectButton.gameObject.SetActive(true);
            }
            else
            {
                EnemySelectButtons.SetActive(true);
                BossSelectButton.gameObject.SetActive(false);
            }

            for (int i = 0; i < CharacterSelectButton.Length; i++)
            {
                CharacterSelectButton[i].gameObject.SetActive(false);
            }
        }
    }

    void ChangeSkillButtonSprites(int firstIndex, int lastIndex)
    {
        int index = 0;

        for (int i = firstIndex; i <= lastIndex; i++)
        {
            CurrentSkillData[index] = SkillInfo[i];
            SkillButtons[index].GetComponent<Image>().sprite = SkillInfo[i].SkillIcon;
            index++;
        }
    }

    void ItemButtonClicked()
    {
        GameManager.instance.isAttackButtonActive = false;
        GameManager.instance.isGuardButtonActive = false;
        GameManager.instance.isSkillButtonActive = false;
        GameManager.instance.isItemButtonActive = true;

        EnemySelectButtons.SetActive(false);
        BossSelectButton.gameObject.SetActive(false);

        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        ItemButtonArrow.GetComponent<Animator>().SetBool("isActive", true);
        ItemButtonViewport.GetComponent<Animator>().SetBool("isActive", true);

        for (int i = 0; i < CharacterSelectButton.Length; i++)
        {
            CharacterSelectButton[i].gameObject.SetActive(false);
        }

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

        ResetViewportPosition();
    }

    void GetItemFromInventory()
    {
        for (int i = 0; i < Inventory.Items.Count; i++)
        {
            if (Inventory.Items.Count != 0)
            {
                Button Item = Instantiate(ItemSlot, ItemViewportContent.transform.position, Quaternion.identity);
                Item.transform.SetParent(ItemViewportContent.transform);                

                Item.GetComponent<UIItem>().ItemImage.sprite = Inventory.Items[i].ItemSprite;
                Item.GetComponent<UIItem>().ItemAmount.text = Inventory.Items[i].ItemAmount.ToString();
                Item.GetComponent<UIItem>().ItemData = Inventory.Items[i];

                ItemButtons.Add(Item);
            }
        }

        SortItemButtons();
    }

    void SortItemButtons()
    {
        for (int i = 0; i < ItemButtons.Count - 1; i++)
        {
            for (int j = 0; j < (ItemButtons.Count - i) - 1; j++)
            {
                if (ItemButtons[j].GetComponent<UIItem>().ItemData.ItemID < ItemButtons[j + 1].GetComponent<UIItem>().ItemData.ItemID)
                {
                    ItemButtons[j].transform.SetAsLastSibling();
                }
            }
        }
    }

    void CheckItemAmount()
    {
        for(int i = 0; i < ItemButtons.Count; i++)
        {
            if(ItemButtons[i].GetComponent<UIItem>().ItemData.ItemAmount == 0)
            {
                Destroy(ItemButtons[i].gameObject);
                ItemButtons.Remove(ItemButtons[i]);
            }
            else if(ItemButtons[i].GetComponent<UIItem>().ItemData.ItemAmount > 0)
            {
                ItemButtons[i].GetComponent<UIItem>().ItemAmount.text = ItemButtons[i].GetComponent<UIItem>().ItemData.ItemAmount.ToString();
            }
        }
    }

    void ItemButtonsClicked(int number)
    {        
        selectedItemID = ItemButtons[number].GetComponent<UIItem>().ItemData.ItemID;
        SelectedItemData = ItemButtons[number].GetComponent<UIItem>().ItemData;

        for (int i = 0; i < CharacterSelectButton.Length; i++)
        {
            CharacterSelectButton[i].gameObject.SetActive(true);
        }
        
        ItemInformationPanel.GetComponent<Animator>().SetBool("isActive", true);

        ItemImage.sprite = ItemButtons[number].GetComponent<UIItem>().ItemData.ItemSprite;
        ItemName.text = ItemButtons[number].GetComponent<UIItem>().ItemData.ItemName;
        ItemDescription.text = ItemButtons[number].GetComponent<UIItem>().ItemData.ItemDescription;
    }    

    void EnemySelectButtonClicked(int number)
    {
        GameManager.instance.EnemyPositionNumber = number;
        GameManager.instance.isAction = true;
        EnemySelectButtons.SetActive(false);

        if(selectedSkillData != null)
        {
            SkillUsedReduceMP(selectedSkillData);
        }

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);

        ResetViewportPosition();
    }

    void BossSelectButtonClicked()
    {
        GameManager.instance.isAction = true;
        BossSelectButton.gameObject.SetActive(false);

        if (selectedSkillData != null)
        {
            SkillUsedReduceMP(selectedSkillData);
        }

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);

        ResetViewportPosition();
    }

    void OnCharacterSelectButtonClicked(int number)
    {
        if (GameManager.instance.AlicePositionNumber == number)
        {
            GameManager.instance.SelectedCharacterPosition = GameManager.instance.CharacterSelected[number].transform.position;

            if (GameManager.instance.isGuardButtonActive)
            {
                GameManager.instance.isAction = true;
                CharacterSelectButton[number].gameObject.SetActive(false);
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                GameManager.instance.isAction = true;

                for (int i = 0; i < CharacterSelectButton.Length; i++)
                {
                    CharacterSelectButton[i].gameObject.SetActive(false);
                }
            }
            else if(GameManager.instance.isItemButtonActive)
            {
                GameManager.instance.isAction = true;

                for (int i = 0; i < CharacterSelectButton.Length; i++)
                {
                    CharacterSelectButton[i].gameObject.SetActive(false);
                }

                UseItem(selectedItemID, number);

                SelectedItemData.ItemAmount--;
            }
        }
        else if (GameManager.instance.GretelPositionNumber == number)
        {
            GameManager.instance.SelectedCharacterPosition = GameManager.instance.CharacterSelected[number].transform.position;

            if (GameManager.instance.isGuardButtonActive)
            {
                GameManager.instance.isAction = true;
                CharacterSelectButton[number].gameObject.SetActive(false);
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                GameManager.instance.isAction = true;

                for (int i = 0; i < CharacterSelectButton.Length; i++)
                {
                    CharacterSelectButton[i].gameObject.SetActive(false);
                }
            }
            else if (GameManager.instance.isItemButtonActive)
            {
                GameManager.instance.isAction = true;

                for (int i = 0; i < CharacterSelectButton.Length; i++)
                {
                    CharacterSelectButton[i].gameObject.SetActive(false);
                }

                UseItem(selectedItemID, number);

                SelectedItemData.ItemAmount--;
            }
        }
        else if (GameManager.instance.SWPositionNumber == number)
        {
            GameManager.instance.SelectedCharacterPosition = GameManager.instance.CharacterSelected[number].transform.position;

            if(GameManager.instance.isGuardButtonActive)
            {
                GameManager.instance.isAction = true;
                CharacterSelectButton[number].gameObject.SetActive(false);
            }
            else if (GameManager.instance.isSkillButtonActive)
            {
                GameManager.instance.isAction = true;                

                for (int i = 0; i < CharacterSelectButton.Length; i++)
                {
                    CharacterSelectButton[i].gameObject.SetActive(false);
                }                
            }
            else if (GameManager.instance.isItemButtonActive)
            {
                GameManager.instance.isAction = true;

                for (int i = 0; i < CharacterSelectButton.Length; i++)
                {
                    CharacterSelectButton[i].gameObject.SetActive(false);
                }

                UseItem(selectedItemID, number);

                SelectedItemData.ItemAmount--;
            }
        }

        actionButtonsAnimator.SetBool("isActive", false);

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);

        ItemButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        ItemButtonViewport.GetComponent<Animator>().SetBool("isActive", false);
        ItemInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

        ResetViewportPosition();
    }

    void SkillUsedReduceMP(SkillData selectedSkillData)
    {
        if (selectedSkillData.SkillUserName == "Alice")
        {
            GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP -= selectedSkillData.CostMP;
        }
        else if (selectedSkillData.SkillUserName == "Gretel")
        {
            GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP -= selectedSkillData.CostMP;
        }
        else if (selectedSkillData.SkillUserName == "Snow White")
        {
            GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP -= selectedSkillData.CostMP;
        }

        //if(selectedSkillData.SkillUserName == "Alice")
        //{
        //    GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentMP -= selectedSkillData.CostMP;
        //}
        //else if(selectedSkillData.SkillUserName == "Gretel")
        //{
        //    GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentMP -= selectedSkillData.CostMP;
        //}
        //else if(selectedSkillData.SkillUserName == "Snow White")
        //{
        //    GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentMP -= selectedSkillData.CostMP;
        //}
    }

    void UseItem(int id, int characterNumber)
    {        
        switch(id)
        {
            case 1:
                if(characterNumber == GameManager.instance.AlicePositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP * 0.2f);
                    GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP += heal;
                }
                else if (characterNumber == GameManager.instance.GretelPositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP * 0.2f);
                    GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP += heal;
                }
                else if(characterNumber == GameManager.instance.SWPositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP * 0.2f);
                    GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP += heal;
                }
                break;

            case 2:
                if (characterNumber == GameManager.instance.AlicePositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP * 0.2f);
                    GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP += heal;
                }
                else if (characterNumber == GameManager.instance.GretelPositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP * 0.2f);
                    GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP += heal;
                }
                else if (characterNumber == GameManager.instance.SWPositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP * 0.2f);
                    GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP += heal;
                }
                break;

            case 3:
                if (characterNumber == GameManager.instance.AlicePositionNumber)
                {
                    int healHP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP * 0.15f);
                    GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP += healHP;

                    int healMP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP * 0.15f);
                    GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP += healMP;
                }
                else if (characterNumber == GameManager.instance.GretelPositionNumber)
                {
                    int healHP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP * 0.15f);
                    GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP += healHP;

                    int healMP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP * 0.15f);
                    GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP += healMP;
                }
                else if (characterNumber == GameManager.instance.SWPositionNumber)
                {
                    int healHP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP * 0.15f);
                    GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP += healHP;

                    int healMP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP * 0.15f);
                    GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP += healMP;
                }
                break;

            case 4:
                if (characterNumber == GameManager.instance.AlicePositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP * 0.4f);
                    GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP += heal;
                }
                else if (characterNumber == GameManager.instance.GretelPositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP * 0.4f);
                    GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP += heal;
                }
                else if (characterNumber == GameManager.instance.SWPositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP * 0.4f);
                    GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP += heal;
                }
                break;

            case 5:
                if (characterNumber == GameManager.instance.AlicePositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP * 0.4f);
                    GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP += heal;
                }
                else if (characterNumber == GameManager.instance.GretelPositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP * 0.4f);
                    GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP += heal;
                }
                else if (characterNumber == GameManager.instance.SWPositionNumber)
                {
                    int heal = (int)(GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP * 0.4f);
                    GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP += heal;
                }
                break;

            case 6:
                if (characterNumber == GameManager.instance.AlicePositionNumber)
                {
                    int healHP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP * 0.35f);
                    GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP += healHP;

                    int healMP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP * 0.35f);
                    GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP += healMP;
                }
                else if (characterNumber == GameManager.instance.GretelPositionNumber)
                {
                    int healHP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP * 0.35f);
                    GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP += healHP;

                    int healMP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP * 0.35f);
                    GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP += healMP;
                }
                else if (characterNumber == GameManager.instance.SWPositionNumber)
                {
                    int healHP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP * 0.35f);
                    GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP += healHP;

                    int healMP = (int)(GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP * 0.35f);
                    GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP += healMP;
                }
                break;
        }        
    }

    void PauseButtonClicked()
    {
        PausePanel.SetActive(true);
        PauseButton.gameObject.SetActive(false);       
    }

    void RestartButtonClicked()
    {
        isRestart = true;
        PauseObjects.SetActive(false);
        SelectionObjects.SetActive(true);

        SelectionText.text = "???? ?????????????????";
    }

    void ConfirmButtonClicked()
    {
        Transition.SetActive(true);
        TransitionAnimator.SetBool("isTransition", true);
    }

    void CancelButtonClicked()
    {
        PauseObjects.SetActive(true);
        SelectionObjects.SetActive(false);

        if(isRestart)
        {
            isRestart = false;
        }
        else if(isLobby)
        {
            isLobby = false;
        }
    }

    void SettingButtonClicked()
    {
        PauseObjects.SetActive(false);
        SettingObjects.SetActive(true);
    }

    void ExitSettingButtonClicked()
    {
        PauseObjects.SetActive(true);
        SettingObjects.SetActive(false);
    }

    void ReturnLobbyButtonClicked()
    {
        isLobby = true;
        SelectionObjects.SetActive(true);
        PauseObjects.SetActive(false);

        SelectionText.text = "?????? ?????????????????";
    }

    void ResumeButtonClicked()
    {
        PausePanel.SetActive(false);
        PauseButton.gameObject.SetActive(true);        
    }

    void ResetViewportPosition()
    {
        if(SkillButtonViewport.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Viewport Idle") ||
            SkillButtonViewport.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("-Viewport"))
        {
            SkillViewportContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else if (ItemButtonViewport.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Viewport Idle") ||
            ItemButtonViewport.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("-Viewport"))
        {
            ItemViewportContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    void SetMiniGauge()
    {
        for(int i = 0; i < GameManager.instance.Characters.Length; i++)
        {
            switch(i)
            {
                case 0:
                    Player1HPGauge.fillAmount = GetPlayerHP(i);
                    break;
                case 1:
                    Player2HPGauge.fillAmount = GetPlayerHP(i);
                    break;
                case 2:
                    Player3HPGauge.fillAmount = GetPlayerHP(i);
                    break;
            }
        }


        for(int i = 0; i < StageManager.EnemyInfo.Count; i++)
        {
            if(StageManager.EnemyInfo[i] != null && StageManager.EnemyInfo[i].name.StartsWith("Lightning"))
            {
                BossHPGauge.fillAmount = StageManager.EnemyInfo[i].GetComponent<Lightning>().CurrentHP / StageManager.EnemyInfo[i].GetComponent<Lightning>().HP;

                for(int j = 0; j < EnemyHPGauge.Length; j++)
                {
                    EnemyHPGauge[j].gameObject.SetActive(false);
                }
            }
            else
            {
                BossHPGauge.gameObject.SetActive(false);

                if (StageManager.EnemyInfo[i] == null)
                {
                    EnemyHPGauge[i].gameObject.SetActive(false);
                    EnemySelectButton[i].gameObject.SetActive(false);
                }
                else
                {
                    if (StageManager.EnemyInfo[i].name.StartsWith("Rat"))
                    {
                        EnemyHPGauge[i].fillAmount = StageManager.EnemyInfo[i].GetComponent<Rat>().CurrentHP / StageManager.EnemyInfo[i].GetComponent<Rat>().HP;
                    }
                    else if (StageManager.EnemyInfo[i].name.StartsWith("Wolf"))
                    {
                        EnemyHPGauge[i].fillAmount = StageManager.EnemyInfo[i].GetComponent<Wolf>().CurrentHP / StageManager.EnemyInfo[i].GetComponent<Wolf>().HP;
                    }
                }
            }
        }

        //for(int i = 0; i < EnemyHPGauge.Length; i++)
        //{
        //    if(StageManager.EnemyInfo[i] == null)
        //    {
        //        EnemyHPGauge[i].gameObject.SetActive(false);
        //        EnemySelectButton[i].gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        if (StageManager.EnemyInfo[i].name.StartsWith("Rat"))
        //        {
        //            EnemyHPGauge[i].fillAmount = StageManager.EnemyInfo[i].GetComponent<Rat>().CurrentHP / StageManager.EnemyInfo[i].GetComponent<Rat>().HP;
        //        }
        //        else if(StageManager.EnemyInfo[i].name.StartsWith("Wolf"))
        //        {
        //            EnemyHPGauge[i].fillAmount = StageManager.EnemyInfo[i].GetComponent<Wolf>().CurrentHP / StageManager.EnemyInfo[i].GetComponent<Wolf>().HP;
        //        }
        //        else if (StageManager.EnemyInfo[i].name.StartsWith("Lightning"))
        //        {
        //            EnemyHPGauge[i].fillAmount = StageManager.EnemyInfo[i].GetComponent<Lightning>().CurrentHP / StageManager.EnemyInfo[i].GetComponent<Lightning>().HP;
        //        }
        //    }
        //}        
    }    

    float GetPlayerHP(int number)
    {
        if (GameManager.instance.CharacterSelected[number].name.StartsWith("Alice"))
        {
            return GameManager.instance.CharacterSelected[number].GetComponent<Alice>().CurrentHP / GameManager.instance.CharacterSelected[number].GetComponent<Alice>().HP;
        }
        else if (GameManager.instance.CharacterSelected[number].name.StartsWith("Gretel"))
        {
            return GameManager.instance.CharacterSelected[number].GetComponent<Gretel>().CurrentHP / GameManager.instance.CharacterSelected[number].GetComponent<Gretel>().HP;
        }
        else if (GameManager.instance.CharacterSelected[number].name.StartsWith("Snow White"))
        {
            return GameManager.instance.CharacterSelected[number].GetComponent<SnowWhite>().CurrentHP / GameManager.instance.CharacterSelected[number].GetComponent<SnowWhite>().HP;
        }

        return 0;
    }

    void SetGauge()
    {
        if (GameManager.instance.isAliceTurn)
        {
            if (GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP > GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP)
            {
                GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP;
            }

            if (GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP > GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP)
            {
                GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP;
            }

            CharacterImage.sprite = AliceSprite;
            CharacterName.text = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().Name;
            CharacterHPGauge.fillAmount = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP / GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP;
            CharacterMPGauge.fillAmount = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP / GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP;
            CurrentHPText.text = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP.ToString() + "/" + GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP.ToString();
            CurrentMPText.text = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP.ToString() + "/" + GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP.ToString();
        }
        else if (GameManager.instance.isGretelTurn)
        {
            if (GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP > GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP)
            {
                GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP;
            }

            if (GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP > GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP)
            {
                GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP;
            }

            CharacterImage.sprite = GretelSprite;
            CharacterName.text = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().Name;
            CharacterHPGauge.fillAmount = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP / GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP;
            CharacterMPGauge.fillAmount = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP / GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP;
            CurrentHPText.text = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP.ToString() + "/" + GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP.ToString();
            CurrentMPText.text = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP.ToString() + "/" + GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP.ToString();
        }
        else if (GameManager.instance.isSWTurn)
        {
            if (GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP > GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP)
            {
                GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP;
            }

            if (GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP > GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP)
            {
                GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP;
            }

            CharacterImage.sprite = SnowWhiteSprite;
            CharacterName.text = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().Name;
            CharacterHPGauge.fillAmount = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP / GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP;
            CharacterMPGauge.fillAmount = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP / GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP;
            CurrentHPText.text = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP.ToString() + "/" + GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP.ToString();
            CurrentMPText.text = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP.ToString() + "/" + GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP.ToString();
        }
        

        //if(GameManager.instance.isAliceTurn)
        //{
        //    if(GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentHP > GameManager.instance.AlicePrefab.GetComponent<Alice>().HP)
        //    {
        //        GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentHP = GameManager.instance.AlicePrefab.GetComponent<Alice>().HP;
        //    }

        //    if (GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentMP > GameManager.instance.AlicePrefab.GetComponent<Alice>().MP)
        //    {
        //        GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentMP = GameManager.instance.AlicePrefab.GetComponent<Alice>().MP;
        //    }

        //    CharacterImage.sprite = AliceSprite;
        //    CharacterName.text = GameManager.instance.AlicePrefab.GetComponent<Alice>().Name;
        //    CharacterHPGauge.fillAmount = GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentHP / GameManager.instance.AlicePrefab.GetComponent<Alice>().HP;
        //    CharacterMPGauge.fillAmount = GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentMP / GameManager.instance.AlicePrefab.GetComponent<Alice>().MP;
        //    CurrentHPText.text = GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentHP.ToString() + "/" + GameManager.instance.AlicePrefab.GetComponent<Alice>().HP.ToString();
        //    CurrentMPText.text = GameManager.instance.AlicePrefab.GetComponent<Alice>().CurrentMP.ToString() + "/" + GameManager.instance.AlicePrefab.GetComponent<Alice>().MP.ToString();
        //}
        //else if(GameManager.instance.isGretelTurn)
        //{
        //    if (GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentHP > GameManager.instance.GretelPrefab.GetComponent<Gretel>().HP)
        //    {
        //        GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentHP = GameManager.instance.GretelPrefab.GetComponent<Gretel>().HP;
        //    }

        //    if (GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentMP > GameManager.instance.GretelPrefab.GetComponent<Gretel>().MP)
        //    {
        //        GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentMP = GameManager.instance.GretelPrefab.GetComponent<Gretel>().MP;
        //    }

        //    CharacterImage.sprite = GretelSprite;
        //    CharacterName.text = GameManager.instance.GretelPrefab.GetComponent<Gretel>().Name;
        //    CharacterHPGauge.fillAmount = GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentHP / GameManager.instance.GretelPrefab.GetComponent<Gretel>().HP;
        //    CharacterMPGauge.fillAmount = GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentMP / GameManager.instance.GretelPrefab.GetComponent<Gretel>().MP;
        //    CurrentHPText.text = GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentHP.ToString() + "/" + GameManager.instance.GretelPrefab.GetComponent<Gretel>().HP.ToString();
        //    CurrentMPText.text = GameManager.instance.GretelPrefab.GetComponent<Gretel>().CurrentMP.ToString() + "/" + GameManager.instance.GretelPrefab.GetComponent<Gretel>().MP.ToString();
        //}
        //else if(GameManager.instance.isSWTurn)
        //{
        //    if (GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentHP > GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().HP)
        //    {
        //        GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentHP = GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().HP;
        //    }

        //    if (GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentMP > GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().MP)
        //    {
        //        GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentMP = GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().MP;
        //    }

        //    CharacterImage.sprite = SnowWhiteSprite;
        //    CharacterName.text = GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().Name;
        //    CharacterHPGauge.fillAmount = GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentHP / GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().HP;
        //    CharacterMPGauge.fillAmount = GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentMP / GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().MP;
        //    CurrentHPText.text = GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentHP.ToString() + "/" + GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().HP.ToString();
        //    CurrentMPText.text = GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().CurrentMP.ToString() + "/" + GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().MP.ToString();
        //}
        if (GameManager.instance.isEnemyTurn)
        {
            //// Array
            //CharacterImage.sprite = RatSprite;
            //CharacterName.text = StageManager.CharacterTurn[GameManager.instance.TurnNumber].name;
            //CharacterHPGauge.fillAmount = StageManager.CharacterTurn[GameManager.instance.TurnNumber].GetComponent<Rat>().CurrentHP / StageManager.CharacterTurn[GameManager.instance.TurnNumber].GetComponent<Rat>().HP;
            //CharacterMPGauge.fillAmount = StageManager.CharacterTurn[GameManager.instance.TurnNumber].GetComponent<Rat>().CurrentMP / StageManager.CharacterTurn[GameManager.instance.TurnNumber].GetComponent<Rat>().MP;
            //CurrentHPText.text = StageManager.CharacterTurn[GameManager.instance.TurnNumber].GetComponent<Rat>().CurrentHP.ToString() + "/" + StageManager.CharacterTurn[GameManager.instance.TurnNumber].GetComponent<Rat>().HP.ToString();
            //CurrentMPText.text = StageManager.CharacterTurn[GameManager.instance.TurnNumber].GetComponent<Rat>().CurrentMP.ToString() + "/" + StageManager.CharacterTurn[GameManager.instance.TurnNumber].GetComponent<Rat>().MP.ToString();

            if(StageManager.CharacterTurns[GameManager.instance.TurnNumber].name.StartsWith("Rat"))
            {
                // List
                CharacterImage.sprite = RatSprite;
                CharacterName.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].name;
                CharacterHPGauge.fillAmount = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Rat>().CurrentHP / StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Rat>().HP;
                CharacterMPGauge.fillAmount = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Rat>().CurrentMP / StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Rat>().MP;
                CurrentHPText.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Rat>().CurrentHP.ToString() + "/" + StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Rat>().HP.ToString();
                CurrentMPText.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Rat>().CurrentMP.ToString() + "/" + StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Rat>().MP.ToString();
            }
            else if(StageManager.CharacterTurns[GameManager.instance.TurnNumber].name.StartsWith("Wolf"))
            {
                CharacterImage.sprite = WolfSprite;
                CharacterName.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].name;
                CharacterHPGauge.fillAmount = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Wolf>().CurrentHP / StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Wolf>().HP;
                CharacterMPGauge.fillAmount = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Wolf>().CurrentMP / StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Wolf>().MP;
                CurrentHPText.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Wolf>().CurrentHP.ToString() + "/" + StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Wolf>().HP.ToString();
                CurrentMPText.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Wolf>().CurrentMP.ToString() + "/" + StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Wolf>().MP.ToString();
            }
            else if (StageManager.CharacterTurns[GameManager.instance.TurnNumber].name.StartsWith("Lightning"))
            {
                CharacterImage.sprite = LightningSprite;
                CharacterName.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].name;
                CharacterHPGauge.fillAmount = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Lightning>().CurrentHP / StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Lightning>().HP;
                CharacterMPGauge.fillAmount = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Lightning>().CurrentMP / StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Lightning>().MP;
                CurrentHPText.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Lightning>().CurrentHP.ToString() + "/" + StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Lightning>().HP.ToString();
                CurrentMPText.text = StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Lightning>().CurrentMP.ToString() + "/" + StageManager.CharacterTurns[GameManager.instance.TurnNumber].GetComponent<Lightning>().MP.ToString();
            }
        }
    }

    void BattleOver()
    {
        if(GameManager.instance.isBattleOver)
        {
            ActionButtons.SetActive(false);
            PauseButton.gameObject.SetActive(false);
            GaugePanel.SetActive(false);
            GameCompleteButtons.SetActive(true);

            if(StageAudioManager.AudioSource.clip.name == "Battle BGM")
            {
                StageAudioManager.AudioSource.Stop();
                StageAudioManager.AudioSource.clip = StageAudioManager.VictoryBGM;
                StageAudioManager.AudioSource.Play();
            }
        }
    }

    void GameCompleteReturnWorldmapButtonClicked()
    {
        isWorldmap = true;

        Transition.SetActive(true);
        TransitionAnimator.SetBool("isTransition", true);
    }

    void GameCompleteReturnLobbyButtonClicked()
    {
        isLobby = true;

        Transition.SetActive(true);
        TransitionAnimator.SetBool("isTransition", true);
    }

    IEnumerator ConfirmButtonPressed()
    {
        if (TransitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("-Stage Transition") &&
            TransitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            yield return new WaitForSeconds(2.0f);

            if(isRestart)
            {
                isRestart = false;

                for(int i = 0; i < Inventory.ItemAmount.Count; i++)
                {
                    Inventory.Items[i].ItemAmount = Inventory.ItemAmount[i];
                }

                GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().HP;
                GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP = GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().MP;

                GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().HP;
                GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP = GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().MP;

                GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().HP;
                GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP = GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().MP;

                GameManager.instance.isAliceTurn = false;
                GameManager.instance.isGretelTurn = false;
                GameManager.instance.isSWTurn = false;

                GameManager.instance.isAttackButtonActive = false;
                GameManager.instance.isGuardButtonActive = false;
                GameManager.instance.isSkillButtonActive = false;
                GameManager.instance.isItemButtonActive = false;

                GameManager.instance.isBattleStart = true;
                GameManager.instance.TurnNumber = 0;
                GameManager.instance.isBattleOver = false;

                GameManager.instance.LoadScene("Stage1");                
            }
            else if(isLobby)
            {
                isLobby = false;

                if(GameManager.instance.isBattleOver)
                {
                    if(ItemButtons.Count != 0)
                    {
                        Inventory.Items.Clear();
                        Inventory.ItemAmount.Clear();

                        for(int i = 0; i < ItemButtons.Count; i++)
                        {
                            Inventory.Items.Add(ItemButtons[i].GetComponent<UIItem>().ItemData);
                            Inventory.ItemAmount.Add(ItemButtons[i].GetComponent<UIItem>().ItemData.ItemAmount);
                        }
                    }
                }
                else
                {
                    if(aliceStartHP != GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP ||
                        aliceStartMP != GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP ||
                        gretelStartHP != GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP ||
                        gretelStartMP != GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP ||
                        swStartHP != GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP ||
                        swStartMP != GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP)
                    {
                        GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentHP = aliceStartHP;
                        GameManager.instance.CharacterSelected[GameManager.instance.AlicePositionNumber].GetComponent<Alice>().CurrentMP = aliceStartMP;

                        GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentHP = gretelStartHP;
                        GameManager.instance.CharacterSelected[GameManager.instance.GretelPositionNumber].GetComponent<Gretel>().CurrentMP = gretelStartMP;

                        GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentHP = swStartHP;
                        GameManager.instance.CharacterSelected[GameManager.instance.SWPositionNumber].GetComponent<SnowWhite>().CurrentMP = swStartMP;
                    }
                }

                GameManager.instance.isAliceTurn = false;
                GameManager.instance.isGretelTurn = false;
                GameManager.instance.isSWTurn = false;

                GameManager.instance.isAttackButtonActive = false;
                GameManager.instance.isGuardButtonActive = false;
                GameManager.instance.isSkillButtonActive = false;
                GameManager.instance.isItemButtonActive = false;

                GameManager.instance.isBattleStart = false;
                GameManager.instance.TurnNumber = 0;
                GameManager.instance.isBattleOver = false;
                GameManager.instance.LoadScene("Lobby");
            }
            else if(isWorldmap)
            {
                isWorldmap = false;

                if (GameManager.instance.isBattleOver)
                {
                    if (ItemButtons.Count != 0)
                    {
                        Inventory.Items.Clear();
                        Inventory.ItemAmount.Clear();

                        for (int i = 0; i < ItemButtons.Count; i++)
                        {
                            Inventory.Items.Add(ItemButtons[i].GetComponent<UIItem>().ItemData);
                            Inventory.ItemAmount.Add(ItemButtons[i].GetComponent<UIItem>().ItemData.ItemAmount);
                        }
                    }
                }

                GameManager.instance.isAliceTurn = false;
                GameManager.instance.isGretelTurn = false;
                GameManager.instance.isSWTurn = false;

                GameManager.instance.isAttackButtonActive = false;
                GameManager.instance.isGuardButtonActive = false;
                GameManager.instance.isSkillButtonActive = false;
                GameManager.instance.isItemButtonActive = false;

                GameManager.instance.isBattleStart = false;
                GameManager.instance.TurnNumber = 0;
                GameManager.instance.isBattleOver = false;
                GameManager.instance.LoadScene("Worldmap");
            }
        }
    }
}
