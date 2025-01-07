using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemData> Items = new List<ItemData>();
    public List<EquipmentData> Weapons = new List<EquipmentData>();
    public List<EquipmentData> Helmets = new List<EquipmentData>();
    public List<EquipmentData> Armors = new List<EquipmentData>();
    public List<EquipmentData> Shoes = new List<EquipmentData>();

    public List<int> ItemAmount = new List<int>();

    void Update()
    {
        SortByItemID();
    }

    void SortByItemID()
    {
        if(Items.Count > 1)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                for(int j = i + 1; j < Items.Count; j++)
                {
                    if(Items[i].ItemID > Items[j].ItemID)
                    {
                        ItemData tempItemData = Items[i];
                        int tempAmount = ItemAmount[i];

                        Items[i] = Items[j];
                        ItemAmount[i] = ItemAmount[j];

                        Items[j] = tempItemData;
                        ItemAmount[j] = tempAmount;
                    }
                }
            }
        }
    }
}
