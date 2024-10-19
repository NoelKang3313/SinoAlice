using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alice_Lobby : MonoBehaviour
{
    private LobbyUIManager UIManager;
    private Animator animator;

    void Start()
    {
        UIManager = GameObject.Find("UIManager").GetComponent<LobbyUIManager>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        AliceLobbyAction();
    }

    void AliceLobbyAction()
    {
        if (GameManager.instance.isCharlotteButtonActive && GameManager.instance.isAction)
        {
            animator.SetBool("Move", true);

            transform.GetComponent<SpriteRenderer>().flipX = true;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(5.5f, -0.5f), 0.1f);

            if (transform.position == new Vector3(5.5f, -0.5f, 0))
            {
                GameManager.instance.isAction = false;

                animator.SetBool("Move", false);

                UIManager.CharlotteWorldmapButton.gameObject.SetActive(true);
                UIManager.CharlotteReturnButton.gameObject.SetActive(true);
            }
        }
        else if (GameManager.instance.isWinryButtonActive && GameManager.instance.isAction)
        {
            animator.SetBool("Move", true);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-2, -0.2f), 0.1f);

            if (transform.position == new Vector3(-2, -0.2f, 0))
            {
                GameManager.instance.isAction = false;

                animator.SetBool("Move", false);

                UIManager.WinryCharacterButton.gameObject.SetActive(true);
                UIManager.WinryReturnButton.gameObject.SetActive(true);
            }
        }
        else if (GameManager.instance.isLidButtonActive && GameManager.instance.isAction)
        {
            animator.SetBool("Move", true);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-5.5f, -0.2f), 0.1f);

            if (transform.position == new Vector3(-5.5f, -0.2f, 0))
            {
                GameManager.instance.isAction = false;

                animator.SetBool("Move", false);

                UIManager.LidShopButton.gameObject.SetActive(true);
                UIManager.LidReturnButton.gameObject.SetActive(true);
            }
        }

        if (GameManager.instance.isReturnButtonActive)
        {
            if (GameManager.instance.isCharlotteButtonActive)
            {
                animator.SetBool("Move", true);

                transform.GetComponent<SpriteRenderer>().flipX = false;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), 0.1f);

                if (transform.position == new Vector3(0, -1, 0))
                {
                    GameManager.instance.isCharlotteButtonActive = false;
                    GameManager.instance.isReturnButtonActive = false;

                    animator.SetBool("Move", false);

                    UIManager.CharlotteButton.interactable = true;
                    UIManager.WinryButton.interactable = true;
                    UIManager.LidButton.interactable = true;
                }
            }
            else if (GameManager.instance.isWinryButtonActive)
            {
                animator.SetBool("Move", true);

                transform.GetComponent<SpriteRenderer>().flipX = true;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), 0.1f);

                if (transform.position == new Vector3(0, -1, 0))
                {
                    GameManager.instance.isWinryButtonActive = false;
                    GameManager.instance.isReturnButtonActive = false;

                    animator.SetBool("Move", false);

                    transform.GetComponent<SpriteRenderer>().flipX = false;

                    UIManager.CharlotteButton.interactable = true;
                    UIManager.WinryButton.interactable = true;
                    UIManager.LidButton.interactable = true;
                }
            }

            else if (GameManager.instance.isLidButtonActive)
            {
                animator.SetBool("Move", true);

                transform.GetComponent<SpriteRenderer>().flipX = true;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), 0.1f);

                if (transform.position == new Vector3(0, -1, 0))
                {
                    GameManager.instance.isLidButtonActive = false;
                    GameManager.instance.isReturnButtonActive = false;

                    animator.SetBool("Move", false);

                    transform.GetComponent<SpriteRenderer>().flipX = false;

                    UIManager.CharlotteButton.interactable = true;
                    UIManager.WinryButton.interactable = true;
                    UIManager.LidButton.interactable = true;
                }
            }
        }
    }
}
