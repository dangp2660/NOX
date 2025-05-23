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
        // Add null check and gameObject.activeInHierarchy check
        if (this != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(FadedDeath());
        }
    }

    IEnumerator FadedDeath()
    {
        redOverlay.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime * .25f;
            //Debug.Log(alpha);
            redOverlay.color = new Color(0.3f, 0, 0, alpha);
            yield return null;
        }
        textMeshPro.gameObject.SetActive(true);
    }
    
    public void HideDeathScreen()
    {
        StopAllCoroutines();
        redOverlay.gameObject.SetActive(false);
        textMeshPro.gameObject.SetActive(false);
    }
}
