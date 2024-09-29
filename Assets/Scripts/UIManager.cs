using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject ActionPanel;

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
    public SkillData[] SkillInfo = new SkillData[5];
    [SerializeField]private SkillData[] CurrentSkillData = new SkillData[5];

    [Header("Skill Information Panel")]
    public GameObject SkillInformationPanel;
    public Image SkillIcon;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillDescription;
    public Image SkillUserIcon;


    void Start()
    {
        AttackButton.onClick.AddListener(AttackButtonClicked);
        GuardButton.onClick.AddListener(GuardButtonClicked);
        SkillButton.onClick.AddListener(SkillButtonClicked);
        ItemButton.onClick.AddListener(ItemButtonClicked);

        for(int i = 0; i < EnemySelectButton.Length; i++)
        {
            int number = i;
            EnemySelectButton[i].onClick.AddListener(() => EnemySelectButtonClicked(number));
        }

        CharacterSelectButton[GameManager.instance.AlicePositionNumber].onClick.AddListener(AliceSelectButtonClicked);

        for(int i = 0; i < SkillButtons.Length; i++)
        {
            int number = i;

            SkillButtons[i].onClick.AddListener(() => SkillButtonClicked(number));
        }
    }
    
    void Update()
    {
        ChangeSkillButtonSprites();
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

        if (GameManager.instance.isAliceTurn)
        {
            CharacterSelectButton[GameManager.instance.AlicePositionNumber].gameObject.SetActive(false);
        }
        else if (GameManager.instance.isGretelTurn)
        {
            CharacterSelectButton[GameManager.instance.GretelPositionNumber].gameObject.SetActive(false);
        }
        else if (GameManager.instance.isSWTurn)
        {
            CharacterSelectButton[GameManager.instance.SWPositionNumber].gameObject.SetActive(false);
        }

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

        ResetSkillViewportPosition();
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
        }
        else if(GameManager.instance.isGretelTurn)
        {
            CharacterSelectButton[GameManager.instance.GretelPositionNumber].gameObject.SetActive(true);
        }
        else if(GameManager.instance.isSWTurn)
        {
            CharacterSelectButton[GameManager.instance.SWPositionNumber].gameObject.SetActive(true);
        }

        EnemySelectButtons.SetActive(false);

        SkillButtonArrow.GetComponent<Animator>().SetBool("isActive", false);
        SkillButtonViewport.GetComponent<Animator>().SetBool("isActive", false);

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);

        ResetSkillViewportPosition();
    }

    void AliceSelectButtonClicked()
    {
        if (GameManager.instance.isGuardButtonActive)
        {
            GameManager.instance.isAction = true;
            CharacterSelectButton[GameManager.instance.AlicePositionNumber].gameObject.SetActive(false);
        }
        else if(GameManager.instance.isSkillButtonActive)
        {
            GameManager.instance.isAction = true;

            for (int i = 0; i < CharacterSelectButton.Length; i++)
            {
                CharacterSelectButton[i].gameObject.SetActive(false);
            }
        }

        ResetSkillViewportPosition();
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

        if (GameManager.instance.isAliceTurn)
        {
            CharacterSelectButton[GameManager.instance.AlicePositionNumber].gameObject.SetActive(false);
        }
        else if (GameManager.instance.isGretelTurn)
        {
            CharacterSelectButton[GameManager.instance.GretelPositionNumber].gameObject.SetActive(false);
        }
        else if (GameManager.instance.isSWTurn)
        {
            CharacterSelectButton[GameManager.instance.SWPositionNumber].gameObject.SetActive(false);
        }

        ResetSkillViewportPosition();
    }

    void SkillButtonClicked(int number)
    {
        GameManager.instance.SkillButtonNumber = number;

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", true);

        SkillIcon.sprite = CurrentSkillData[number].SkillIcon;
        SkillName.text = CurrentSkillData[number].SkillName;
        SkillDescription.text = CurrentSkillData[number].SkillDescription;
        SkillUserIcon.sprite = CurrentSkillData[number].SkillUserIcon;

        if (number == 4)
        {
            for (int i = 0; i < CharacterSelectButton.Length; i++)
            {
                CharacterSelectButton[i].gameObject.SetActive(true);
            }
        }
        else
        {
            EnemySelectButtons.SetActive(true);
        }
    }

    void ChangeSkillButtonSprites()
    {
        for(int i = 0; i < SkillButtons.Length; i++)
        {
            for(int j = 0; j < SkillInfo.Length; j++)
            {
                if (GameManager.instance.isAliceTurn && SkillInfo[j].SkillUserName == "Alice")
                {
                    if (CurrentSkillData[j] == SkillInfo[j])
                        continue;

                    CurrentSkillData[i] = SkillInfo[j];
                    SkillButtons[i].GetComponent<Image>().sprite = SkillInfo[j].SkillIcon;
                }
                else if(GameManager.instance.isGretelTurn && SkillInfo[j].SkillUserName == "Gretel")
                {
                    if (CurrentSkillData[j] == SkillInfo[j])
                        continue;

                    CurrentSkillData[i] = SkillInfo[j];
                    SkillButtons[i].GetComponent<Image>().sprite = SkillInfo[j].SkillIcon;
                }
                else if (GameManager.instance.isSWTurn && SkillInfo[j].SkillUserName == "Snow White")
                {
                    if (CurrentSkillData[j] == SkillInfo[j])
                        continue;

                    CurrentSkillData[i] = SkillInfo[j];
                    SkillButtons[i].GetComponent<Image>().sprite = SkillInfo[j].SkillIcon;
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

        if (GameManager.instance.isAliceTurn)
        {
            CharacterSelectButton[GameManager.instance.AlicePositionNumber].gameObject.SetActive(false);
        }
        else if (GameManager.instance.isGretelTurn)
        {
            CharacterSelectButton[GameManager.instance.GretelPositionNumber].gameObject.SetActive(false);
        }
        else if (GameManager.instance.isSWTurn)
        {
            CharacterSelectButton[GameManager.instance.SWPositionNumber].gameObject.SetActive(false);
        }

        SkillInformationPanel.GetComponent<Animator>().SetBool("isActive", false);
        SkillViewportContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        ResetSkillViewportPosition();
    }

    void EnemySelectButtonClicked(int number)
    {
        GameManager.instance.EnemyPositionNumber = number;
        GameManager.instance.isAction = true;
        EnemySelectButtons.SetActive(false);

        ResetSkillViewportPosition();
    }

    void ResetSkillViewportPosition()
    {
        if(SkillButtonViewport.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Skill Buttons Viewport Idle") ||
            SkillButtonViewport.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("-Skill Buttons Viewport"))
        {
            SkillViewportContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
