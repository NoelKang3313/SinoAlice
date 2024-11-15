using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public Inventory Inventory;
    public UIInventory UIInventory;
    public UICharacterInfo UICharacterInfo;

    public AudioSource SystemAudioSource;
    public AudioClip ButtonAudioClip;
    public AudioClip ConfirmAudioClip;
    public AudioClip CancelAudioClip;

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
    public GameObject LidShopViewport;
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
    public Button CharlotteConfirmButton;
    public Button CharlotteCancelButton;

    [Header("Inventory")]
    public Button InventoryButton;
    public GameObject InventoryPanel;

    [Header("Lid Shop")]
    public Button[] LidShopItemPurchaseButtons = new Button[7];
    public ItemData[] ItemDatas = new ItemData[7];
    public Button[] LidShopWeaponPurchaseButtons = new Button[10];
    public EquipmentData[] WeaponDatas = new EquipmentData[10];
    public Button[] LidShopHelmetPurchaseButtons = new Button[10];
    public EquipmentData[] HelmetDatas = new EquipmentData[10];
    public Button[] LidShopArmorPurchaseButtons = new Button[10];
    public EquipmentData[] ArmorDatas = new EquipmentData[10];
    public Button[] LidShopShoePurchaseButtons = new Button[10];
    public EquipmentData[] ShoeDatas = new EquipmentData[10];
    private bool isItemPurchaseButtonPressed;
    private bool isEquipmentPurchaseButtonPresed;
    [SerializeField] private ItemData purchaseItemData;
    [SerializeField] private EquipmentData purchaseEquipmentData;
    public GameObject PurchasePanel;
    public Button PurchaseConfirmButton;
    public Button PurchaseCancelButton;

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
        CharlotteConfirmButton.onClick.AddListener(CharlotteConfirmButtonClicked);
        CharlotteCancelButton.onClick.AddListener(CharlotteCancelButtonClicked);

        LidShopExitButton.onClick.AddListener(LidShopExitButtonClicked);

        InventoryButton.onClick.AddListener(InventoryButtonClicked);        

        for (int i = 0; i < LidShopItemPurchaseButtons.Length; i++)
        {
            int number = i;

            LidShopItemPurchaseButtons[i].onClick.AddListener(() => LidShopItemPurchaseButtonClicked(number));
        }

        for(int i = 0; i < 10; i++)
        {
            int number = i;

            LidShopWeaponPurchaseButtons[i].onClick.AddListener(() => LidShopWeaponPurchaseButtonClicked(number));
            LidShopHelmetPurchaseButtons[i].onClick.AddListener(() => LidShopHelmetPurchaseButtonClicked(number));
            LidShopArmorPurchaseButtons[i].onClick.AddListener(() => LidShopArmorPurchaseButtonClicked(number));
            LidShopShoePurchaseButtons[i].onClick.AddListener(() => LidShopShoePurchaseButtonClicked(number));
        }

        for (int i = 0; i < ItemDatas.Length; i++)
        {
            ItemDatas[i].ItemAmount = 0;
        }

        PurchaseConfirmButton.onClick.AddListener(PurchaseConfirmButtonClicked);
        PurchaseCancelButton.onClick.AddListener(PurchaseCancelButtonClicked);
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
        
        LidShopViewport.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
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

        LidShopViewport.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        LidItemShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        SystemAudioSource.PlayOneShot(ButtonAudioClip);
    }

    // Click Equipment Tab
    void LidEquipmentShopButtonClicked()
    {
        LidShopScrollRect.content = LidWeaponShopContent.GetComponent<RectTransform>();
        EquipmentCategories.SetActive(true);

        LidItemShopContent.SetActive(false);
        LidWeaponShopContent.SetActive(true);

        LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
        LidWeaponShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        SystemAudioSource.PlayOneShot(ButtonAudioClip);
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
                LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
                LidWeaponShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                SystemAudioSource.PlayOneShot(ButtonAudioClip);

                break;

            case 1:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(true);
                LidArmorShopContent.SetActive(false);
                LidShoeShopContent.SetActive(false);

                LidShopScrollRect.content = LidHelmetShopContent.GetComponent<RectTransform>();
                LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
                LidHelmetShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                SystemAudioSource.PlayOneShot(ButtonAudioClip);

                break;
                
            case 2:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(false);
                LidArmorShopContent.SetActive(true);
                LidShoeShopContent.SetActive(false);

                LidShopScrollRect.content = LidArmorShopContent.GetComponent<RectTransform>();
                LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
                LidArmorShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                SystemAudioSource.PlayOneShot(ButtonAudioClip);

                break;

            case 3:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(false);
                LidArmorShopContent.SetActive(false);
                LidShoeShopContent.SetActive(true);

                LidShopScrollRect.content = LidShoeShopContent.GetComponent<RectTransform>();
                LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
                LidShoeShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                SystemAudioSource.PlayOneShot(ButtonAudioClip);

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
        isItemPurchaseButtonPressed = true;
        isEquipmentPurchaseButtonPresed = false;

        PurchasePanel.SetActive(true);

        purchaseItemData = ItemDatas[number];

        SystemAudioSource.PlayOneShot(ButtonAudioClip);
    }    

    // Purchase Weapon
    void LidShopWeaponPurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = false;
        isEquipmentPurchaseButtonPresed = true;

        PurchasePanel.SetActive(true);

        purchaseEquipmentData = WeaponDatas[number];

        SystemAudioSource.PlayOneShot(ButtonAudioClip);
    }

    // Purchase Helmet
    void LidShopHelmetPurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = false;
        isEquipmentPurchaseButtonPresed = true;

        PurchasePanel.SetActive(true);

        purchaseEquipmentData = HelmetDatas[number];

        SystemAudioSource.PlayOneShot(ButtonAudioClip);
    }

    // Purchase Armor
    void LidShopArmorPurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = false;
        isEquipmentPurchaseButtonPresed = true;

        PurchasePanel.SetActive(true);

        purchaseEquipmentData = ArmorDatas[number];

        SystemAudioSource.PlayOneShot(ButtonAudioClip);
    }

    // Purchase Shoe
    void LidShopShoePurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = false;
        isEquipmentPurchaseButtonPresed = true;

        PurchasePanel.SetActive(true);

        purchaseEquipmentData = ShoeDatas[number];

        SystemAudioSource.PlayOneShot(ButtonAudioClip);
    }

    // Confirm purchasing
    void PurchaseConfirmButtonClicked()
    {
        if (isItemPurchaseButtonPressed && !isEquipmentPurchaseButtonPresed)
        {
            Inventory.Items.Add(purchaseItemData);
            purchaseItemData.ItemAmount++;

            CheckItem();

            Button ItemSlot = Instantiate(UIInventory.UIItem, UIInventory.InventoryItemContent.transform.position, Quaternion.identity);
            ItemSlot.transform.SetParent(UIInventory.InventoryItemContent.transform);

            ItemSlot.GetComponent<UIItem>().ItemImage.sprite = purchaseItemData.ItemSprite;
            ItemSlot.GetComponent<UIItem>().ItemAmount.text = purchaseItemData.ItemAmount.ToString();
            ItemSlot.GetComponent<UIItem>().ItemData = purchaseItemData;

            UIInventory.UIItems.Add(ItemSlot);

            CheckUIItem();

            PurchasePanel.SetActive(false);
        }
        else if (!isItemPurchaseButtonPressed && isEquipmentPurchaseButtonPresed)
        {
            if(purchaseEquipmentData.EquipmentCategory == "Weapon")
            {
                Inventory.Weapons.Add(purchaseEquipmentData);
                purchaseEquipmentData.EquipmentAmount++;

                CheckEquipment();

                Button EquipmentSlot = Instantiate(UIInventory.UIEquipment, UIInventory.InventoryWeaponContent.transform.position, Quaternion.identity);
                EquipmentSlot.transform.SetParent(UIInventory.InventoryWeaponContent.transform);

                EquipmentSlot.GetComponent<UIEquipment>().EquipmentImage.sprite = purchaseEquipmentData.EquipmentSprite;
                EquipmentSlot.GetComponent<UIEquipment>().EquipmentAmountText.text = purchaseEquipmentData.EquipmentAmount.ToString();
                EquipmentSlot.GetComponent<UIEquipment>().EquipmentData = purchaseEquipmentData;

                Button CEquipmentSlot = Instantiate(UICharacterInfo.UICEquipment, UICharacterInfo.WeaponsContent.transform.position, Quaternion.identity);
                CEquipmentSlot.transform.SetParent(UICharacterInfo.WeaponsContent.transform);

                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = purchaseEquipmentData.EquipmentSprite;
                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = purchaseEquipmentData;
                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = purchaseEquipmentData.EquipmentAmount;

                UIInventory.UIEquipments.Add(EquipmentSlot);
                UICharacterInfo.UIWeapons.Add(CEquipmentSlot);

                CheckUIEquipment();

                PurchasePanel.SetActive(false);
            }
            else if(purchaseEquipmentData.EquipmentCategory == "Helmet")
            {
                Inventory.Helmets.Add(purchaseEquipmentData);
                purchaseEquipmentData.EquipmentAmount++;

                CheckEquipment();

                Button EquipmentSlot = Instantiate(UIInventory.UIEquipment, UIInventory.InventoryHelmetContent.transform.position, Quaternion.identity);
                EquipmentSlot.transform.SetParent(UIInventory.InventoryHelmetContent.transform);

                EquipmentSlot.GetComponent<UIEquipment>().EquipmentImage.sprite = purchaseEquipmentData.EquipmentSprite;
                EquipmentSlot.GetComponent<UIEquipment>().EquipmentAmountText.text = purchaseEquipmentData.EquipmentAmount.ToString();
                EquipmentSlot.GetComponent<UIEquipment>().EquipmentData = purchaseEquipmentData;

                Button CEquipmentSlot = Instantiate(UICharacterInfo.UICEquipment, UICharacterInfo.HelmetsContent.transform.position, Quaternion.identity);
                CEquipmentSlot.transform.SetParent(UICharacterInfo.HelmetsContent.transform);

                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = purchaseEquipmentData.EquipmentSprite;
                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = purchaseEquipmentData;
                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = purchaseEquipmentData.EquipmentAmount;

                UIInventory.UIEquipments.Add(EquipmentSlot);
                UICharacterInfo.UIHelmets.Add(CEquipmentSlot);

                CheckUIEquipment();

                PurchasePanel.SetActive(false);
            }
            else if(purchaseEquipmentData.EquipmentCategory == "Armor")
            {
                Inventory.Armors.Add(purchaseEquipmentData);
                purchaseEquipmentData.EquipmentAmount++;

                CheckEquipment();

                Button EquipmentSlot = Instantiate(UIInventory.UIEquipment, UIInventory.InventoryArmorContent.transform.position, Quaternion.identity);
                EquipmentSlot.transform.SetParent(UIInventory.InventoryArmorContent.transform);

                EquipmentSlot.GetComponent<UIEquipment>().EquipmentImage.sprite = purchaseEquipmentData.EquipmentSprite;
                EquipmentSlot.GetComponent<UIEquipment>().EquipmentAmountText.text = purchaseEquipmentData.EquipmentAmount.ToString();
                EquipmentSlot.GetComponent<UIEquipment>().EquipmentData = purchaseEquipmentData;

                Button CEquipmentSlot = Instantiate(UICharacterInfo.UICEquipment, UICharacterInfo.ArmorsContent.transform.position, Quaternion.identity);
                CEquipmentSlot.transform.SetParent(UICharacterInfo.ArmorsContent.transform);

                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = purchaseEquipmentData.EquipmentSprite;
                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = purchaseEquipmentData;
                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = purchaseEquipmentData.EquipmentAmount;

                UIInventory.UIEquipments.Add(EquipmentSlot);
                UICharacterInfo.UIArmors.Add(CEquipmentSlot);

                CheckUIEquipment();

                PurchasePanel.SetActive(false);
            }
            else if(purchaseEquipmentData.EquipmentCategory == "Shoe")
            {
                Inventory.Shoes.Add(purchaseEquipmentData);
                purchaseEquipmentData.EquipmentAmount++;

                CheckEquipment();

                Button EquipmentSlot = Instantiate(UIInventory.UIEquipment, UIInventory.InventoryShoeContent.transform.position, Quaternion.identity);
                EquipmentSlot.transform.SetParent(UIInventory.InventoryShoeContent.transform);

                EquipmentSlot.GetComponent<UIEquipment>().EquipmentImage.sprite = purchaseEquipmentData.EquipmentSprite;
                EquipmentSlot.GetComponent<UIEquipment>().EquipmentAmountText.text = purchaseEquipmentData.EquipmentAmount.ToString();
                EquipmentSlot.GetComponent<UIEquipment>().EquipmentData = purchaseEquipmentData;

                Button CEquipmentSlot = Instantiate(UICharacterInfo.UICEquipment, UICharacterInfo.ShoesContent.transform.position, Quaternion.identity);
                CEquipmentSlot.transform.SetParent(UICharacterInfo.ShoesContent.transform);

                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = purchaseEquipmentData.EquipmentSprite;
                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = purchaseEquipmentData;
                CEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = purchaseEquipmentData.EquipmentAmount;

                UIInventory.UIEquipments.Add(EquipmentSlot);
                UICharacterInfo.UIShoes.Add(CEquipmentSlot);

                CheckUIEquipment();

                PurchasePanel.SetActive(false);
            }
        }

        SystemAudioSource.PlayOneShot(ConfirmAudioClip);
    }

    // Cancel purchasing
    void PurchaseCancelButtonClicked()
    {
        PurchasePanel.SetActive(false);

        SystemAudioSource.PlayOneShot(CancelAudioClip);
    }

    // Check if current item is already purchased, if so, remove item
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

    // Check if current item is already purchased, if so, remove item UI
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

    // Check if current equipment is already purchased, if so, remove equipment
    void CheckEquipment()
    {
        for (int i = 0; i < Inventory.Weapons.Count; i++)
        {
            for (int j = i + 1; j < Inventory.Weapons.Count; j++)
            {
                if (Inventory.Weapons[i] == Inventory.Weapons[j])
                {
                    Inventory.Weapons.Remove(Inventory.Weapons[j]);
                }
            }
        }

        for (int i = 0; i < Inventory.Helmets.Count; i++)
        {
            for (int j = i + 1; j < Inventory.Helmets.Count; j++)
            {
                if (Inventory.Helmets[i] == Inventory.Helmets[j])
                {
                    Inventory.Helmets.Remove(Inventory.Helmets[j]);
                }
            }
        }

        for (int i = 0; i < Inventory.Armors.Count; i++)
        {
            for (int j = i + 1; j < Inventory.Armors.Count; j++)
            {
                if (Inventory.Armors[i] == Inventory.Armors[j])
                {
                    Inventory.Armors.Remove(Inventory.Armors[j]);
                }
            }
        }
    }

    // Check if current equipment is already purchased, if so, remove equipment UI
    void CheckUIEquipment()
    {
        for (int i = 0; i < UIInventory.UIEquipments.Count; i++)
        {
            for (int j = i + 1; j < UIInventory.UIEquipments.Count; j++)
            {
                if (UIInventory.UIEquipments[i].GetComponent<UIEquipment>().EquipmentData.EquipmentID
                    == UIInventory.UIEquipments[j].GetComponent<UIEquipment>().EquipmentData.EquipmentID)
                {
                    Destroy(UIInventory.UIEquipments[i].gameObject);
                    UIInventory.UIEquipments.Remove(UIInventory.UIEquipments[i]);
                }
            }
        }

        for (int i = 0; i < UICharacterInfo.UIWeapons.Count; i++)
        {
            for (int j = i + 1; j < UICharacterInfo.UIWeapons.Count; j++)
            {
                if (UICharacterInfo.UIWeapons[i].GetComponent<UICEquipment>().EquipmentData.EquipmentID
                    == UICharacterInfo.UIWeapons[j].GetComponent<UICEquipment>().EquipmentData.EquipmentID)
                {
                    Destroy(UICharacterInfo.UIWeapons[i].gameObject);
                    UICharacterInfo.UIWeapons.Remove(UICharacterInfo.UIWeapons[i]);
                }
            }
        }

        for (int i = 0; i < UICharacterInfo.UIHelmets.Count; i++)
        {
            for (int j = i + 1; j < UICharacterInfo.UIHelmets.Count; j++)
            {
                if (UICharacterInfo.UIHelmets[i].GetComponent<UICEquipment>().EquipmentData.EquipmentID
                    == UICharacterInfo.UIHelmets[j].GetComponent<UICEquipment>().EquipmentData.EquipmentID)
                {
                    Destroy(UICharacterInfo.UIHelmets[i].gameObject);
                    UICharacterInfo.UIHelmets.Remove(UICharacterInfo.UIHelmets[i]);
                }
            }
        }

        for (int i = 0; i < UICharacterInfo.UIArmors.Count; i++)
        {
            for (int j = i + 1; j < UICharacterInfo.UIArmors.Count; j++)
            {
                if (UICharacterInfo.UIArmors[i].GetComponent<UICEquipment>().EquipmentData.EquipmentID
                    == UICharacterInfo.UIArmors[j].GetComponent<UICEquipment>().EquipmentData.EquipmentID)
                {
                    Destroy(UICharacterInfo.UIArmors[i].gameObject);
                    UICharacterInfo.UIArmors.Remove(UICharacterInfo.UIArmors[i]);
                }
            }
        }

        for (int i = 0; i < UICharacterInfo.UIShoes.Count; i++)
        {
            for (int j = i + 1; j < UICharacterInfo.UIShoes.Count; j++)
            {
                if (UICharacterInfo.UIShoes[i].GetComponent<UICEquipment>().EquipmentData.EquipmentID
                    == UICharacterInfo.UIShoes[j].GetComponent<UICEquipment>().EquipmentData.EquipmentID)
                {
                    Destroy(UICharacterInfo.UIShoes[i].gameObject);
                    UICharacterInfo.UIShoes.Remove(UICharacterInfo.UIShoes[i]);
                }
            }
        }
    }

    // Enter Inventory
    void InventoryButtonClicked()
    {
        InventoryPanel.SetActive(true);
        UIInventory.InventoryItemContent.SetActive(true);
        UIInventory.EquipmentCategories.SetActive(false);
        UIInventory.InventoryWeaponContent.SetActive(false);
        UIInventory.InventoryHelmetContent.SetActive(false);
        UIInventory.InventoryArmorContent.SetActive(false);
        UIInventory.InventoryShoeContent.SetActive(false);
    }

    // Enter Character Info
    void WinryCharacterButtonClicked()
    {
        UICharacterInfo.ResetEquipmentButtons();
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

    // Confirm or Cancel Button Activate (Entering Worldmap Scene)
    void CharlotteWorldmapButtonClicked()
    {
        CharlotteConfirmButton.gameObject.SetActive(true);
        CharlotteCancelButton.gameObject.SetActive(true);
    }

    // Return Original Position
    void CharlotteReturnButtonClicked()
    {
        CharlotteWorldmapButton.gameObject.SetActive(false);
        CharlotteReturnButton.gameObject.SetActive(false);

        GameManager.instance.isReturnButtonActive = true;
    }

    // Confirm (Enter Worldmap Scene)
    void CharlotteConfirmButtonClicked()
    {
        GameManager.instance.isTransition = true;
        GameManager.instance.isCharlotteButtonActive = false;
    }

    // Cancel (Deactivate Confirm & Cancel Button)
    void CharlotteCancelButtonClicked()
    {
        CharlotteConfirmButton.gameObject.SetActive(false);
        CharlotteCancelButton.gameObject.SetActive(false);
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
