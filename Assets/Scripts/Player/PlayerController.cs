using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
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
}
