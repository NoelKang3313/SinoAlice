using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public Image TransitionImage;

    public GameObject LobbyPanel;

    public Button CharlotteButton;
    public Button WinryButton;
    public Button LidButton;

    public GameObject NPCPanel;
    public GameObject LidInteractImage;
    public GameObject WinryInteractImage;
    public GameObject AliceInteractImage;

    public Button LidShopButton;
    public GameObject LidShopContent;
    public Button LidReturnButton;

    public Button WinryShopButton;
    public GameObject WinryShopContent;
    public Button WinryReturnButton;

    public Button CharlotteWorldmapButton;
    public Button CharlotteReturnButton;

    public Button ExitButton;

    public Button InventoryButton;
    public GameObject InventoryPanel;
    public Button ExitInventoryButton;

    void Start()
    {
        CharlotteButton.onClick.AddListener(CharlotteButtonClicked);
        WinryButton.onClick.AddListener(WinryButtonClicked);
        LidButton.onClick.AddListener(LidButtonClicked);

        LidShopButton.onClick.AddListener(LidShopButtonClicked);
        LidReturnButton.onClick.AddListener(LidReturnButtonClicked);

        WinryShopButton.onClick.AddListener(WinryShopButtonClicked);
        WinryReturnButton.onClick.AddListener(WinryReturnButtonClicked);

        CharlotteWorldmapButton.onClick.AddListener(CharlotteWorldmapButtonClicked);
        CharlotteReturnButton.onClick.AddListener(CharlotteReturnButtonClicked);

        ExitButton.onClick.AddListener(ExitButtonClicked);

        InventoryButton.onClick.AddListener(InventoryButtonClicked);
        ExitInventoryButton.onClick.AddListener(ExitInventoryButtonClicked);
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

    void CharlotteButtonClicked()
    {
        GameManager.instance.isCharlotteButtonActive = true;
        GameManager.instance.isAction = true;

        WinryButton.interactable = false;
        LidButton.interactable = false;
    }

    void WinryButtonClicked()
    {
        GameManager.instance.isWinryButtonActive = true;
        GameManager.instance.isAction = true;

        CharlotteButton.interactable = false;
        LidButton.interactable = false;
    }

    void LidButtonClicked()
    {
        GameManager.instance.isLidButtonActive = true;
        GameManager.instance.isAction = true;

        CharlotteButton.interactable = false;
        WinryButton.interactable = false;
    }

    void LidShopButtonClicked()
    {
        NPCPanel.SetActive(true);
        AliceInteractImage.SetActive(true);
        LidInteractImage.SetActive(true);
        LidShopContent.SetActive(true);
        WinryShopContent.SetActive(false);
    }

    void LidReturnButtonClicked()
    {
        NPCPanel.SetActive(false);
        AliceInteractImage.SetActive(false);
        LidInteractImage.SetActive(false);

        LidShopButton.gameObject.SetActive(false);
        LidReturnButton.gameObject.SetActive(false);

        GameManager.instance.isReturnButtonActive = true;
    }

    void WinryShopButtonClicked()
    {
        NPCPanel.SetActive(true);
        AliceInteractImage.SetActive(true);
        WinryInteractImage.SetActive(true);
        LidShopContent.SetActive(false);
        WinryShopContent.SetActive(true);
    }

    void WinryReturnButtonClicked()
    {
        NPCPanel.SetActive(false);
        AliceInteractImage.SetActive(false);
        WinryInteractImage.SetActive(false);

        WinryShopButton.gameObject.SetActive(false);
        WinryReturnButton.gameObject.SetActive(false);

        GameManager.instance.isReturnButtonActive = true;
    }

    void CharlotteWorldmapButtonClicked()
    {
        GameManager.instance.isTransition = true;
        GameManager.instance.isCharlotteButtonActive = false;
    }

    void CharlotteReturnButtonClicked()
    {
        CharlotteWorldmapButton.gameObject.SetActive(false);
        CharlotteReturnButton.gameObject.SetActive(false);

        GameManager.instance.isReturnButtonActive = true;
    }

    void ExitButtonClicked()
    {
        NPCPanel.SetActive(false);
        AliceInteractImage.SetActive(false);
        LidInteractImage.SetActive(false);
        WinryInteractImage.SetActive(false);
    }

    void InventoryButtonClicked()
    {
        InventoryPanel.SetActive(true);
    }

    void ExitInventoryButtonClicked()
    {
        InventoryPanel.SetActive(false);
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
