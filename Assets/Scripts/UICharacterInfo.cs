using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICharacterInfo : MonoBehaviour
{
    public UIInventory UIInventory;

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
    public Button SelectedEquipmentButton;
    public ScrollRect EquipmentListScroll;
    public GameObject WeaponsContent;
    public GameObject HelmetsContent;
    public GameObject ArmorsContent;
    public GameObject ShoesContent;

    public List<Button> UIWeapons = new List<Button>();
    public List<Button> UIHelmets = new List<Button>();
    public List<Button> UIArmors = new List<Button>();
    public List<Button> UIShoes = new List<Button>();

    public GameObject EquipmentSelectionButtons;
    public EquipmentData SelectedEquipmentData;
    public Button EquipmentConfirmButton;
    public Button EquipmentCancelButton;

    public EquipmentData[] AliceEquipments = new EquipmentData[4];
    public EquipmentData[] GretelEquipments = new EquipmentData[4];
    public EquipmentData[] SWEquipments = new EquipmentData[4];

    void Start()
    {
        CharacterNameText.text = "ALICE";
        CharacterInfoExitButton.onClick.AddListener(CharacterInfoExitButtonClicked);

        changeID = 0;

        for (int i = 0; i < ChangeCharacterButtons.Length; i++)
        {
            int number = i;
            ChangeCharacterButtons[i].onClick.AddListener(() => ChangeCharacterButtonClicked(number));
        }

        for (int i = 0; i < EquipmentButtons.Length; i++)
        {
            int number = i;
            EquipmentButtons[i].onClick.AddListener(() => EquipmentButtonClicked(number));
        }

        for(int i = 0; i < UIWeapons.Count; i++)
        {
            int number = i;
            UIWeapons[i].onClick.AddListener(() => UIWeaponButtonClicked(number));
        }

        for (int i = 0; i < UIHelmets.Count; i++)
        {
            int number = i;
            UIHelmets[i].onClick.AddListener(() => UIHelmetButtonClicked(number));
        }

        for (int i = 0; i < UIArmors.Count; i++)
        {
            int number = i;
            UIArmors[i].onClick.AddListener(() => UIArmorButtonClicked(number));
        }

        for (int i = 0; i < UIShoes.Count; i++)
        {
            int number = i;
            UIShoes[i].onClick.AddListener(() => UIShoeButtonClicked(number));
        }

        EquipmentConfirmButton.onClick.AddListener(EquipmentConfirmButtonClicked);
        EquipmentCancelButton.onClick.AddListener(EquipmentCancelButtonClicked);

    }

    void Update()
    {
        SortUIEquipments();
    }

    void ChangeCharacterButtonClicked(int number)
    {
        EquipmentListScroll.gameObject.SetActive(false);
        EquipmentSelectionButtons.SetActive(false);

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
                
                CharacterAnimator.Play(alice);
                CharacterNameText.text = "ALICE";

                for(int i = 0; i < 4; i++)
                {
                    if (AliceEquipments[i] == null)
                        EquipmentButtons[i].GetComponent<Image>().sprite = null;
                    else
                        EquipmentButtons[i].GetComponent<Image>().sprite = AliceEquipments[i].EquipmentSprite;
                }

                break;
            case 1:
                ChangeCharacterButtons[0].gameObject.SetActive(true);
                ChangeCharacterButtons[1].gameObject.SetActive(true);
                
                CharacterAnimator.Play(gretel);
                CharacterNameText.text = "GRETEL";

                for (int i = 0; i < 4; i++)
                {
                    if (GretelEquipments[i] == null)
                        EquipmentButtons[i].GetComponent<Image>().sprite = null;
                    else
                        EquipmentButtons[i].GetComponent<Image>().sprite = GretelEquipments[i].EquipmentSprite;
                }

                break;
            case 2:
                ChangeCharacterButtons[1].gameObject.SetActive(false);
                
                CharacterAnimator.Play(sw);
                CharacterNameText.text = "SNOW WHITE";

                for (int i = 0; i < 4; i++)
                {
                    if (SWEquipments[i] == null)
                        EquipmentButtons[i].GetComponent<Image>().sprite = null;
                    else
                        EquipmentButtons[i].GetComponent<Image>().sprite = SWEquipments[i].EquipmentSprite;
                }

                break;
        }
    }

    void EquipmentButtonClicked(int number)
    {
        EquipmentListScroll.gameObject.SetActive(true);
        EquipmentSelectionButtons.SetActive(false);
        SelectedEquipmentButton = EquipmentButtons[number];

        switch (number)
        {
            case 0:
                EquipmentListScroll.content = WeaponsContent.GetComponent<RectTransform>();

                WeaponsContent.SetActive(true);
                HelmetsContent.SetActive(false);
                ArmorsContent.SetActive(false);
                ShoesContent.SetActive(false);

                break;

            case 1:
                EquipmentListScroll.content = HelmetsContent.GetComponent<RectTransform>();

                WeaponsContent.SetActive(false);
                HelmetsContent.SetActive(true);
                ArmorsContent.SetActive(false);
                ShoesContent.SetActive(false);

                break;

            case 2:
                EquipmentListScroll.content = ArmorsContent.GetComponent<RectTransform>();

                WeaponsContent.SetActive(false);
                HelmetsContent.SetActive(false);
                ArmorsContent.SetActive(true);
                ShoesContent.SetActive(false);

                break;

            case 3:
                EquipmentListScroll.content = ShoesContent.GetComponent<RectTransform>();

                WeaponsContent.SetActive(false);
                HelmetsContent.SetActive(false);
                ArmorsContent.SetActive(false);
                ShoesContent.SetActive(true);

                break;
        }
    }

    void UIWeaponButtonClicked(int number)
    {
        EquipmentSelectionButtons.SetActive(true);
        SelectedEquipmentData = UIWeapons[number].GetComponent<UIEquipment>().EquipmentData;
    }

    void UIHelmetButtonClicked(int number)
    {
        EquipmentSelectionButtons.SetActive(true);
        SelectedEquipmentData = UIHelmets[number].GetComponent<UIEquipment>().EquipmentData;
    }

    void UIArmorButtonClicked(int number)
    {
        EquipmentSelectionButtons.SetActive(true);
        SelectedEquipmentData = UIArmors[number].GetComponent<UIEquipment>().EquipmentData;
    }

    void UIShoeButtonClicked(int number)
    {
        EquipmentSelectionButtons.SetActive(true);
        SelectedEquipmentData = UIShoes[number].GetComponent<UIEquipment>().EquipmentData;
    }

    void EquipmentConfirmButtonClicked()
    {
        if(changeID == 0)
        {
            if (SelectedEquipmentData.EquipmentCategory == "Weapon")
            {
                AliceEquipments[0] = SelectedEquipmentData;
                EquipmentButtons[0].GetComponent<Image>().sprite = AliceEquipments[0].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Helmet")
            {
                AliceEquipments[1] = SelectedEquipmentData;
                EquipmentButtons[1].GetComponent<Image>().sprite = AliceEquipments[1].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Armor")
            {
                AliceEquipments[2] = SelectedEquipmentData;
                EquipmentButtons[2].GetComponent<Image>().sprite = AliceEquipments[2].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Shoe")
            {
                AliceEquipments[3] = SelectedEquipmentData;
                EquipmentButtons[3].GetComponent<Image>().sprite = AliceEquipments[3].EquipmentSprite;
            }
        }
        else if(changeID == 1)
        {
            if (SelectedEquipmentData.EquipmentCategory == "Weapon")
            {
                GretelEquipments[0] = SelectedEquipmentData;
                EquipmentButtons[0].GetComponent<Image>().sprite = GretelEquipments[0].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Helmet")
            {
                GretelEquipments[1] = SelectedEquipmentData;
                EquipmentButtons[1].GetComponent<Image>().sprite = GretelEquipments[1].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Armor")
            {
                GretelEquipments[2] = SelectedEquipmentData;
                EquipmentButtons[2].GetComponent<Image>().sprite = GretelEquipments[2].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Shoe")
            {
                GretelEquipments[3] = SelectedEquipmentData;
                EquipmentButtons[3].GetComponent<Image>().sprite = GretelEquipments[3].EquipmentSprite;
            }
        }
        else if (changeID == 2)
        {
            if (SelectedEquipmentData.EquipmentCategory == "Weapon")
            {
                SWEquipments[0] = SelectedEquipmentData;
                EquipmentButtons[0].GetComponent<Image>().sprite = SWEquipments[0].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Helmet")
            {
                SWEquipments[1] = SelectedEquipmentData;
                EquipmentButtons[1].GetComponent<Image>().sprite = SWEquipments[1].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Armor")
            {
                SWEquipments[2] = SelectedEquipmentData;
                EquipmentButtons[2].GetComponent<Image>().sprite = SWEquipments[2].EquipmentSprite;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Shoe")
            {
                SWEquipments[3] = SelectedEquipmentData;
                EquipmentButtons[3].GetComponent<Image>().sprite = SWEquipments[3].EquipmentSprite;
            }
        }

        EquipmentSelectionButtons.SetActive(false);
    }

    void EquipmentCancelButtonClicked()
    {
        EquipmentSelectionButtons.SetActive(false);
    }

    void SortUIEquipments()
    {
        for (int i = 0; i < UIWeapons.Count; i++)
        {
            for (int j = i + 1; j < UIWeapons.Count; j++)
            {
                if (UIWeapons[i].GetComponent<UIEquipment>().EquipmentData.EquipmentID
                    > UIWeapons[j].GetComponent<UIEquipment>().EquipmentData.EquipmentID)
                {
                    UIWeapons[i].transform.SetSiblingIndex(j);
                }
            }
        }

        for (int i = 0; i < UIHelmets.Count; i++)
        {
            for (int j = i + 1; j < UIHelmets.Count; j++)
            {
                if (UIHelmets[i].GetComponent<UIEquipment>().EquipmentData.EquipmentID
                    > UIHelmets[j].GetComponent<UIEquipment>().EquipmentData.EquipmentID)
                {
                    UIHelmets[i].transform.SetSiblingIndex(j);
                }
            }
        }

        for (int i = 0; i < UIArmors.Count; i++)
        {
            for (int j = i + 1; j < UIArmors.Count; j++)
            {
                if (UIArmors[i].GetComponent<UIEquipment>().EquipmentData.EquipmentID
                    > UIArmors[j].GetComponent<UIEquipment>().EquipmentData.EquipmentID)
                {
                    UIArmors[i].transform.SetSiblingIndex(j);
                }
            }
        }

        for (int i = 0; i < UIShoes.Count; i++)
        {
            for (int j = i + 1; j < UIShoes.Count; j++)
            {
                if (UIShoes[i].GetComponent<UIEquipment>().EquipmentData.EquipmentID
                    > UIShoes[j].GetComponent<UIEquipment>().EquipmentData.EquipmentID)
                {
                    UIShoes[i].transform.SetSiblingIndex(j);
                }
            }
        }
    }

    void CharacterInfoExitButtonClicked()
    {
        EquipmentListScroll.gameObject.SetActive(false);
        EquipmentSelectionButtons.SetActive(false);

        changeID = 0;

        ChangeCharacterButtons[0].gameObject.SetActive(false);
        ChangeCharacterButtons[1].gameObject.SetActive(true);

        CharacterAnimator.Play(alice);
        CharacterNameText.text = "ALICE";

        gameObject.SetActive(false);
    }
}
