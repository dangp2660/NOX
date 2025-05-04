using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlayerSwitch : MonoBehaviour
{
    [Header("Forms Settings")]
    [SerializeField] public GameObject defaultForm;
    [SerializeField] public GameObject darkForm;
    [SerializeField] private CinemachineVirtualCamera camera;

    private PlayerMovement defaultMove;
    private PlayerMovement darkMove;

    private Damageable defaultHealth;
    private Damageable darkHealth;

    private DarkEnergyManager defaultEnergy;
    private DarkEnergyManager darkEnergy;

    [SerializeField] private float energyCostPerSecond = 10f;
    private float darkFormTimer = 0f;
    [SerializeField] private bool isDefault = true;

    private void Start()
    {
        // Kiểm tra và gán các thành phần
        if (defaultForm == null)
        {
            Debug.LogError("defaultForm chưa được gán trong Unity Inspector!");
            return;
        }

        if (darkForm == null)
        {
            Debug.LogError("darkForm chưa được gán trong Unity Inspector!");
            return;
        }

        defaultMove = defaultForm.GetComponent<PlayerMovement>();
        darkMove = darkForm.GetComponent<PlayerMovement>();

        defaultHealth = defaultForm.GetComponent<Damageable>();
        darkHealth = darkForm.GetComponent<Damageable>();

        defaultEnergy = defaultForm.GetComponent<DarkEnergyManager>();
        darkEnergy = darkForm.GetComponent<DarkEnergyManager>();

        // Thiết lập bắt đầu
        defaultForm.SetActive(true);
        darkForm.SetActive(false);
        camera.Follow = defaultForm.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            TrySwitchForm();
        }

        // Nếu đang ở dark form -> trừ năng lượng mỗi frame
        if (!isDefault)
        {
            darkFormTimer -= Time.deltaTime;
            darkEnergy.StartDrain();

            if (darkFormTimer <= 0f || darkEnergy.getDarkEnergy() <= 0f)
            {
                ForceSwitchToDefault();
            }
        }
        else
        {
            defaultEnergy.StopDrain();
        }
    }

    private void TrySwitchForm()
    {
        if (isDefault)
        {
            if (defaultEnergy.HasEnoughEnergy(10f))
            {
                darkFormTimer = defaultEnergy.getDarkEnergy() / energyCostPerSecond;
                SwitchForm(false);
            }
            else
            {
                Debug.Log("Not enough energy to enter dark form!");
            }
        }
        else
        {
            ForceSwitchToDefault();
        }
    }

    private void ForceSwitchToDefault()
    {
        SwitchForm(true);
    }

    private void SwitchForm(bool toDefault)
    {
        // Lưu vị trí giữ nguyên
        Vector3 currentPosition = isDefault ? defaultForm.transform.position : darkForm.transform.position;

        if (toDefault)
        {
            // Copy máu và năng lượng từ dark về default
            defaultHealth.healthChange(darkHealth);
            defaultEnergy.changeEnergy(darkEnergy);

            darkEnergy.StopDrain();
        }
        else
        {
            // Copy máu và năng lượng từ default sang dark
            darkHealth.healthChange(defaultHealth);
            darkEnergy.changeEnergy(defaultEnergy);

            darkEnergy.StartDrain();
        }

        isDefault = toDefault;

        defaultForm.SetActive(isDefault);
        darkForm.SetActive(!isDefault);

        if (isDefault)
        {
            currentPosition.y += -0.1900992f;
            defaultForm.transform.position = currentPosition;
            defaultMove.CopyStateFrom();
        }
        else
        {
            darkForm.transform.position = currentPosition;
            darkMove.CopyStateFrom();
        }

        camera.Follow = isDefault ? defaultForm.transform : darkForm.transform;
    }
}
