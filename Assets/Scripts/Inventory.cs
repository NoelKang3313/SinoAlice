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
}
