using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathFadeAutoStart : MonoBehaviour
{
    [SerializeField] private Image redOverlay;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    void Start()
    {
        // Ẩn ban đầu
        redOverlay.gameObject.SetActive(false);
        textMeshPro.gameObject.SetActive(false);

        // Bắt đầu hiệu ứng tự động
        StartCoroutine(FadedDeath());
    }

    IEnumerator FadedDeath()
    {
        redOverlay.gameObject.SetActive(true);
        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime * 0.25f;
            redOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        textMeshPro.gameObject.SetActive(true);
    }
}
