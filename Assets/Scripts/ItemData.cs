using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item Data")]
public class ItemData : ScriptableObject
{
    public int ItemID;
    public string ItemName;
    public Sprite ItemSprite;
    public string ItemDescription;
    public int ItemAmount;

    public void ResetItemAmount()
    {
        ItemAmount = 0;
    }

    //Reset Item Amount when editor start playing
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnPlayModeStart()
    {
        var assets = Resources.FindObjectsOfTypeAll<ItemData>();

        foreach(var asset in assets)
        {
            asset.ResetItemAmount();
        }
    }
}
