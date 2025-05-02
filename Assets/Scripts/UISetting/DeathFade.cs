using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeathFade : MonoBehaviour
{
    [SerializeField] private Image redOverlay;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    public void showDeathScreen()
    {
        Debug.Log("DieEffect");
        StartCoroutine(FadedDeath());
    }

    IEnumerator FadedDeath()
    {
        redOverlay.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime * 0.5f;
            Debug.Log(alpha);
            redOverlay.color = new Color(0.3f, 0, 0, alpha);
            yield return null;
        }
        textMeshPro.gameObject.SetActive(true);
    }
}
