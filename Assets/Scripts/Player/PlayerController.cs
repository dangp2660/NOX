using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
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
}
