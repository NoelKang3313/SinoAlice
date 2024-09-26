using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Button[] SkillButtons = new Button[5];
    public Sprite[] AliceSkillSprites = new Sprite[5];

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
    }

    void AliceSelectButtonClicked()
    {
        GameManager.instance.isAction = true;
        CharacterSelectButton[GameManager.instance.AlicePositionNumber].gameObject.SetActive(false);
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
    }

    void SkillButtonClicked(int number)
    {
        switch(number)
        {
            case 0:
                Debug.Log("Skill 1");
                break;
            case 1:
                Debug.Log("Skill 2");
                break;
            case 2:
                Debug.Log("Skill 3");
                break;
            case 3:
                Debug.Log("Skill 4");
                break;
            case 4:
                Debug.Log("Skill 5");
                break;
        }
    }

    void ChangeSkillButtonSprites()
    {
        for (int i = 0; i < SkillButtons.Length; i++)
        {
            if (GameManager.instance.isAliceTurn)
            {
                SkillButtons[i].GetComponent<Image>().sprite = AliceSkillSprites[i];
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
    }

    void EnemySelectButtonClicked(int number)
    {
        GameManager.instance.EnemyPositionNumber = number;
        GameManager.instance.isAction = true;
        EnemySelectButtons.SetActive(false);
    }

    //void ActionPanelActive()
    //{
    //    if(GameManager.instance.isAliceTurn || GameManager.instance.isGretelTurn || GameManager.instance.isSWTurn)
    //    {
    //        ActionPanel.SetActive(true);
    //    }
    //    else if (!GameManager.instance.isAliceTurn || !GameManager.instance.isGretelTurn || !GameManager.instance.isSWTurn)
    //    {
    //        ActionPanel.SetActive(false);
    //        EnemySelectButtons.SetActive(false);
    //    }
    //}
}
