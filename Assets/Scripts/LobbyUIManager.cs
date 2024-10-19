using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public Inventory Inventory;
    public UIInventory UIInventory;

    public Image TransitionImage;

    public GameObject LobbyPanel;

    [Header("NPC Buttons")]
    public Button CharlotteButton;
    public Button WinryButton;
    public Button LidButton;

    public GameObject NPCPanel;
    public GameObject LidInteractImage;    
    public GameObject AliceInteractImage;

    [Header("Lid's")]
    public Button LidShopButton;

    public ScrollRect LidShopScrollRect;
    public GameObject LidItemShopContent;
    public GameObject LidWeaponShopContent;
    public GameObject LidHelmetShopContent;
    public GameObject LidArmorShopContent;
    public GameObject LidShoeShopContent;

    public Button LidItemShopButton;
    public Button LidEquipmentShopButton;
    public GameObject EquipmentCategories;
    public Button[] EquipmentButtons = new Button[4];

    public Button LidShopExitButton;
    public Button LidReturnButton;

    [Header("Winry's")]
    public Button WinryCharacterButton;
    public GameObject CharacterInfoPanel;
    public Button WinryReturnButton;

    [Header("Charlotte's")]
    public Button CharlotteWorldmapButton;
    public Button CharlotteReturnButton;

    [Header("Inventory")]
    public Button InventoryButton;
    public GameObject InventoryPanel;    

    [Header("Lid Shop")]
    public Button[] LidShopItemPurchaseButtons = new Button[7];
    public ItemData[] ItemDatas = new ItemData[7];

    void Start()
    {
        CharlotteButton.onClick.AddListener(CharlotteButtonClicked);
        WinryButton.onClick.AddListener(WinryButtonClicked);
        LidButton.onClick.AddListener(LidButtonClicked);

        LidShopButton.onClick.AddListener(LidShopButtonClicked);
        LidItemShopButton.onClick.AddListener(LidItemShopButtonClicked);
        LidEquipmentShopButton.onClick.AddListener(LidEquipmentShopButtonClicked);

        for(int i = 0; i < EquipmentButtons.Length; i++)
        {
            int number = i;

            EquipmentButtons[i].onClick.AddListener(() => EquipmentButtonClicked(number));
        }

        LidReturnButton.onClick.AddListener(LidReturnButtonClicked);

        WinryCharacterButton.onClick.AddListener(WinryCharacterButtonClicked);
        WinryReturnButton.onClick.AddListener(WinryReturnButtonClicked);

        CharlotteWorldmapButton.onClick.AddListener(CharlotteWorldmapButtonClicked);
        CharlotteReturnButton.onClick.AddListener(CharlotteReturnButtonClicked);

        LidShopExitButton.onClick.AddListener(LidShopExitButtonClicked);

        InventoryButton.onClick.AddListener(InventoryButtonClicked);        

        for (int i = 0; i < LidShopItemPurchaseButtons.Length; i++)
        {
            int number = i;

            LidShopItemPurchaseButtons[i].onClick.AddListener(() => LidShopItemPurchaseButtonClicked(number));
        }

        for (int i = 0; i < ItemDatas.Length; i++)
        {
            ItemDatas[i].ItemAmount = 0;
        }
    }

    void Update()
    {
        if(TransitionImage.fillAmount == 0)
        {
            LobbyPanel.SetActive(true);
        }
        else
        {
            LobbyPanel.SetActive(false);
        }

        ActivateTransition(1.0f);

        StartCoroutine(DelaySceneChange());
    }

    void ActivateTransition(float transitionSpeed)
    {
        if(GameManager.instance.isTransition)
        {
            TransitionImage.fillAmount += transitionSpeed / 1.0f * Time.deltaTime;
        }
        else
        {
            TransitionImage.fillAmount -= transitionSpeed / 1.0f * Time.deltaTime;
        }
    }

    // Move to Charlotte Position
    void CharlotteButtonClicked()
    {
        GameManager.instance.isCharlotteButtonActive = true;
        GameManager.instance.isAction = true;

        WinryButton.interactable = false;
        LidButton.interactable = false;
    }

    // Move to Winry Position
    void WinryButtonClicked()
    {
        GameManager.instance.isWinryButtonActive = true;
        GameManager.instance.isAction = true;

        CharlotteButton.interactable = false;
        LidButton.interactable = false;
    }

    // Move to Lid Position
    void LidButtonClicked()
    {
        GameManager.instance.isLidButtonActive = true;
        GameManager.instance.isAction = true;

        CharlotteButton.interactable = false;
        WinryButton.interactable = false;
    }

    // Enter Shop
    void LidShopButtonClicked()
    {
        NPCPanel.SetActive(true);
        AliceInteractImage.SetActive(true);
        LidInteractImage.SetActive(true);

        LidShopScrollRect.content = LidItemShopContent.GetComponent<RectTransform>();

        LidItemShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;        
    }

    // Click Item Tab
    void LidItemShopButtonClicked()
    {
        LidShopScrollRect.content = LidItemShopContent.GetComponent<RectTransform>();

        LidItemShopContent.SetActive(true);
        LidWeaponShopContent.SetActive(false);
        LidHelmetShopContent.SetActive(false);
        LidArmorShopContent.SetActive(false);
        LidShoeShopContent.SetActive(false);

        EquipmentCategories.SetActive(false);

        LidItemShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    // Click Equipment Tab
    void LidEquipmentShopButtonClicked()
    {
        LidShopScrollRect.content = LidWeaponShopContent.GetComponent<RectTransform>();
        EquipmentCategories.SetActive(true);

        LidItemShopContent.SetActive(false);
        LidWeaponShopContent.SetActive(true);        

        LidWeaponShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    // Click Equipment Category Buttons
    void EquipmentButtonClicked(int number)
    {
        switch(number)
        {
            case 0:
                LidWeaponShopContent.SetActive(true);
                LidHelmetShopContent.SetActive(false);
                LidArmorShopContent.SetActive(false);
                LidShoeShopContent.SetActive(false);

                LidShopScrollRect.content = LidWeaponShopContent.GetComponent<RectTransform>();
                LidWeaponShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;

            case 1:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(true);
                LidArmorShopContent.SetActive(false);
                LidShoeShopContent.SetActive(false);

                LidShopScrollRect.content = LidHelmetShopContent.GetComponent<RectTransform>();
                LidHelmetShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;
                
            case 2:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(false);
                LidArmorShopContent.SetActive(true);
                LidShoeShopContent.SetActive(false);

                LidShopScrollRect.content = LidArmorShopContent.GetComponent<RectTransform>();
                LidArmorShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;

            case 3:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(false);
                LidArmorShopContent.SetActive(false);
                LidShoeShopContent.SetActive(true);

                LidShopScrollRect.content = LidShoeShopContent.GetComponent<RectTransform>();
                LidShoeShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;
        }
    }

    // Return Original Position
    void LidReturnButtonClicked()
    {         
        LidShopButton.gameObject.SetActive(false);
        LidReturnButton.gameObject.SetActive(false);

        GameManager.instance.isReturnButtonActive = true;
    }

    // Exit Shop
    void LidShopExitButtonClicked()
    {
        NPCPanel.SetActive(false);
        AliceInteractImage.SetActive(false);
        LidInteractImage.SetActive(false);

        LidItemShopContent.SetActive(true);
        LidWeaponShopContent.SetActive(false);
        LidHelmetShopContent.SetActive(false);
        LidArmorShopContent.SetActive(false);
        LidShoeShopContent.SetActive(false);
        EquipmentCategories.SetActive(false);
    }

    // Purchase Item
    void LidShopItemPurchaseButtonClicked(int number)
    {
        ItemData purchasedItem = ItemDatas[number];
        Inventory.Items.Add(purchasedItem);
        purchasedItem.ItemAmount++;

        CheckItem();

        Button ItemSlot = Instantiate(UIInventory.UIItem, UIInventory.InventoryItemContent.transform.position, Quaternion.identity);
        ItemSlot.transform.SetParent(UIInventory.InventoryItemContent.transform);

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

    void InventoryButtonClicked()
    {
        InventoryPanel.SetActive(true);
    }

    // Enter Character Info
    void WinryCharacterButtonClicked()
    {
        CharacterInfoPanel.SetActive(true);
    }

    // Return Original Position
    void WinryReturnButtonClicked()
    {
        NPCPanel.SetActive(false);
        AliceInteractImage.SetActive(false);

        WinryCharacterButton.gameObject.SetActive(false);
        WinryReturnButton.gameObject.SetActive(false);

        GameManager.instance.isReturnButtonActive = true;
    }

    // Enter Worldmap Scene
    void CharlotteWorldmapButtonClicked()
    {
        GameManager.instance.isTransition = true;
        GameManager.instance.isCharlotteButtonActive = false;
    }

    // Return Original Position
    void CharlotteReturnButtonClicked()
    {
        CharlotteWorldmapButton.gameObject.SetActive(false);
        CharlotteReturnButton.gameObject.SetActive(false);

        GameManager.instance.isReturnButtonActive = true;
    }

    IEnumerator DelaySceneChange()
    {
        if(TransitionImage.fillAmount == 1.0f)
        {
            yield return new WaitForSeconds(2.0f);

            GameManager.instance.isTransition = false;
            GameManager.instance.LoadScene("Worldmap");
        }
    }
}
