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

    public Button UICEquipment;

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
    public Button[] RemoveEquipmentButtons = new Button[4];
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
    public Button SelectedButton;
    public EquipmentData SelectedEquipmentData;
    public Button EquipmentConfirmButton;
    public Button EquipmentCancelButton;

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

        for (int i = 0; i < 4; i++)
        {
            int number = i;
            EquipmentButtons[i].onClick.AddListener(() => EquipmentButtonClicked(number));
            RemoveEquipmentButtons[i].onClick.AddListener(() => RemoveEquipmentButtonClicked(number));
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
        for(int i = 0; i < RemoveEquipmentButtons.Length; i++)
        {
            RemoveEquipmentButtons[i].gameObject.SetActive(false);
        }

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
                    if (GameManager.instance.AliceEquipments[i] == null)
                        EquipmentButtons[i].GetComponent<Image>().sprite = null;
                    else
                        EquipmentButtons[i].GetComponent<Image>().sprite = GameManager.instance.AliceEquipments[i].EquipmentSprite;
                }

                break;
            case 1:
                ChangeCharacterButtons[0].gameObject.SetActive(true);
                ChangeCharacterButtons[1].gameObject.SetActive(true);
                
                CharacterAnimator.Play(gretel);
                CharacterNameText.text = "GRETEL";

                for (int i = 0; i < 4; i++)
                {
                    if (GameManager.instance.GretelEquipments[i] == null)
                        EquipmentButtons[i].GetComponent<Image>().sprite = null;
                    else
                        EquipmentButtons[i].GetComponent<Image>().sprite = GameManager.instance.GretelEquipments[i].EquipmentSprite;
                }

                break;
            case 2:
                ChangeCharacterButtons[1].gameObject.SetActive(false);
                
                CharacterAnimator.Play(sw);
                CharacterNameText.text = "SNOW WHITE";

                for (int i = 0; i < 4; i++)
                {
                    if (GameManager.instance.SWEquipments[i] == null)
                        EquipmentButtons[i].GetComponent<Image>().sprite = null;
                    else
                        EquipmentButtons[i].GetComponent<Image>().sprite = GameManager.instance.SWEquipments[i].EquipmentSprite;
                }

                break;
        }
    }

    void EquipmentButtonClicked(int number)
    {
        for(int i = 0; i < RemoveEquipmentButtons.Length; i++)
        {
            RemoveEquipmentButtons[i].gameObject.SetActive(false);
        }

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

                CheckEquipmentEquipped(number);

                break;

            case 1:
                EquipmentListScroll.content = HelmetsContent.GetComponent<RectTransform>();

                WeaponsContent.SetActive(false);
                HelmetsContent.SetActive(true);
                ArmorsContent.SetActive(false);
                ShoesContent.SetActive(false);

                CheckEquipmentEquipped(number);

                break;

            case 2:
                EquipmentListScroll.content = ArmorsContent.GetComponent<RectTransform>();

                WeaponsContent.SetActive(false);
                HelmetsContent.SetActive(false);
                ArmorsContent.SetActive(true);
                ShoesContent.SetActive(false);

                CheckEquipmentEquipped(number);

                break;

            case 3:
                EquipmentListScroll.content = ShoesContent.GetComponent<RectTransform>();

                WeaponsContent.SetActive(false);
                HelmetsContent.SetActive(false);
                ArmorsContent.SetActive(false);
                ShoesContent.SetActive(true);

                CheckEquipmentEquipped(number);

                break;
        }
    }

    void RemoveEquipmentButtonClicked(int number)
    {
        switch(number)
        {
            case 0:
                if(UIWeapons.Count == 0)
                {
                    Button CEquipmentSlot = Instantiate(UICEquipment, WeaponsContent.transform.position, Quaternion.identity);
                    CEquipmentSlot.transform.SetParent(WeaponsContent.transform);

                    if (changeID == 0)
                    {
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = GameManager.instance.AliceEquipments[number].EquipmentSprite;
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = GameManager.instance.AliceEquipments[number];
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = 1;
                    }

                    UIWeapons.Add(CEquipmentSlot);
                }
                else
                {
                    for(int i = 0; i < UIWeapons.Count; i++)
                    {
                        if(changeID == 0 &&
                            UIWeapons[i].GetComponent<UICEquipment>().EquipmentData == GameManager.instance.AliceEquipments[number])
                        {
                            UIWeapons[i].GetComponent<UICEquipment>().EquipmentAmount++;
                        }
                    }
                }

                break;

            case 1:
                if (UIHelmets.Count == 0)
                {
                    Button CEquipmentSlot = Instantiate(UICEquipment, HelmetsContent.transform.position, Quaternion.identity);
                    CEquipmentSlot.transform.SetParent(HelmetsContent.transform);

                    if (changeID == 0)
                    {
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = GameManager.instance.AliceEquipments[number].EquipmentSprite;
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = GameManager.instance.AliceEquipments[number];
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = 1;
                    }

                    UIHelmets.Add(CEquipmentSlot);
                }
                else
                {
                    for (int i = 0; i < UIHelmets.Count; i++)
                    {
                        if (changeID == 0 &&
                            UIHelmets[i].GetComponent<UICEquipment>().EquipmentData == GameManager.instance.AliceEquipments[number])
                        {
                            UIHelmets[i].GetComponent<UICEquipment>().EquipmentAmount++;
                        }
                    }
                }

                break;

            case 2:
                if (UIArmors.Count == 0)
                {
                    Button CEquipmentSlot = Instantiate(UICEquipment, ArmorsContent.transform.position, Quaternion.identity);
                    CEquipmentSlot.transform.SetParent(ArmorsContent.transform);

                    if (changeID == 0)
                    {
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = GameManager.instance.AliceEquipments[number].EquipmentSprite;
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = GameManager.instance.AliceEquipments[number];
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = 1;
                    }

                    UIArmors.Add(CEquipmentSlot);
                }
                else
                {
                    for (int i = 0; i < UIArmors.Count; i++)
                    {
                        if (changeID == 0 &&
                            UIArmors[i].GetComponent<UICEquipment>().EquipmentData == GameManager.instance.AliceEquipments[number])
                        {
                            UIArmors[i].GetComponent<UICEquipment>().EquipmentAmount++;
                        }
                    }
                }

                break;

            case 3:
                if (UIShoes.Count == 0)
                {
                    Button CEquipmentSlot = Instantiate(UICEquipment, ShoesContent.transform.position, Quaternion.identity);
                    CEquipmentSlot.transform.SetParent(ShoesContent.transform);

                    if (changeID == 0)
                    {
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = GameManager.instance.AliceEquipments[number].EquipmentSprite;
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = GameManager.instance.AliceEquipments[number];
                        CEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = 1;
                    }

                    UIShoes.Add(CEquipmentSlot);
                }
                else
                {
                    for (int i = 0; i < UIShoes.Count; i++)
                    {
                        if (changeID == 0 &&
                            UIShoes[i].GetComponent<UICEquipment>().EquipmentData == GameManager.instance.AliceEquipments[number])
                        {
                            UIShoes[i].GetComponent<UICEquipment>().EquipmentAmount++;
                        }
                    }
                }

                break;
        }

        EquipmentButtons[number].GetComponent<Image>().sprite = null;
        GameManager.instance.AliceEquipments[number] = null;
        RemoveEquipmentButtons[number].gameObject.SetActive(false);

        ResetEquipmentButtons();
    }

    void UIWeaponButtonClicked(int number)
    {
        EquipmentSelectionButtons.SetActive(true);
        SelectedEquipmentData = UIWeapons[number].GetComponent<UICEquipment>().EquipmentData;
        SelectedButton = UIWeapons[number];
    }

    void UIHelmetButtonClicked(int number)
    {
        EquipmentSelectionButtons.SetActive(true);
        SelectedEquipmentData = UIHelmets[number].GetComponent<UICEquipment>().EquipmentData;
        SelectedButton = UIHelmets[number];
    }

    void UIArmorButtonClicked(int number)
    {
        EquipmentSelectionButtons.SetActive(true);
        SelectedEquipmentData = UIArmors[number].GetComponent<UICEquipment>().EquipmentData;
        SelectedButton = UIArmors[number];
    }

    void UIShoeButtonClicked(int number)
    {
        EquipmentSelectionButtons.SetActive(true);
        SelectedEquipmentData = UIShoes[number].GetComponent<UICEquipment>().EquipmentData;
        SelectedButton = UIShoes[number];
    }

    void EquipmentConfirmButtonClicked()
    {
        if(changeID == 0)
        {
            if (SelectedEquipmentData.EquipmentCategory == "Weapon")
            {                
                GameManager.instance.AliceEquipments[0] = SelectedEquipmentData;
                EquipmentButtons[0].GetComponent<Image>().sprite = GameManager.instance.AliceEquipments[0].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Helmet")
            {                
                GameManager.instance.AliceEquipments[1] = SelectedEquipmentData;
                EquipmentButtons[1].GetComponent<Image>().sprite = GameManager.instance.AliceEquipments[1].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Armor")
            {                
                GameManager.instance.AliceEquipments[2] = SelectedEquipmentData;
                EquipmentButtons[2].GetComponent<Image>().sprite = GameManager.instance.AliceEquipments[2].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Shoe")
            {                
                GameManager.instance.AliceEquipments[3] = SelectedEquipmentData;
                EquipmentButtons[3].GetComponent<Image>().sprite = GameManager.instance.AliceEquipments[3].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
        }
        else if(changeID == 1)
        {
            if (SelectedEquipmentData.EquipmentCategory == "Weapon")
            {
                GameManager.instance.GretelEquipments[0] = SelectedEquipmentData;
                EquipmentButtons[0].GetComponent<Image>().sprite = GameManager.instance.GretelEquipments[0].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Helmet")
            {
                GameManager.instance.GretelEquipments[1] = SelectedEquipmentData;
                EquipmentButtons[1].GetComponent<Image>().sprite = GameManager.instance.GretelEquipments[1].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Armor")
            {
                GameManager.instance.GretelEquipments[2] = SelectedEquipmentData;
                EquipmentButtons[2].GetComponent<Image>().sprite = GameManager.instance.GretelEquipments[2].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Shoe")
            {
                GameManager.instance.GretelEquipments[3] = SelectedEquipmentData;
                EquipmentButtons[3].GetComponent<Image>().sprite = GameManager.instance.GretelEquipments[3].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
        }
        else if (changeID == 2)
        {
            if (SelectedEquipmentData.EquipmentCategory == "Weapon")
            {
                GameManager.instance.SWEquipments[0] = SelectedEquipmentData;
                EquipmentButtons[0].GetComponent<Image>().sprite = GameManager.instance.SWEquipments[0].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Helmet")
            {
                GameManager.instance.SWEquipments[1] = SelectedEquipmentData;
                EquipmentButtons[1].GetComponent<Image>().sprite = GameManager.instance.SWEquipments[1].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Armor")
            {
                GameManager.instance.SWEquipments[2] = SelectedEquipmentData;
                EquipmentButtons[2].GetComponent<Image>().sprite = GameManager.instance.SWEquipments[2].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
            else if (SelectedEquipmentData.EquipmentCategory == "Shoe")
            {
                GameManager.instance.SWEquipments[3] = SelectedEquipmentData;
                EquipmentButtons[3].GetComponent<Image>().sprite = GameManager.instance.SWEquipments[3].EquipmentSprite;
                SelectedButton.GetComponent<UICEquipment>().EquipmentAmount--;
            }
        }

        EquipmentSelectionButtons.SetActive(false);
        CheckEquipmentAmount();
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
                if (UIWeapons[i].GetComponent<UICEquipment>().EquipmentData.EquipmentID
                    > UIWeapons[j].GetComponent<UICEquipment>().EquipmentData.EquipmentID)
                {
                    UIWeapons[i].transform.SetSiblingIndex(j);
                }
            }
        }

        for (int i = 0; i < UIHelmets.Count; i++)
        {
            for (int j = i + 1; j < UIHelmets.Count; j++)
            {
                if (UIHelmets[i].GetComponent<UICEquipment>().EquipmentData.EquipmentID
                    > UIHelmets[j].GetComponent<UICEquipment>().EquipmentData.EquipmentID)
                {
                    UIHelmets[i].transform.SetSiblingIndex(j);
                }
            }
        }

        for (int i = 0; i < UIArmors.Count; i++)
        {
            for (int j = i + 1; j < UIArmors.Count; j++)
            {
                if (UIArmors[i].GetComponent<UICEquipment>().EquipmentData.EquipmentID
                    > UIArmors[j].GetComponent<UICEquipment>().EquipmentData.EquipmentID)
                {
                    UIArmors[i].transform.SetSiblingIndex(j);
                }
            }
        }

        for (int i = 0; i < UIShoes.Count; i++)
        {
            for (int j = i + 1; j < UIShoes.Count; j++)
            {
                if (UIShoes[i].GetComponent<UICEquipment>().EquipmentData.EquipmentID
                    > UIShoes[j].GetComponent<UICEquipment>().EquipmentData.EquipmentID)
                {
                    UIShoes[i].transform.SetSiblingIndex(j);
                }
            }
        }
    }

    void CheckEquipmentAmount()
    {
        for(int i = 0; i < UIWeapons.Count; i++)
        {
            if (UIWeapons[i].GetComponent<UICEquipment>().EquipmentAmount == 0)
            {
                Destroy(UIWeapons[i].gameObject);
                UIWeapons.RemoveAt(i);
            }
        }

        for (int i = 0; i < UIHelmets.Count; i++)
        {
            if (UIHelmets[i].GetComponent<UICEquipment>().EquipmentAmount == 0)
            {
                Destroy(UIHelmets[i].gameObject);
                UIHelmets.RemoveAt(i);
            }
        }

        for (int i = 0; i < UIArmors.Count; i++)
        {
            if (UIArmors[i].GetComponent<UICEquipment>().EquipmentAmount == 0)
            {
                Destroy(UIArmors[i].gameObject);
                UIArmors.RemoveAt(i);
            }
        }

        for (int i = 0; i < UIShoes.Count; i++)
        {
            if (UIShoes[i].GetComponent<UICEquipment>().EquipmentAmount == 0)
            {
                Destroy(UIShoes[i].gameObject);
                UIShoes.RemoveAt(i);
            }
        }
    }

    void CheckEquipmentEquipped(int number)
    {
        if (changeID == 0)
        {
            if (GameManager.instance.AliceEquipments[number] != null)
            {
                RemoveEquipmentButtons[number].gameObject.SetActive(true);
            }
        }
        else if (changeID == 1)
        {
            if (GameManager.instance.GretelEquipments[number] != null)
            {
                RemoveEquipmentButtons[number].gameObject.SetActive(true);
            }
        }
        else if (changeID == 2)
        {
            if (GameManager.instance.SWEquipments[number] != null)
            {
                RemoveEquipmentButtons[number].gameObject.SetActive(true);
            }
        }
    }

    public void ResetEquipmentButtons()
    {
        for (int i = 0; i < UIWeapons.Count; i++)
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
