using System.Collections;
using TMPro;
using UnityEngine;

public class endGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float autoNextDelay = 2f;

    private int index;

    void Start()
    {
        dialogueText.text = string.Empty;
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        while (index < lines.Length)
        {
            // Gõ từng chữ
            dialogueText.text = "";
            foreach (char c in lines[index].ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            // Đợi trước khi sang dòng tiếp theo
            yield return new WaitForSeconds(autoNextDelay);
            index++;
        }

        // Kết thúc đoạn hội thoại
        gameObject.SetActive(false);
    }
}
