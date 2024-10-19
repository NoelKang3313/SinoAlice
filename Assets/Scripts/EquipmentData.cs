using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Equipment Data")]
public class EquipmentData : ScriptableObject
{
    public int EquipmentID;
    public string EquipmentName;
    public Sprite EquipmentSprite;
    public string EquipmentDescription;
    public int EquipmentAmount;
    public string EquipmentCategory;
    public int Attack;
    public int Magic;
    public int Defense;
    public int Health;
    public int Speed;

    public void ResetEquipmentAmount()
    {
        EquipmentAmount = 0;
    }

    //Reset Item Amount when editor start playing
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnPlayModeStart()
    {
        var assets = Resources.FindObjectsOfTypeAll<EquipmentData>();

        foreach (var asset in assets)
        {
            asset.ResetEquipmentAmount();
        }
    }
}
