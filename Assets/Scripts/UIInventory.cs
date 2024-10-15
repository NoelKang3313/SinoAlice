using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{ 
    public Button UIItem;
    public GameObject InventoryContent;
    public GameObject ItemInfoPanel;

    public List<Button> UIItems = new List<Button>();

    void Update()
    {
        SortUIItem();
    }

    void SortUIItem()
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
}
