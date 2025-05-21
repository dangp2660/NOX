using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Cutscene;
    [SerializeField] private float timeCutscene;
    [SerializeField] private bool isActive =false;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PlayerManager;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager = GameObject.FindGameObjectWithTag("PlayerManager");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Cutscene == null) return;
        if ((collision.CompareTag("Player")) && !isActive)
        {
            StartCoroutine(TimeCutsceneRun(timeCutscene));
        }

    }

    IEnumerator TimeCutsceneRun(float timeCutscene)
    {
        Player.GetComponent<PlayerInput>().enabled = false;
        PlayerManager.GetComponent<PlayerSwitch>().enabled = false;
        isActive = true;
        Cutscene.SetActive(true);
        yield return new WaitForSeconds(timeCutscene);
        Cutscene.SetActive(false);
        isActive = false;
        Player.GetComponent<PlayerInput>().enabled = true;
        PlayerManager.GetComponent<PlayerSwitch>().enabled = true;
        this.enabled = false;
        this.gameObject.SetActive(false);
    }


}
