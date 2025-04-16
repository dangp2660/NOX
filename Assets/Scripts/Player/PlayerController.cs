using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAttack))]
public class PlayerController : MonoBehaviour
{
    public Data PlayerData;
    private PlayerMovement movement;
    private PlayerAttack attack;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        attack.Initialize(PlayerData);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movement.OnMove(context);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        movement.OnRun(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        movement.OnJump(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        attack.HandleAttackInput(context);
    }
}
