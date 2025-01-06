using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    public LobbyAudioManager AudioManager;

    public Inventory Inventory;
    public UIInventory UIInventory;
    public UICharacterInfo UICharacterInfo;    

    [Header("Character Prefab")]
    public GameObject AlicePrefab;
    public GameObject GretelPrefab;
    public GameObject SnowWhitePrefab;

    public Image TransitionImage;

    public GameObject LobbyPanel;

    [Header("NPC Buttons")]
    public GameObject NPCButtons;
    public Button CharlotteButton;
    public Button WinryButton;
    public Button LidButton;

    [Header("NPC Dialogue UI")]
    public GameObject NPCDialoguePanel;
    public Animator NPCDialogueAnimator;
    public Button NPCButton;
    public TextMeshProUGUI NPCButtonText;
    public Button ReturnButton;
    public Image NPCImage;
    public TextMeshProUGUI NPCDialogueText;
    public TextMeshProUGUI NPCNameText;

    public GameObject ShopPanel;
    public GameObject LidShopImage;
    public GameObject AliceShopImage;

    [Header("Lid's")]
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

    public Animator LidBubble;
    public TextMeshProUGUI LidBubbleText;
    public Animator AliceBubble;

    public Button LidShopExitButton;    

    [Header("Winry's")]    
    public GameObject CharacterInfoPanel;

    [Header("Charlotte's")]
    [SerializeField]
    private bool isWorldmapButtonClicked;

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
    public Image PurchasePanelItemImage;
    public TextMeshProUGUI PurchasePanelItemName;
    public TextMeshProUGUI PurchasePanelItemCostText;
    public Button PurchaseConfirmButton;
    public Button PurchaseCancelButton;

    public TextMeshProUGUI GaldText;

    [Header("Alice Gauge")]
    public Image AliceHPGauge;
    public Image AliceMPGauge;
    public TextMeshProUGUI AliceHPText;
    public TextMeshProUGUI AliceMPText;

    [Header("Gretel Gauge")]
    public Image GretelHPGauge;
    public Image GretelMPGauge;
    public TextMeshProUGUI GretelHPText;
    public TextMeshProUGUI GretelMPText;

    [Header("Snow White Gauge")]
    public Image SWHPGauge;
    public Image SWMPGauge;
    public TextMeshProUGUI SWHPText;
    public TextMeshProUGUI SWMPText;

    void Start()
    {
        Inventory = FindObjectOfType<Inventory>();

        GaldText.text = GameManager.instance.Gald.ToString("#,##0");

        CharlotteButton.onClick.AddListener(CharlotteButtonClicked);
        WinryButton.onClick.AddListener(WinryButtonClicked);
        LidButton.onClick.AddListener(LidButtonClicked);
        
        LidItemShopButton.onClick.AddListener(LidItemShopButtonClicked);
        LidEquipmentShopButton.onClick.AddListener(LidEquipmentShopButtonClicked);

        NPCButton.onClick.AddListener(NPCButtonClicked);
        ReturnButton.onClick.AddListener(ReturnButtonClicked);

        for(int i = 0; i < EquipmentButtons.Length; i++)
        {
            int number = i;

            EquipmentButtons[i].onClick.AddListener(() => EquipmentButtonClicked(number));
        }

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

        PurchaseConfirmButton.onClick.AddListener(PurchaseConfirmButtonClicked);
        PurchaseCancelButton.onClick.AddListener(PurchaseCancelButtonClicked);

        SetGaugeUI();
        SetUIInventory();
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


        if (LidBubble.gameObject.activeSelf && LidBubble.GetCurrentAnimatorStateInfo(0).IsName("Bubble Image") && LidBubble.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            LidBubble.SetBool("isActive", false);
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

    void NPCButtonClicked()
    {
        if(GameManager.instance.isLidButtonActive)
        {
            NPCDialogueAnimator.SetBool("isActive", false);
            ShopPanel.SetActive(true);
            InventoryButton.gameObject.SetActive(false);
            AliceShopImage.SetActive(true);
            LidShopImage.SetActive(true);

            LidShopImage.GetComponent<Animator>().Play("Lid_Idle");

            LidShopScrollRect.content = LidItemShopContent.GetComponent<RectTransform>();

            LidShopViewport.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
 
            AudioManager.NPCAudioSource.clip = AudioManager.LidEnterShopClip;
            AudioManager.NPCAudioSource.Play();

            LidBubble.gameObject.SetActive(true);
            LidBubble.SetBool("isActive", true);

            LidBubbleText.text = "무엇??" + System.Environment.NewLine + "구매?�시겠습?�까?";

            AliceBubble.gameObject.SetActive(true);
            AliceBubble.Play("Bubble Active");
        }
        else if(GameManager.instance.isWinryButtonActive)
        {
            NPCDialogueAnimator.SetBool("isActive", false);

            UICharacterInfo.ResetEquipmentButtons();
            CharacterInfoPanel.SetActive(true);
        }
        else if(GameManager.instance.isCharlotteButtonActive)
        {
            if(!isWorldmapButtonClicked)
            {
                isWorldmapButtonClicked = true;
                NPCDialogueText.text = "준비되?�습?�까?";
                NPCButtonText.text = "?�장";
            }
            else
            {
                AudioManager.NPCAudioSource.clip = AudioManager.CharlotteEnterWorldClip;
                AudioManager.NPCAudioSource.Play();

                GameManager.instance.isTransition = true;
                GameManager.instance.isCharlotteButtonActive = false;
                isWorldmapButtonClicked = false;
            }
        }        
    }

    void ReturnButtonClicked()
    {
        if(isWorldmapButtonClicked)
        {
            NPCButtonText.text = "?�드�?";
            NPCDialogueText.text = "?�서?�십?�오, 기다리고 ?�었?�니??";
            isWorldmapButtonClicked = false;
        }
        else
        {
            NPCDialogueAnimator.SetBool("isActive", false);

            GameManager.instance.isReturnButtonActive = true;
            InventoryButton.interactable = true;

            if(GameManager.instance.isLidButtonActive)
            {
                AudioManager.NPCAudioSource.clip = AudioManager.LidReturnClip;
                AudioManager.NPCAudioSource.Play();
            }
            else if(GameManager.instance.isWinryButtonActive)
            {
                AudioManager.NPCAudioSource.clip = AudioManager.WinryReturnClip;
                AudioManager.NPCAudioSource.Play();
            }
            else if(GameManager.instance.isCharlotteButtonActive)
            {
                AudioManager.NPCAudioSource.clip = AudioManager.CharlotteReturnClip;
                AudioManager.NPCAudioSource.Play();
            }
        }
    }

    // Move to Charlotte Position
    void CharlotteButtonClicked()
    {
        NPCButtons.SetActive(false);

        GameManager.instance.isCharlotteButtonActive = true;
        GameManager.instance.isAction = true;

        InventoryButton.interactable = false;
    }

    // Move to Winry Position
    void WinryButtonClicked()
    {
        NPCButtons.SetActive(false);

        GameManager.instance.isWinryButtonActive = true;
        GameManager.instance.isAction = true;

        InventoryButton.interactable = false;
    }

    // Move to Lid Position
    void LidButtonClicked()
    {
        NPCButtons.SetActive(false);

        GameManager.instance.isLidButtonActive = true;
        GameManager.instance.isAction = true;

        InventoryButton.interactable = false;
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
    }

    // Click Equipment Tab
    void LidEquipmentShopButtonClicked()
    {
        LidShopScrollRect.content = LidWeaponShopContent.GetComponent<RectTransform>();
        EquipmentCategories.SetActive(true);

        LidItemShopContent.SetActive(false);

        if(LidWeaponShopContent.activeSelf || LidHelmetShopContent.activeSelf || LidArmorShopContent.activeSelf || LidShoeShopContent.activeInHierarchy)
        {
            return;
        }
        else
        {
            LidWeaponShopContent.SetActive(true);
        }

        LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
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
                LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
                LidWeaponShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;

            case 1:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(true);
                LidArmorShopContent.SetActive(false);
                LidShoeShopContent.SetActive(false);

                LidShopScrollRect.content = LidHelmetShopContent.GetComponent<RectTransform>();
                LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
                LidHelmetShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;
                
            case 2:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(false);
                LidArmorShopContent.SetActive(true);
                LidShoeShopContent.SetActive(false);

                LidShopScrollRect.content = LidArmorShopContent.GetComponent<RectTransform>();
                LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
                LidArmorShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;

            case 3:
                LidWeaponShopContent.SetActive(false);
                LidHelmetShopContent.SetActive(false);
                LidArmorShopContent.SetActive(false);
                LidShoeShopContent.SetActive(true);

                LidShopScrollRect.content = LidShoeShopContent.GetComponent<RectTransform>();
                LidShopViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45);
                LidShoeShopContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;
        }
    }

    // Exit Shop
    void LidShopExitButtonClicked()
    {
        NPCDialogueAnimator.SetBool("isActive", true);
        ShopPanel.SetActive(false);
        InventoryButton.gameObject.SetActive(true);
        AliceShopImage.SetActive(false);
        LidShopImage.SetActive(false);

        LidItemShopContent.SetActive(true);
        LidWeaponShopContent.SetActive(false);
        LidHelmetShopContent.SetActive(false);
        LidArmorShopContent.SetActive(false);
        LidShoeShopContent.SetActive(false);
        EquipmentCategories.SetActive(false);

        AliceBubble.gameObject.SetActive(false);        

        AudioManager.NPCAudioSource.clip = AudioManager.LidGreetingClip;
        AudioManager.NPCAudioSource.Play();
    }

    // Purchase Item
    void LidShopItemPurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = true;
        isEquipmentPurchaseButtonPresed = false;

        purchaseItemData = ItemDatas[number];

        PurchasePanel.SetActive(true);
        PurchasePanelItemImage.sprite = purchaseItemData.ItemSprite;
        PurchasePanelItemName.text = purchaseItemData.ItemName;
        PurchasePanelItemCostText.text = purchaseItemData.ItemCost.ToString("#,##0") + " Gald";

        AudioManager.NPCAudioSource.clip = AudioManager.LidPurchaseButtonClip;
        AudioManager.NPCAudioSource.Play();
    }    

    // Purchase Weapon
    void LidShopWeaponPurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = false;
        isEquipmentPurchaseButtonPresed = true;

        PurchasePanel.SetActive(true);

        purchaseEquipmentData = WeaponDatas[number];
    }

    // Purchase Helmet
    void LidShopHelmetPurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = false;
        isEquipmentPurchaseButtonPresed = true;

        PurchasePanel.SetActive(true);

        purchaseEquipmentData = HelmetDatas[number];        
    }

    // Purchase Armor
    void LidShopArmorPurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = false;
        isEquipmentPurchaseButtonPresed = true;

        PurchasePanel.SetActive(true);

        purchaseEquipmentData = ArmorDatas[number];        
    }

    // Purchase Shoe
    void LidShopShoePurchaseButtonClicked(int number)
    {
        isItemPurchaseButtonPressed = false;
        isEquipmentPurchaseButtonPresed = true;

        PurchasePanel.SetActive(true);

        purchaseEquipmentData = ShoeDatas[number];
    }

    // Confirm purchasing
    void PurchaseConfirmButtonClicked()
    {
        if (isItemPurchaseButtonPressed && !isEquipmentPurchaseButtonPresed)
        {
            if (GameManager.instance.Gald >= purchaseItemData.ItemCost)
            {
                Inventory.Items.Add(purchaseItemData);
                purchaseItemData.ItemAmount++;
                Inventory.ItemAmount.Add(purchaseItemData.ItemAmount);

                GameManager.instance.Gald -= purchaseItemData.ItemCost;
                GaldText.text = GameManager.instance.Gald.ToString("#,##0");

                CheckItem();

                Button ItemSlot = Instantiate(UIInventory.UIItem, UIInventory.InventoryItemContent.transform.position, Quaternion.identity);
                ItemSlot.transform.SetParent(UIInventory.InventoryItemContent.transform);

                ItemSlot.GetComponent<UIItem>().ItemImage.sprite = purchaseItemData.ItemSprite;
                ItemSlot.GetComponent<UIItem>().ItemAmount.text = purchaseItemData.ItemAmount.ToString();
                ItemSlot.GetComponent<UIItem>().ItemData = purchaseItemData;

                UIInventory.UIItems.Add(ItemSlot);

                CheckUIItem();

                PurchasePanel.SetActive(false);

                LidBubble.SetBool("isActive", true);
                LidBubbleText.text = "구매 감사?�니??";
            }
            else
            {
                LidBubble.SetBool("isActive", true);
                LidBubbleText.text = "?�재??구매?�실 ??" + System.Environment.NewLine + "?�습?�다";

                PurchasePanel.SetActive(false);
            }
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

        AudioManager.NPCAudioSource.clip = AudioManager.LidPurchasedClip;
        AudioManager.NPCAudioSource.Play();
    }

    // Cancel purchasing
    void PurchaseCancelButtonClicked()
    {
        PurchasePanel.SetActive(false);        
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
                    Inventory.ItemAmount.Remove(Inventory.ItemAmount[i]);
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
        InventoryButton.gameObject.SetActive(false);
        InventoryPanel.SetActive(true);
        UIInventory.InventoryItemContent.SetActive(true);
        UIInventory.EquipmentCategories.SetActive(false);
        UIInventory.InventoryWeaponContent.SetActive(false);
        UIInventory.InventoryHelmetContent.SetActive(false);
        UIInventory.InventoryArmorContent.SetActive(false);
        UIInventory.InventoryShoeContent.SetActive(false);
    }

    void SetGaugeUI()
    {
        AliceHPText.text = GameManager.instance.AliceCurrentHP.ToString() + "/" + AlicePrefab.GetComponent<Alice>().HP.ToString();
        AliceMPText.text = GameManager.instance.AliceCurrentMP.ToString() + "/" + AlicePrefab.GetComponent<Alice>().MP.ToString();

        AliceHPGauge.fillAmount  = GameManager.instance.AliceCurrentHP / GameManager.instance.AlicePrefab.GetComponent<Alice>().HP;
        AliceMPGauge.fillAmount = GameManager.instance.AliceCurrentMP / GameManager.instance.AlicePrefab.GetComponent<Alice>().MP;

        GretelHPText.text = GameManager.instance.GretelCurrentHP.ToString() + "/" + GretelPrefab.GetComponent<Gretel>().HP.ToString();
        GretelMPText.text = GameManager.instance.GretelCurrentMP.ToString() + "/" + GretelPrefab.GetComponent<Gretel>().MP.ToString();

        GretelHPGauge.fillAmount = GameManager.instance.GretelCurrentHP / GameManager.instance.GretelPrefab.GetComponent<Gretel>().HP;
        GretelMPGauge.fillAmount = GameManager.instance.GretelCurrentMP / GameManager.instance.GretelPrefab.GetComponent<Gretel>().MP;

        SWHPText.text = GameManager.instance.SWCurrentHP.ToString() + "/" + SnowWhitePrefab.GetComponent<SnowWhite>().HP.ToString();
        SWMPText.text = GameManager.instance.SWCurrentMP.ToString() + "/" + SnowWhitePrefab.GetComponent<SnowWhite>().MP.ToString();

        SWHPGauge.fillAmount = GameManager.instance.SWCurrentHP / GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().HP;
        SWMPGauge.fillAmount = GameManager.instance.SWCurrentMP / GameManager.instance.SnowWhitePrefab.GetComponent<SnowWhite>().MP;
    }

    void SetUIInventory()
    {
        if (Inventory.Items.Count != 0)
        {
            for (int i = 0; i < Inventory.Items.Count; i++)
            {
                Button itemSlot = Instantiate(UIInventory.UIItem, UIInventory.InventoryItemContent.transform.position, Quaternion.identity);
                itemSlot.transform.SetParent(UIInventory.InventoryItemContent.transform);

                itemSlot.GetComponent<UIItem>().ItemImage.sprite = Inventory.Items[i].ItemSprite;
                itemSlot.GetComponent<UIItem>().ItemAmount.text = Inventory.Items[i].ItemAmount.ToString();
                itemSlot.GetComponent<UIItem>().ItemData = Inventory.Items[i];

                UIInventory.UIItems.Add(itemSlot);
            }
        }

        if (Inventory.Weapons.Count != 0)
        {
            for (int i = 0; i < Inventory.Weapons.Count; i++)
            {
                Button equipmentSlot = Instantiate(UIInventory.UIEquipment, UIInventory.InventoryWeaponButton.transform.position, Quaternion.identity);
                equipmentSlot.transform.SetParent(UIInventory.InventoryWeaponContent.transform);

                equipmentSlot.GetComponent<UIEquipment>().EquipmentImage.sprite = Inventory.Weapons[i].EquipmentSprite;
                equipmentSlot.GetComponent<UIEquipment>().EquipmentAmountText.text = Inventory.Weapons[i].EquipmentAmount.ToString();
                equipmentSlot.GetComponent<UIEquipment>().EquipmentData = Inventory.Weapons[i];

                Button cEquipmentSlot = Instantiate(UICharacterInfo.UICEquipment, UICharacterInfo.WeaponsContent.transform.position, Quaternion.identity);
                cEquipmentSlot.transform.SetParent(UICharacterInfo.WeaponsContent.transform);

                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = Inventory.Weapons[i].EquipmentSprite;
                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = Inventory.Weapons[i];
                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = Inventory.Weapons[i].EquipmentAmount;

                UIInventory.UIEquipments.Add(equipmentSlot);
                UICharacterInfo.UIWeapons.Add(cEquipmentSlot);
            }
        }

        if (Inventory.Helmets.Count != 0)
        {
            for (int i = 0; i < Inventory.Helmets.Count; i++)
            {
                Button equipmentSlot = Instantiate(UIInventory.UIEquipment, UIInventory.InventoryHelmetButton.transform.position, Quaternion.identity);
                equipmentSlot.transform.SetParent(UIInventory.InventoryHelmetContent.transform);

                equipmentSlot.GetComponent<UIEquipment>().EquipmentImage.sprite = Inventory.Helmets[i].EquipmentSprite;
                equipmentSlot.GetComponent<UIEquipment>().EquipmentAmountText.text = Inventory.Helmets[i].EquipmentAmount.ToString();
                equipmentSlot.GetComponent<UIEquipment>().EquipmentData = Inventory.Helmets[i];

                Button cEquipmentSlot = Instantiate(UICharacterInfo.UICEquipment, UICharacterInfo.HelmetsContent.transform.position, Quaternion.identity);
                cEquipmentSlot.transform.SetParent(UICharacterInfo.HelmetsContent.transform);

                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = Inventory.Helmets[i].EquipmentSprite;
                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = Inventory.Helmets[i];
                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = Inventory.Helmets[i].EquipmentAmount;

                UIInventory.UIEquipments.Add(equipmentSlot);
                UICharacterInfo.UIHelmets.Add(cEquipmentSlot);
            }
        }

        if (Inventory.Armors.Count != 0)
        {
            for (int i = 0; i < Inventory.Armors.Count; i++)
            {
                Button equipmentSlot = Instantiate(UIInventory.UIEquipment, UIInventory.InventoryArmorButton.transform.position, Quaternion.identity);
                equipmentSlot.transform.SetParent(UIInventory.InventoryArmorContent.transform);

                equipmentSlot.GetComponent<UIEquipment>().EquipmentImage.sprite = Inventory.Armors[i].EquipmentSprite;
                equipmentSlot.GetComponent<UIEquipment>().EquipmentAmountText.text = Inventory.Armors[i].EquipmentAmount.ToString();
                equipmentSlot.GetComponent<UIEquipment>().EquipmentData = Inventory.Armors[i];

                Button cEquipmentSlot = Instantiate(UICharacterInfo.UICEquipment, UICharacterInfo.ArmorsContent.transform.position, Quaternion.identity);
                cEquipmentSlot.transform.SetParent(UICharacterInfo.ArmorsContent.transform);

                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = Inventory.Armors[i].EquipmentSprite;
                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = Inventory.Armors[i];
                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = Inventory.Armors[i].EquipmentAmount;

                UIInventory.UIEquipments.Add(equipmentSlot);
                UICharacterInfo.UIHelmets.Add(cEquipmentSlot);
            }
        }

        if (Inventory.Shoes.Count != 0)
        {
            for (int i = 0; i < Inventory.Shoes.Count; i++)
            {
                Button equipmentSlot = Instantiate(UIInventory.UIEquipment, UIInventory.InventoryShoeButton.transform.position, Quaternion.identity);
                equipmentSlot.transform.SetParent(UIInventory.InventoryShoeContent.transform);

                equipmentSlot.GetComponent<UIEquipment>().EquipmentImage.sprite = Inventory.Shoes[i].EquipmentSprite;
                equipmentSlot.GetComponent<UIEquipment>().EquipmentAmountText.text = Inventory.Shoes[i].EquipmentAmount.ToString();
                equipmentSlot.GetComponent<UIEquipment>().EquipmentData = Inventory.Shoes[i];

                Button cEquipmentSlot = Instantiate(UICharacterInfo.UICEquipment, UICharacterInfo.ShoesContent.transform.position, Quaternion.identity);
                cEquipmentSlot.transform.SetParent(UICharacterInfo.ShoesContent.transform);

                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentImage.sprite = Inventory.Shoes[i].EquipmentSprite;
                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentData = Inventory.Shoes[i];
                cEquipmentSlot.GetComponent<UICEquipment>().EquipmentAmount = Inventory.Shoes[i].EquipmentAmount;

                UIInventory.UIEquipments.Add(equipmentSlot);
                UICharacterInfo.UIHelmets.Add(cEquipmentSlot);
            }
        }
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
