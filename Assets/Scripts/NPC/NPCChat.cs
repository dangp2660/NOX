using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCChat : MonoBehaviour
{
    public GameObject dialoguePanel; // Bubble UI: canvas dạng World Space
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    public float worldSpeed = 0.03f;
    public bool closePlayer;

    public Vector3 panelOffset = new Vector3(0, 2f, 0); // offset so với đầu NPC
    private Coroutine typingCoroutine;
    private Coroutine autoHideCoroutine;

    void Update()
    {
        // Hiện thoại khi người chơi gần và bấm E
        if (closePlayer && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                index = 0;
                dialogueText.text = "";

                typingCoroutine = StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }
            else
            {
                // Nếu đang gõ dở thì hiện toàn bộ câu ngay
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                dialogueText.text = dialogue[index];
            }
        }
    }


    void LateUpdate()
    {
        // Đảm bảo bubble luôn nằm trên đầu NPC
        if (dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.transform.position = transform.position + panelOffset;
            dialoguePanel.transform.localScale = Vector3.one * 0.01f; // giữ kích thước ổn định

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            closePlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            closePlayer = false;
            ZeroText();
        }
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }

    IEnumerator Typing()
    {
        dialogueText.text = "";

        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(worldSpeed);
        }

        // Khi gõ xong toàn bộ → bắt đầu đếm giờ tự ẩn
        if (autoHideCoroutine != null) StopCoroutine(autoHideCoroutine);
        autoHideCoroutine = StartCoroutine(AutoHideAfterDelay());
    }

    IEnumerator AutoHideAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        // Nếu người chơi chưa bấm E tiếp thì tự ẩn
        if (dialogueText.text == dialogue[index])
        {
            ZeroText();
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (autoHideCoroutine != null) StopCoroutine(autoHideCoroutine);
    }
}