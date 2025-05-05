using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    [Header("Spell Settings")]
    [SerializeField] private GameObject changingSpell;
    [SerializeField] private Transform changingPosition;
    [SerializeField] private float coolDown = 2f;

    private GameObject activeSpell;
    public float cooldownTimer = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Giảm cooldown mỗi frame
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStringList.Attack1);
        }
    }

    public void HandleMagic(InputAction.CallbackContext context)
    {
        if (context.started && cooldownTimer <= 0f)
        {
            Debug.Log("UseSpell");

            cooldownTimer = coolDown;

            if (changingSpell != null && changingPosition != null)
            {
                activeSpell = Instantiate(changingSpell, changingPosition.position, Quaternion.identity);
                Destroy(activeSpell, 2f);
            }

            animator.SetTrigger(AnimationStringList.MagicAttack);
        }
    }

    private void OnDestroy()
    {
        if (activeSpell != null)
        {
            Destroy(activeSpell, 0.5f);
        }
    }

    // (Tùy chọn) Hàm public để lấy % cooldown còn lại
    public float GetCooldownPercent()
    {
        return Mathf.Clamp01(cooldownTimer / coolDown);
    }

    public void getCoolDown(PlayerAttack other)
    {
        this.cooldownTimer = other.cooldownTimer;
    }
}
