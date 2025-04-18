using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerAttack), typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    public Data PlayerData;

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
        attack.Initialize(PlayerData);
        health.Initialize(PlayerData); // ← thêm dòng này
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

    public void OnDash(InputAction.CallbackContext context)
    {
        movement.OnDash(context);
    }
}
