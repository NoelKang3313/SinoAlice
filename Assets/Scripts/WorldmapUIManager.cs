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

    public Image TransitionImage;

    void Start()
    {
        Stage1Button.onClick.AddListener(Stage1ButtonClicked);
        Stage1_1Button.onClick.AddListener(Stage1_1ButtonClicked);

        StagePanelExitButton.onClick.AddListener(StagePanelExitButtonClicked);
    }

    void Update()
    {
        ActivateTransition(1.0f);

        if(TransitionImage.fillAmount == 0)
        {
            Stage1Button.gameObject.SetActive(true);
        }
    }

    void Stage1ButtonClicked()
    {
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
