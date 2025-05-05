using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSwitch : MonoBehaviour
{
    [SerializeField] public GameObject defaultForm;
    [SerializeField] public GameObject darkForm;
    [SerializeField] private CinemachineVirtualCamera camera;

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

            camera.Follow = isDefault ? defaultForm.transform : darkForm.transform;
        }

        HandleDarkEnergy();
    }

    private void HandleDarkEnergy()
    {
        if (!isDefault)
        {
            darkEnergy.CurrentDarkEnergy -= darkFormDrainRate * Time.deltaTime;

            if (darkEnergy.CurrentDarkEnergy <= 0f)
            {
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
