using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerSwitch : MonoBehaviour
{
    public static PlayerSwitch instance;
    [SerializeField] private GameObject defaultForm;
    [SerializeField] private GameObject darkForm;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private GameObject VFX;
    [SerializeField] private SpellCoolDown SpellCoolDown;
    private PlayerMovement defaultMove;
    private PlayerMovement darkMove;
    private Damageable defaultHealth;
    private Damageable darkHealth;
    private DarkEnergyManager defaultEnergy;
    private DarkEnergyManager darkEnergy;
    private PlayerAttack defaultFormAttack;
    private PlayerAttack darkFormAttack;

    public bool isDefault = true;

    [Header("Dark Energy Settings")]
    public float darkFormDrainRate = 10f; // per second
    public float regenRate = 5f; // per second
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();

    }
    private void LateUpdate()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
    }
    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Tìm vị trí spawn trong scene
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

        if (spawnPoint != null)
        {
            if(isDefault)
            {
                defaultForm.transform.position = spawnPoint.transform.position;
            }
            else
            {
                darkForm.transform.position = spawnPoint.transform.position;
            }
        }
        else
        {
            Debug.LogWarning("No PlayerSpawn point found in scene. Player stays at previous position.");
        }

        // Dọn dẹp nếu có bản player khác được load sẵn trong scene
        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        if (tempPlayer != null && tempPlayer != defaultForm && tempPlayer != darkForm)
        {
            Destroy(tempPlayer);
        }
    }

    private void Start()
    {
        defaultFormAttack = defaultForm.GetComponent<PlayerAttack>();
        darkFormAttack = darkForm.GetComponent<PlayerAttack>();
        defaultMove = defaultForm.GetComponent<PlayerMovement>();
        darkMove = darkForm.GetComponent<PlayerMovement>();
        defaultHealth = defaultForm.GetComponent<Damageable>();
        darkHealth = darkForm.GetComponent<Damageable>();
        darkEnergy = darkForm.GetComponent<DarkEnergyManager>();
        defaultEnergy = defaultForm.GetComponent<DarkEnergyManager>();
        if (!isDefault)
        {
            Vector3 currentPosition = defaultForm.transform.position;

            defaultForm.SetActive(false);
            darkForm.SetActive(true);

            darkForm.transform.position = currentPosition;
            darkMove.CopyStateFrom();
            darkHealth.healthCopy(defaultHealth);
            darkEnergy.CopyDarkEnergy(defaultEnergy);

            camera.Follow = darkForm.transform;
        }

    }
  

    void Update()
    {
     
       
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SpellCoolDown.UseSpell();
            Vector3 currentPosition = isDefault ? defaultForm.transform.position : darkForm.transform.position;

            isDefault = !isDefault;

            defaultForm.SetActive(isDefault);
            darkForm.SetActive(!isDefault);

            if (isDefault)
            {
                currentPosition.y += -0.1900992f;
                defaultForm.transform.position = currentPosition;
                defaultMove.CopyStateFrom();
                defaultEnergy.CopyDarkEnergy(darkEnergy);
                defaultHealth.healthCopy(darkHealth);
                defaultFormAttack.getCoolDown(darkFormAttack);
            }
            else
            {
                darkForm.transform.position = currentPosition;
                darkMove.CopyStateFrom();
                darkHealth.healthCopy(defaultHealth);
                darkEnergy.CopyDarkEnergy(defaultEnergy);
                darkFormAttack.getCoolDown(defaultFormAttack);
            }
            StartCoroutine(DestroyVFX());

        }

        HandleDarkEnergy();
    }

    IEnumerator DestroyVFX()
    {
        Vector3 currentPosition = isDefault ? defaultForm.transform.position : darkForm.transform.position;
        GameObject VFXused = Instantiate(VFX, currentPosition, Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(VFXused);

    }
    private void HandleDarkEnergy()
    {
        if (!isDefault)
        {
            darkEnergy.CurrentDarkEnergy -= darkFormDrainRate * Time.deltaTime;

            if (darkHealth.IsAlive)
            {
                float regenAmount = darkHealth.getMaxHealth() * 0.02f * Time.deltaTime;
                darkHealth.CurrentHealth += regenAmount;
            }

            if (darkEnergy.CurrentDarkEnergy <= 0f)
            {
                StartCoroutine(DestroyVFX());
                // Auto chuyển về default
                isDefault = true;
                darkForm.SetActive(false);
                defaultForm.SetActive(true);

                Vector3 newPosition = darkForm.transform.position;
                newPosition.y += -0.1900992f;
                defaultForm.transform.position = newPosition;

                defaultMove.CopyStateFrom();
                defaultHealth.healthCopy(darkHealth);
                defaultEnergy.CopyDarkEnergy(darkEnergy);

                camera.Follow = defaultForm.transform;
            }
        }
        else
        {
            if (!defaultHealth.IsAlive || !darkHealth.IsAlive) return;

            if (defaultEnergy.CurrentDarkEnergy < defaultEnergy.MaxDarkEnergy)
                defaultEnergy.RegenerateDarkEnergy(regenRate * Time.deltaTime);
        }

    }

    public void EnableSignal()
    {
        this.enabled = true;
    }
    public void DisableSignal()
    {
        this.enabled = false;
    }
}