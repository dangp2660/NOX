using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Cutscene;
    [SerializeField] private float timeCutscene;
    [SerializeField] private bool isActive =false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")) && !isActive)
        {

            StartCoroutine(TimeCutsceneRun(timeCutscene));
        }

    }

    IEnumerator TimeCutsceneRun(float timeCutscene)
    {
        isActive = true;
        Cutscene.SetActive(true);
        yield return new WaitForSeconds(timeCutscene);
        Cutscene.SetActive(false);
        isActive = false;
        Destroy(Cutscene);
    }


}
