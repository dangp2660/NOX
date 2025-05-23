using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Cutscene;
    [SerializeField] private bool isActive = false;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PlayerManager;
    [SerializeField] private CinemachineVirtualCamera cameraBattle;
    [SerializeField] private CinemachineVirtualCamera currentCam;



    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager = GameObject.FindGameObjectWithTag("PlayerManager");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Cutscene == null) return;
        if (collision.CompareTag("Player") && !isActive)
        {
            StartCutscene();
        }
    }

    public void StartCutscene()
    {
        
        currentCam.gameObject.SetActive(false);
        isActive = true;
        Player.GetComponent<PlayerInput>().enabled = false;
        PlayerManager.GetComponent<PlayerSwitch>().enabled = false;
        if (Cutscene != null)
        {
            Cutscene.SetActive(true);
        }

    }

    // ✅ Gọi hàm này từ cuối Timeline / AnimationEvent / Trigger tùy bạn
    public void EndCutscene(PlayableDirector signalDirector)
    {
        if (!isActive) return;

        Cutscene.SetActive(false);

        if (cameraBattle != null )
        {
            Debug.Log("avc");
 
            cameraBattle.gameObject.SetActive(true);

        }

        Player.GetComponent<PlayerInput>().enabled = true;
        PlayerManager.GetComponent<PlayerSwitch>().enabled = true;
        if (signalDirector != null)
        {
            signalDirector.gameObject.SetActive(true);
            signalDirector.Play(); // ✅ Phát Timeline có SignalEmitter → Trigger UnityEvent
        }

        this.gameObject.SetActive(false );
        
    }
}
