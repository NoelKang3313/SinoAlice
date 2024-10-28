using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{ 
    public Button UIItem;
    public Button UIEquipment;
    public GameObject InventoryItemContent;
    public GameObject InventoryEquipmentContent;
    public GameObject ItemInfoPanel;
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI ItemDescriptionText;

    public ScrollRect InventoryScrollRect;
    public Button InventoryItemButton;
    public Button InventoryEquipmentButton;
    
    public List<Button> UIItems = new List<Button>();
    public List<Button> UIEquipments = new List<Button>();

    public Button InventoryExitButton;

    void Start()
    {
        InventoryItemButton.onClick.AddListener(InventoryItemButtonClicked);
        InventoryEquipmentButton.onClick.AddListener(InventoryEquipmentButtonClicked);

        InventoryExitButton.onClick.AddListener(InventoryExitButtonClicked);
    }

    void Update()
    {
        for(int i = 0; i < UIItems.Count; i++)
        {
            int number = i;

            UIItems[i].onClick.AddListener(() => UIItemSlotClicked(number));
        }

        SortUIItems();
        SortUIEquipments();
    }

    void InventoryItemButtonClicked()
    {
        InventoryItemContent.SetActive(true);
        InventoryEquipmentContent.SetActive(false);

        InventoryScrollRect.content = InventoryItemContent.GetComponent<RectTransform>();
    }

    void InventoryEquipmentButtonClicked()
    {
        InventoryItemContent.SetActive(false);
        InventoryEquipmentContent.SetActive(true);

        InventoryScrollRect.content = InventoryEquipmentContent.GetComponent<RectTransform>();
    }

    void UIItemSlotClicked(int number)
    {
        ItemNameText.text = UIItems[number].GetComponent<UIItem>().ItemData.ItemName;
        ItemDescriptionText.text = UIItems[number].GetComponent<UIItem>().ItemData.ItemDescription;
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
        InventoryItemContent.SetActive(true);
        InventoryEquipmentContent.SetActive(false);

        InventoryScrollRect.content = InventoryItemContent.GetComponent<RectTransform>();

        gameObject.SetActive(false);

        ItemNameText.text = "";
        ItemDescriptionText.text = "";
    }
}
