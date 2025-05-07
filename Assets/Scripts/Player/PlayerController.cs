using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;
    private PlayerInput input;
    [SerializeField] private GameObject UIMenu;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
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
        if(context.performed)
        {
            bool active = !UIMenu.activeSelf;
            UIMenu.SetActive(active);
        }
    }
}   
