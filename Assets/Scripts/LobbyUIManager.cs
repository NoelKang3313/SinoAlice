using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public GameObject LobbyPanel;

    public Button CharlotteButton;
    public Button WinryButton;
    public Button LidButton;

    public Button ExitButton;
    public Button EnterWorldButton;

    void Start()
    {
        CharlotteButton.onClick.AddListener(CharlotteButtonClicked);
        WinryButton.onClick.AddListener(WinryButtonClicked);
        LidButton.onClick.AddListener(LidButtonClicked);

        ExitButton.onClick.AddListener(ExitButtonClicked);
        EnterWorldButton.onClick.AddListener(EnterWorldButtonClicked);
    }

    void Update()
    {
        
    }

    void CharlotteButtonClicked()
    {
        GameManager.instance.isCharlotteButtonActive = true;
        GameManager.instance.isAction = true;

        WinryButton.GetComponent<Button>().interactable = false;
        LidButton.GetComponent<Button>().interactable = false;
    }

    void WinryButtonClicked()
    {
        GameManager.instance.isWinryButtonActive = true;
        GameManager.instance.isAction = true;

        CharlotteButton.GetComponent<Button>().interactable = false;
        LidButton.GetComponent<Button>().interactable = false;
    }

    void LidButtonClicked()
    {
        GameManager.instance.isLidButtonActive = true;
        GameManager.instance.isAction = true;

        CharlotteButton.GetComponent<Button>().interactable = false;
        WinryButton.GetComponent<Button>().interactable = false;
    }

    void ExitButtonClicked()
    {
        GameManager.instance.isExitButtonActive = true;

        CharlotteButton.GetComponent<Button>().interactable = true;
        WinryButton.GetComponent<Button>().interactable = true;
        LidButton.GetComponent<Button>().interactable = true;
    }

    void EnterWorldButtonClicked()
    {
        GameManager.instance.isCharlotteButtonActive = false;
        GameManager.instance.isTransition = true;
        LobbyPanel.SetActive(false);
    }
}
