using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private MeleeAttack MeleeAttack;
    private Data Data;
    private PlayerMovement PlayerMovement;
    public void Initialize(Data data)
    {
        this.Data = data;
    }

    private void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        MeleeAttack = GetComponent<MeleeAttack>();  

    }

    public void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStringList.Attack1);
            MeleeAttack.meleeAttack(Data.Dame);
            PlayerMovement.AttackPush();
        }
    }

    public void HandleMagic(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStringList.MagicAttack);
        }
    }
}
