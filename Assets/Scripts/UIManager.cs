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
    }
    
    void Update()
    {
        //ActionPanelActive();
    }

    void AttackButtonClicked()
    {
        GameManager.instance.isAttackButtonActive = true;
        GameManager.instance.isGuardButtonActive = false;
        GameManager.instance.isSkillButtonActive = false;
        GameManager.instance.isItemButtonActive = false;

        EnemySelectButtons.SetActive(true);
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
            CharacterSelectButton[GameManager.instance.AlicePositionNumber].onClick.AddListener(AliceSelectButtonClicked);
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
    }

    void ItemButtonClicked()
    {
        GameManager.instance.isAttackButtonActive = false;
        GameManager.instance.isGuardButtonActive = false;
        GameManager.instance.isSkillButtonActive = false;
        GameManager.instance.isItemButtonActive = true;

        EnemySelectButtons.SetActive(false);
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
