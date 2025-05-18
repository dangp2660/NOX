using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;
    private PlayerSwitch input;
    private PlayerOneWayPlatform oneWay;
    [SerializeField] private GameObject UIMenu;
    [SerializeField] private GameObject SettingUI;
    private void Awake()
    {
        oneWay = gameObject.GetComponent<PlayerOneWayPlatform>();
        input = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerSwitch>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
    }
    public void EnableSignal()
    {
        input.enabled = true;
    }

    public void DisableSignal()
    {
        input.enabled = false;
    }

    private void Start()
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement.OnMove(context);
    }

   

    public void OnJump(InputAction.CallbackContext context)
    {
        movement.OnJump(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        attack.HandleAttackInput(context);

    }
    public void OnMagicAttack(InputAction.CallbackContext context)
    {
        attack.HandleMagic(context);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        movement.OnDash(context);
    }
    public void DeathCheat(InputAction.CallbackContext context)
    {
        health.deadth(context);
    }
    public void OnReborn(InputAction.CallbackContext context)
    {
        health.OnRestart(context);
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            bool active = !UIMenu.activeSelf;
            UIMenu.SetActive(active);
            SettingUI.SetActive(false);
            if (!UIMenu.activeSelf)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
    }

    public void downPlatForm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCoroutine(oneWay.DisableCollision());
        }
    }
}
