using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public Inventory Inventory;
    public UIInventory UIInventory;

    public Button[] LidShopButtons = new Button[7];
    public ItemData[] ItemDatas = new ItemData[7];

    void Start()
    {
        for(int i = 0; i < LidShopButtons.Length; i++)
        {
            int number = i;

            LidShopButtons[i].onClick.AddListener(() => LidShopButtonClicked(number));
        }

        for(int i = 0; i < ItemDatas.Length; i++)
        {
            ItemDatas[i].ItemAmount = 0;
        }
    }

    void LidShopButtonClicked(int number)
    {
        ItemData purchasedItem = ItemDatas[number];
        Inventory.Items.Add(purchasedItem);
        purchasedItem.ItemAmount++;

        CheckItem();

        Button ItemSlot = Instantiate(UIInventory.UIItem, UIInventory.InventoryContent.transform.position, Quaternion.identity);
        ItemSlot.transform.SetParent(UIInventory.InventoryContent.transform);

        ItemSlot.GetComponent<UIItem>().ItemImage.sprite = purchasedItem.ItemSprite;
        ItemSlot.GetComponent<UIItem>().ItemAmount.text = purchasedItem.ItemAmount.ToString();
        ItemSlot.GetComponent<UIItem>().ItemData = purchasedItem;

        UIInventory.UIItems.Add(ItemSlot);

        CheckUIItem();
    }

    void CheckItem()
    {
        for (int i = 0; i < Inventory.Items.Count; i++)
        {
            for (int j = i + 1; j < Inventory.Items.Count; j++)
            {
                if (Inventory.Items[i] == Inventory.Items[j])
                {
                    Inventory.Items.Remove(Inventory.Items[j]);
                }
            }
        }
    }

    void CheckUIItem()
    {
        for (int i = 0; i < UIInventory.UIItems.Count; i++)
        {
            for (int j = i + 1; j < UIInventory.UIItems.Count; j++)
            {
                if (UIInventory.UIItems[i].GetComponent<UIItem>().ItemData.ItemID == UIInventory.UIItems[j].GetComponent<UIItem>().ItemData.ItemID)
                {                    
                    Destroy(UIInventory.UIItems[i].gameObject);
                    UIInventory.UIItems.Remove(UIInventory.UIItems[i]);
                }
            }
        }
    }
}
