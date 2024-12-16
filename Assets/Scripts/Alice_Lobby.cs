using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alice_Lobby : MonoBehaviour
{
    private LobbyUIManager UIManager;
    public LobbyAudioManager AudioManager;
    private Animator animator;

    private float moveSpeed = 15.0f;

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
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(5.0f, -0.4f), moveSpeed * Time.deltaTime);

            if (transform.position == new Vector3(5.0f, -0.4f, 0))
            {
                UIManager.NPCDialogueAnimator.SetBool("isActive", true);
                UIManager.NPCNameText.text = "샤를로트";
                UIManager.NPCButtonText.text = "월드맵";

                GameManager.instance.isAction = false;

                animator.SetBool("Move", false);

                AudioManager.NPCAudioSource.clip = AudioManager.CharlotteGreetingClip;
                AudioManager.NPCAudioSource.Play();
            }
        }
        else if (GameManager.instance.isWinryButtonActive && GameManager.instance.isAction)
        {
            animator.SetBool("Move", true);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-1.5f, 0.2f), moveSpeed * Time.deltaTime);

            if (transform.position == new Vector3(-1.5f, 0.2f, 0))
            {
                UIManager.NPCDialogueAnimator.SetBool("isActive", true);
                UIManager.NPCNameText.text = "윈리";
                UIManager.NPCButtonText.text = "캐릭터";

                GameManager.instance.isAction = false;

                animator.SetBool("Move", false);

                AudioManager.NPCAudioSource.clip = AudioManager.WinryGreetingClip;
                AudioManager.NPCAudioSource.Play();
            }
        }
        else if (GameManager.instance.isLidButtonActive && GameManager.instance.isAction)
        {
            animator.SetBool("Move", true);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-5.0f, 0.2f), moveSpeed * Time.deltaTime);

            if (transform.position == new Vector3(-5.0f, 0.2f, 0))
            {
                UIManager.NPCDialogueAnimator.SetBool("isActive", true);                
                UIManager.NPCNameText.text = "리드";
                UIManager.NPCButtonText.text = "샵";

                GameManager.instance.isAction = false;

                animator.SetBool("Move", false);
                
                AudioManager.NPCAudioSource.clip = AudioManager.LidGreetingClip;
                AudioManager.NPCAudioSource.Play();
            }
        }

        if (GameManager.instance.isReturnButtonActive)
        {
            if (GameManager.instance.isCharlotteButtonActive)
            {
                animator.SetBool("Move", true);

                transform.GetComponent<SpriteRenderer>().flipX = false;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), moveSpeed * Time.deltaTime);

                if (transform.position == new Vector3(0, -1, 0))
                {
                    GameManager.instance.isCharlotteButtonActive = false;
                    GameManager.instance.isReturnButtonActive = false;

                    animator.SetBool("Move", false);

                    //UIManager.CharlotteButton.interactable = true;
                    //UIManager.WinryButton.interactable = true;
                    //UIManager.LidButton.interactable = true;

                    UIManager.NPCButtons.SetActive(true);
                }
            }
            else if (GameManager.instance.isWinryButtonActive)
            {
                animator.SetBool("Move", true);

                transform.GetComponent<SpriteRenderer>().flipX = true;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), moveSpeed * Time.deltaTime);

                if (transform.position == new Vector3(0, -1, 0))
                {
                    GameManager.instance.isWinryButtonActive = false;
                    GameManager.instance.isReturnButtonActive = false;

                    animator.SetBool("Move", false);

                    transform.GetComponent<SpriteRenderer>().flipX = false;

                    //UIManager.CharlotteButton.interactable = true;
                    //UIManager.WinryButton.interactable = true;
                    //UIManager.LidButton.interactable = true;

                    UIManager.NPCButtons.SetActive(true);
                }
            }

            else if (GameManager.instance.isLidButtonActive)
            {
                animator.SetBool("Move", true);

                transform.GetComponent<SpriteRenderer>().flipX = true;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, -1), moveSpeed * Time.deltaTime);

                if (transform.position == new Vector3(0, -1, 0))
                {
                    GameManager.instance.isLidButtonActive = false;
                    GameManager.instance.isReturnButtonActive = false;

                    animator.SetBool("Move", false);

                    transform.GetComponent<SpriteRenderer>().flipX = false;

                    //UIManager.CharlotteButton.interactable = true;
                    //UIManager.WinryButton.interactable = true;
                    //UIManager.LidButton.interactable = true;

                    UIManager.NPCButtons.SetActive(true);
                }
            }
        }
    }
}
