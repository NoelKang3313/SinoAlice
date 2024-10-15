using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemData> Items = new List<ItemData>();

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
