using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICharacterInfo : MonoBehaviour
{
    public Image CharacterImage;
    public Animator CharacterAnimator;
    public TextMeshProUGUI CharacterNameText;

    [Header("Character Image")]    
    public Sprite[] CharacterImages = new Sprite[3];

    [Header("Animation")]
    private const string alice = "Alice_Idle";
    private const string gretel = "Gretel_Idle";
    private const string sw = "SnowWhite_Idle";

    [SerializeField] private int changeID;     //Use ID to disable left or right button (ex. if ID == 0, left button disable)
    public Button[] ChangeCharacterButtons = new Button[2];
    public Button CharacterInfoExitButton;

    public Button[] EquipmentButtons = new Button[4];
    public GameObject EquipmentListScroll;

    void Start()
    {        
        CharacterImage.sprite = CharacterImages[0];
        CharacterNameText.text = "ALICE";
        CharacterInfoExitButton.onClick.AddListener(CharacterInfoExitButtonClicked);

        changeID = 0;

        for(int i = 0; i < ChangeCharacterButtons.Length; i++)
        {
            int number = i;
            ChangeCharacterButtons[i].onClick.AddListener(() => ChangeCharacterButtonClicked(number));
        }

        for(int i = 0; i < EquipmentButtons.Length; i++)
        {
            int number = i;
            EquipmentButtons[i].onClick.AddListener(() => EquipmentButtonClicked(number));
        }
    }   

    void ChangeCharacterButtonClicked(int number)
    {
        EquipmentListScroll.SetActive(false);

        if (number == 0)
        {
            changeID--;
        }
        else if(number == 1)
        {
            changeID++;
        }

        switch(changeID)
        {
            case 0:
                ChangeCharacterButtons[0].gameObject.SetActive(false);

                CharacterImage.sprite = CharacterImages[0];
                CharacterAnimator.Play(alice);
                CharacterNameText.text = "ALICE";

                break;
            case 1:
                ChangeCharacterButtons[0].gameObject.SetActive(true);
                ChangeCharacterButtons[1].gameObject.SetActive(true);

                CharacterImage.sprite = CharacterImages[1];
                CharacterAnimator.Play(gretel);
                CharacterNameText.text = "GRETEL";

                break;
            case 2:
                ChangeCharacterButtons[1].gameObject.SetActive(false);

                CharacterImage.sprite = CharacterImages[2];
                CharacterAnimator.Play(sw);
                CharacterNameText.text = "SNOW WHITE";

                break;
        }
    }

    void EquipmentButtonClicked(int number)
    {
        EquipmentListScroll.SetActive(true);
    }

    void CharacterInfoExitButtonClicked()
    {
        changeID = 0;

        ChangeCharacterButtons[0].gameObject.SetActive(false);
        ChangeCharacterButtons[1].gameObject.SetActive(true);

        CharacterImage.sprite = CharacterImages[0];
        CharacterAnimator.Play(alice);
        CharacterNameText.text = "ALICE";

        gameObject.SetActive(false);
    }
}
