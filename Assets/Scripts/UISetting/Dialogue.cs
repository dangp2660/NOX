using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float autoNextDelay = 2f;
    [SerializeField] private CutsceneTrigger cutsceneTrigger;
    [SerializeField] private PlayableDirector signalDirector;


    private int index;
    private bool lineFinished;
    private Coroutine autoNextCoroutine;

    void Start()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueText.text == lines[index])
            {
                if (autoNextCoroutine != null)
                {
                    StopCoroutine(autoNextCoroutine);
                }
                nextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
                lineFinished = true;

                if (autoNextCoroutine == null)
                    autoNextCoroutine = StartCoroutine(AutoNextLineAfterDelay());
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        dialogueText.text = string.Empty;
        lineFinished = false;

        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        lineFinished = true;

        if (autoNextCoroutine != null)
            StopCoroutine(autoNextCoroutine);
        autoNextCoroutine = StartCoroutine(AutoNextLineAfterDelay());
    }

    IEnumerator AutoNextLineAfterDelay()
    {
        yield return new WaitForSeconds(autoNextDelay);
        if (lineFinished)
        {
            nextLine();
        }
    }

    public void nextLine()
    {
        if (autoNextCoroutine != null)
        {
            StopCoroutine(autoNextCoroutine);
            autoNextCoroutine = null;
        }

        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (cutsceneTrigger != null && signalDirector != null) 
                cutsceneTrigger.EndCutscene(signalDirector);
            gameObject.SetActive(false);
            
        }
    }
}
