using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldmapUIManager : MonoBehaviour
{
    public Button Stage1Button;
    public GameObject BackgroundGameObject;
    public Sprite StageBG;

    public Button Stage1_1Button;

    public GameObject StagePanel;
    public Button StagePanelExitButton;

    public Button[] CharacterLocationButtons = new Button[3];
    public Button SelectedButton;
    public GameObject CharacterSelectionScroll;
    public Button[] CharacterSelectionButtons = new Button[3];

    public GameObject SelectedCharacter;
    public GameObject[] CharacterPrefabs = new GameObject[3];
    public GameObject AnimatedCharacterImage;
    public GameObject CharacterLocations;

    public Image TransitionImage;

    void Start()
    {
        Stage1Button.onClick.AddListener(Stage1ButtonClicked);
        Stage1_1Button.onClick.AddListener(Stage1_1ButtonClicked);

        StagePanelExitButton.onClick.AddListener(StagePanelExitButtonClicked);

        for(int i = 0; i < CharacterLocationButtons.Length; i++)
        {
            int number = i;
            CharacterLocationButtons[i].onClick.AddListener(() => CharacterLocationButtonClicked(number));
        }

        for(int i = 0; i < CharacterSelectionButtons.Length; i++)
        {
            int number = i;
            CharacterSelectionButtons[i].onClick.AddListener(() => CharacterSelectionButtonClicked(number));
        }
    }

    void Update()
    {
        ActivateTransition(1.0f);

        if(TransitionImage.fillAmount == 0)
        {
            TransitionImage.gameObject.SetActive(false);
        }

    }

    void Stage1ButtonClicked()
    {
        Stage1Button.gameObject.SetActive(false);

        BackgroundGameObject.GetComponent<SpriteRenderer>().sprite = StageBG;
        BackgroundGameObject.transform.localScale = new Vector3(1, 0.53f, 0);
        
        Stage1_1Button.gameObject.SetActive(true);
    }

    void Stage1_1ButtonClicked()
    {
        StagePanel.SetActive(true);
    }

    void StagePanelExitButtonClicked()
    {
        StagePanel.SetActive(false);        
    }

    void CharacterLocationButtonClicked(int number)
    {
        CharacterSelectionScroll.SetActive(true);
        SelectedButton = CharacterLocationButtons[number];
    }

    void CharacterSelectionButtonClicked(int number)
    {
        SelectedCharacter = CharacterPrefabs[number];

        GameManager.instance.CharacterSelected[number] = CharacterPrefabs[number];
        GameObject AnimatedCharacter = Instantiate(AnimatedCharacterImage);
        AnimatedCharacter.transform.SetParent(CharacterLocations.transform);
        AnimatedCharacter.GetComponent<RectTransform>().anchoredPosition = SelectedButton.GetComponent<RectTransform>().anchoredPosition;

        if (SelectedCharacter.name == "Alice")
        {
            AnimatedCharacter.GetComponent<Animator>().Play("Alice_Idle");
        }
        else if (SelectedCharacter.name == "Gretel")
        {
            AnimatedCharacter.GetComponent<Animator>().Play("Gretel_Idle");
        }
        else if (SelectedCharacter.name == "Snow White")
        {
            AnimatedCharacter.GetComponent<Animator>().Play("SnowWhite_Idle");
        }

        CharacterSelectionScroll.SetActive(false);
    }

    void ActivateTransition(float transitionSpeed)
    {
        if (GameManager.instance.isTransition)
        {
            TransitionImage.fillAmount += transitionSpeed / 1.0f * Time.deltaTime;
        }
        else
        {
            TransitionImage.fillAmount -= transitionSpeed / 1.0f * Time.deltaTime;
        }
    }
}
