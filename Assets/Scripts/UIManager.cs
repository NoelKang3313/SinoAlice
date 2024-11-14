using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject HeavenOrHell;
    private Animator heavenOrHellAnimator;
    public GameObject LetsRock;
    private Animator letsRockAnimator;

    public GameObject Transition;
    public Animator TransitionAnimator;

    public AudioSource AudioSource;
    public AudioClip LetsRockAudioClip;
    public AudioClip BattleBGM;

    public GameObject ActionButtons;
    private Animator actionButtonsAnimator;

    public Button AttackButton;
    public Button GuardButton;
    public Button SkillButton;
    public Button ItemButton;

    public GameObject EnemySelectButtons;
    public Button[] EnemySelectButton = new Button[4];

    public Button[] CharacterSelectButton = new Button[3];

    public GameObject SkillButtonArrow;
    public GameObject SkillButtonViewport;
    public GameObject SkillViewportContent;

    public Button[] SkillButtons = new Button[5];
    public SkillData[] SkillInfo = new SkillData[15];
    [SerializeField]private SkillData[] CurrentSkillData = new SkillData[5];

    [Header("Skill Information Panel")]
    public GameObject SkillInformationPanel;
    public Image SkillIcon;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillDescription;
    public Image SkillUserIcon;

    public GameObject ItemButtonArrow;
    public GameObject ItemButtonViewport;
    public GameObject ItemViewportContent;

    public Button PauseButton;
    public GameObject PausePanel;
    public Button RestartButton;
    public GameObject RestartPanel;
    public Button RestartConfirmButton;
    public Button RestartCancelButton;
    public Button SettingButton;
    public Button SettingReturnButton;
    public GameObject SettingPanel;
    public Button ReturnButton;

    void Start()
    {
        heavenOrHellAnimator = HeavenOrHell.GetComponent<Animator>();
        letsRockAnimator = LetsRock.GetComponent<Animator>();

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
        RestartConfirmButton.onClick.AddListener(RestartConfirmButtonClicked);
        RestartCancelButton.onClick.AddListener(RestartCancelButtonClicked);
        SettingButton.onClick.AddListener(SettingButtonClicked);
        SettingReturnButton.onClick.AddListener(SettingReturnButtonClicked);
        ReturnButton.onClick.AddListener(ReturnButtonClicked);
    }
    
    void Update()
    {
        if (GameManager.instance.isAliceTurn || GameManager.instance.isGretelTurn || GameManager.instance.isSWTurn)
        {
            ChangeSkillButtonSprites();
        }
        else if (!GameManager.instance.isAliceTurn || !GameManager.instance.isGretelTurn || !GameManager.instance.isSWTurn)
        {
            for(int i = 0; i < CurrentSkillData.Length; i++)
            {
                CurrentSkillData[i] = null;
            }
        }

        BattleIntro();
        ActionButtonsActivate();
        StartCoroutine(RestartStage());
    }

    void BattleIntro()
    {
        if(heavenOrHellAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            HeavenOrHell.SetActive(false);
            LetsRock.SetActive(true);
            AudioSource.PlayOneShot(LetsRockAudioClip);
        }

        if (LetsRock.activeSelf && letsRockAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            LetsRock.SetActive(false);
            Transition.SetActive(false);
            PauseButton.gameObject.SetActive(true);

            AudioSource.PlayOneShot(BattleBGM);
            AudioSource.loop = true;
            AudioSource.volume = 0.5f;
        }
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

        EnemySelectButtons.SetActive(true);

        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        ItemButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        ItemButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        for(int i = 0; i < CharacterSelectButton.Length; i++)
        {
            CharacterSelectButton[i].gameObject.SetActive(false);
        }

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

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

        ResetViewportPosition();
    }

    void SkillButtonClicked()
    {
        GameManager.instance.isAttackButtonActive = false;
        GameManager.instance.isGuardButtonActive = false;
        GameManager.instance.isSkillButtonActive = true;
        GameManager.instance.isItemButtonActive = false;

        EnemySelectButtons.SetActive(false);

        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", true);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", true);

        ItemButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        ItemButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

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

        SkillIcon.sprite = CurrentSkillData[number].SkillIcon;
        SkillName.text = CurrentSkillData[number].SkillName;
        SkillDescription.text = CurrentSkillData[number].SkillDescription;
        SkillUserIcon.sprite = CurrentSkillData[number].SkillUserIcon;

        if (number == 4)
        {
            if(GameManager.instance.isAliceTurn)
            {
                EnemySelectButtons.SetActive(false);

                for (int i = 0; i < CharacterSelectButton.Length; i++)
                {
                    CharacterSelectButton[i].gameObject.SetActive(true);
                }
            }
            else if (GameManager.instance.isGretelTurn || GameManager.instance.isSWTurn)
            {
                EnemySelectButtons.SetActive(true);                
            }
        }
        else
        {
            EnemySelectButtons.SetActive(true);

            for (int i = 0; i < CharacterSelectButton.Length; i++)
            {
                CharacterSelectButton[i].gameObject.SetActive(false);
            }
        }
    }

    void ChangeSkillButtonSprites()
    {
        for(int i = 0; i < SkillInfo.Length; i++)
        {
            for(int j = 0; j < SkillButtons.Length; j++)
            {
                if (CurrentSkillData[j] == null)
                {
                    if (GameManager.instance.isAliceTurn && SkillInfo[i].SkillUserName == "Alice")
                    {
                        CurrentSkillData[j] = SkillInfo[i];
                        SkillButtons[j].GetComponent<Image>().sprite = SkillInfo[i].SkillIcon;

                        break;
                    }
                    else if (GameManager.instance.isGretelTurn && SkillInfo[i].SkillUserName == "Gretel")
                    {
                        CurrentSkillData[j] = SkillInfo[i];
                        SkillButtons[j].GetComponent<Image>().sprite = SkillInfo[i].SkillIcon;

                        break;
                    }
                    else if (GameManager.instance.isSWTurn && SkillInfo[i].SkillUserName == "Snow White")
                    {
                        CurrentSkillData[j] = SkillInfo[i];
                        SkillButtons[j].GetComponent<Image>().sprite = SkillInfo[i].SkillIcon;

                        break;
                    }
                }                
            }
        }     
    }

    void ItemButtonClicked()
    {
        GameManager.instance.isAttackButtonActive = false;
        GameManager.instance.isGuardButtonActive = false;
        GameManager.instance.isSkillButtonActive = false;
        GameManager.instance.isItemButtonActive = true;

        EnemySelectButtons.SetActive(false);

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

    void EnemySelectButtonClicked(int number)
    {
        GameManager.instance.EnemyPositionNumber = number;
        GameManager.instance.isAction = true;
        EnemySelectButtons.SetActive(false);

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);

        ResetViewportPosition();
    }

    void OnCharacterSelectButtonClicked(int number)
    {
        if (GameManager.instance.AlicePositionNumber == number)
        {
            GameManager.instance.SelectedCharacterPosition = GameManager.instance.Characters[number].transform.position;

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
        }
        else if (GameManager.instance.GretelPositionNumber == number)
        {
            GameManager.instance.SelectedCharacterPosition = GameManager.instance.Characters[number].transform.position;

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
        }
        else if (GameManager.instance.SWPositionNumber == number)
        {
            GameManager.instance.SelectedCharacterPosition = GameManager.instance.Characters[number].transform.position;

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
        }

        actionButtonsAnimator.SetBool("isActive", false);

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);

        ResetViewportPosition();
    }

    void PauseButtonClicked()
    {
        PausePanel.SetActive(true);
        PauseButton.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    void RestartButtonClicked()
    {
        RestartPanel.SetActive(true);
    }

    void RestartConfirmButtonClicked()
    {
        Transition.SetActive(true);
        TransitionAnimator.SetBool("isTransition", true);
    }

    void RestartCancelButtonClicked()
    {
        RestartPanel.SetActive(false);
    }

    void SettingButtonClicked()
    {
        SettingPanel.SetActive(true);
    }

    void SettingReturnButtonClicked()
    {
        SettingPanel.SetActive(false);
    }

    void ReturnButtonClicked()
    {
        PausePanel.SetActive(false);
        PauseButton.gameObject.SetActive(true);
        Time.timeScale = 1;
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

    IEnumerator RestartStage()
    {
        if (TransitionAnimator.GetCurrentAnimatorStateInfo(0).IsName("-Stage Transition") &&
            TransitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            yield return new WaitForSeconds(2.0f);

            GameManager.instance.isBattleStart = true;
            GameManager.instance.LoadScene("Stage1-1");
        }
    }
}
