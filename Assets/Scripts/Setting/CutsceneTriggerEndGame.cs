using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CutsceneTriggerEndGame : MonoBehaviour
{
    public static CutsceneTriggerEndGame Instance;
    [SerializeField] private GameObject Cutscene;
    [SerializeField] private bool isActive = false;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PlayerManager;
    [SerializeField] private CinemachineVirtualCamera cameraBattle;
    [SerializeField] private CinemachineVirtualCamera currentCam;
    [SerializeField] private GameObject Boss;
    [SerializeField] private bool reEnableAfterCutscene = false; // keep disabled by default

    private GameObject UI;
    private PlayableDirector director;

    private void Awake()
    {
        Instance = this;
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (PlayerManager == null) PlayerManager = GameObject.FindGameObjectWithTag("PlayerManager");
        if (Boss == null) Boss = GameObject.FindGameObjectWithTag("Boss");
    }

    private void Start()
    {
        if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");
        if (PlayerManager == null) PlayerManager = GameObject.FindGameObjectWithTag("PlayerManager");
        if (Boss == null) Boss = GameObject.FindGameObjectWithTag("Boss");
        UI = GameObject.FindGameObjectWithTag("UI");
    }

    public void StartCutscene()
    {
        UI = GameObject.FindGameObjectWithTag("UI");
        if (isActive) return;
        isActive = true;
        if (UI != null) UI.SetActive(false);
        if (currentCam != null) currentCam.gameObject.SetActive(false);

        if (Boss != null) Boss.GetComponent<NecromancerBoss>().enabled = false;

        if (Player != null) Player.GetComponent<PlayerInput>().enabled = false;
        if (PlayerManager != null) PlayerManager.GetComponent<PlayerSwitch>().enabled = false;


        if (Cutscene != null)
        {
            Cutscene.SetActive(true);
            director = Cutscene.GetComponent<PlayableDirector>();
            if (director != null)
            {
                director.stopped += OnDirectorStopped;
                director.Play();
            }
        }
    }

    private void OnDirectorStopped(PlayableDirector d)
    {
        d.stopped -= OnDirectorStopped;

        if (reEnableAfterCutscene)
        {
            EndCutscene();
        }
    }

    public void EndCutscene()
    {
        if (!isActive) return;

        if (Cutscene != null) Cutscene.SetActive(false);
        if (cameraBattle != null) cameraBattle.gameObject.SetActive(true);

        if (Player != null) Player.GetComponent<PlayerInput>().enabled = true;
        if (PlayerManager != null) PlayerManager.GetComponent<PlayerSwitch>().enabled = true;
        if (UI != null) UI.SetActive(true);

        isActive = false;
        gameObject.SetActive(false);
    }
}