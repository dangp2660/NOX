using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCChat : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    public float worldSpeed;
    public bool closePlayer;

    private void Awake()
    {
        
    }

    void Update()
    {
        if (closePlayer && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                index = 0;
                dialogueText.text = "";
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                nextLine(); 
            }
            else
            {
                // Nếu đang gõ dở thì hiện toàn bộ câu ngay lập tức
                StopAllCoroutines();
                dialogueText.text = dialogue[index];
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            closePlayer = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            closePlayer = false;
    }
  

    public void nextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(worldSpeed);
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }
}
