using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldmapUIManager : MonoBehaviour
{
    public Image TransitionImage;

    void Update()
    {
        ActivateTransition(1.0f);
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
