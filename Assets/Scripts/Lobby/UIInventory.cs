using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    public LobbyUIManager UIManager;

    public Button UIItem;
    public Button UIEquipment;

    public GameObject InventoryViewport;
    public GameObject InventoryItemContent;
    public GameObject InventoryEquipmentContent;
    public GameObject InventoryWeaponContent;
    public GameObject InventoryHelmetContent;
    public GameObject InventoryArmorContent;
    public GameObject InventoryShoeContent;
    public GameObject ItemInfoPanel;
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI ItemDescriptionText;

    public ScrollRect InventoryScrollRect;
    public Button InventoryItemButton;
    public Button InventoryEquipmentButton;
    public GameObject EquipmentCategories;
    public Button InventoryWeaponButton;
    public Button InventoryHelmetButton;
    public Button InventoryArmorButton;
    public Button InventoryShoeButton;

    public List<Button> UIItems = new List<Button>();
    public List<Button> UIEquipments = new List<Button>();

    public Button InventoryExitButton;

    void Start()
    {
        InventoryItemButton.onClick.AddListener(InventoryItemButtonClicked);
        InventoryEquipmentButton.onClick.AddListener(InventoryEquipmentButtonClicked);
        InventoryWeaponButton.onClick.AddListener(InventoryWeaponButtonClicked);
        InventoryHelmetButton.onClick.AddListener(InventoryHelmetButtonClicked);
        InventoryArmorButton.onClick.AddListener(InventoryArmorButtonClicked);
        InventoryShoeButton.onClick.AddListener(InventoryShoeButtonClicked);

        InventoryExitButton.onClick.AddListener(InventoryExitButtonClicked);
    }

    void Update()
    {
        for(int i = 0; i < UIItems.Count; i++)
        {
            int number = i;

            UIItems[i].onClick.AddListener(() => UIItemSlotClicked(number));
        }

        for(int i = 0; i < UIEquipments.Count; i++)
        {
            int number = i;
            UIEquipments[i].onClick.AddListener(() => UIEquipmentSlotClicked(number));
        }

        SortUIItems();
        SortUIEquipments();
    }

    void InventoryItemButtonClicked()
    {
        InventoryViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);

        InventoryItemContent.SetActive(true);
        InventoryWeaponContent.SetActive(false);
        InventoryHelmetContent.SetActive(false);
        InventoryArmorContent.SetActive(false);
        InventoryShoeContent.SetActive(false);

        EquipmentCategories.SetActive(false);

        InventoryScrollRect.content = InventoryItemContent.GetComponent<RectTransform>();
    }

    void InventoryEquipmentButtonClicked()
    {
        InventoryViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);

        InventoryItemContent.SetActive(false);
        InventoryWeaponContent.SetActive(true);

        EquipmentCategories.SetActive(true);

        InventoryScrollRect.content = InventoryWeaponContent.GetComponent<RectTransform>();
    }

    void InventoryWeaponButtonClicked()
    {        
        InventoryWeaponContent.SetActive(true);
        InventoryHelmetContent.SetActive(false);
        InventoryArmorContent.SetActive(false);
        InventoryShoeContent.SetActive(false);

        InventoryScrollRect.content = InventoryWeaponContent.GetComponent<RectTransform>();
    }

    void InventoryHelmetButtonClicked()
    {
        InventoryWeaponContent.SetActive(false);
        InventoryHelmetContent.SetActive(true);
        InventoryArmorContent.SetActive(false);
        InventoryShoeContent.SetActive(false);

        InventoryScrollRect.content = InventoryHelmetContent.GetComponent<RectTransform>();
    }

    void InventoryArmorButtonClicked()
    {
        InventoryWeaponContent.SetActive(false);
        InventoryHelmetContent.SetActive(false);
        InventoryArmorContent.SetActive(true);
        InventoryShoeContent.SetActive(false);

        InventoryScrollRect.content = InventoryArmorContent.GetComponent<RectTransform>();
    }

    void InventoryShoeButtonClicked()
    {
        InventoryWeaponContent.SetActive(false);
        InventoryHelmetContent.SetActive(false);
        InventoryArmorContent.SetActive(false);
        InventoryShoeContent.SetActive(true);

        InventoryScrollRect.content = InventoryShoeContent.GetComponent<RectTransform>();
    }

    void UIItemSlotClicked(int number)
    {
        ItemNameText.text = UIItems[number].GetComponent<UIItem>().ItemData.ItemName;
        ItemDescriptionText.text = UIItems[number].GetComponent<UIItem>().ItemData.ItemDescription;
    }

    void UIEquipmentSlotClicked(int number)
    {
        ItemNameText.text = UIEquipments[number].GetComponent<UIEquipment>().EquipmentData.EquipmentName;
        ItemDescriptionText.text = UIEquipments[number].GetComponent<UIEquipment>().EquipmentData.EquipmentDescription;
    }

    void SortUIItems()
    {
        for(int i = 0; i < UIItems.Count; i++)
        {
            for(int j = i + 1; j < UIItems.Count; j++)
            {
                if(UIItems[i].GetComponent<UIItem>().ItemData.ItemID > UIItems[j].GetComponent<UIItem>().ItemData.ItemID)
                {
                    UIItems[i].transform.SetSiblingIndex(j);
                }
            }
        }
    }

    void SortUIEquipments()
    {
        for (int i = 0; i < UIEquipments.Count; i++)
        {
            for (int j = i + 1; j < UIEquipments.Count; j++)
            {
                if (UIEquipments[i].GetComponent<UIEquipment>().EquipmentData.EquipmentID
                    > UIEquipments[j].GetComponent<UIEquipment>().EquipmentData.EquipmentID)
                {
                    UIEquipments[i].transform.SetSiblingIndex(j);
                }
            }
        }
    }

    void InventoryExitButtonClicked()
    {
        UIManager.InventoryButton.gameObject.SetActive(true);
        InventoryItemContent.SetActive(true);
        InventoryWeaponContent.SetActive(false);
        InventoryHelmetContent.SetActive(false);
        InventoryArmorContent.SetActive(false);
        InventoryShoeContent.SetActive(false);

        InventoryScrollRect.content = InventoryItemContent.GetComponent<RectTransform>();

        gameObject.SetActive(false);

        ItemNameText.text = "";
        ItemDescriptionText.text = "";
    }
}
