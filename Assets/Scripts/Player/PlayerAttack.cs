using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    [Header("Spell Settings")]
    [SerializeField] private GameObject changingSpell;
    [SerializeField] private Transform changingPosition;
    [SerializeField] private float coolDown = 2f;

    private GameObject activeSpell;
    public float cooldownTimer = 0f;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
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
            if(movement.IsDash)
            {
                Debug.Log("cancel");
                animator.ResetTrigger(AnimationStringList.Attack1);
                animator.SetBool(AnimationStringList.isDash, true);
                return;
            }   
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
        if (context.started && movement.IsDash)
        {
            animator.SetBool(AnimationStringList.isDash, true);
        }
    }

    private void OnDestroy()
    {
        if (activeSpell != null)
        {
            Destroy(activeSpell, 0.5f);
        }
    }

    public void getCoolDown(PlayerAttack other)
    {
        this.cooldownTimer = other.cooldownTimer;
    }

    public void attackSlashSFX(AudioClip clip)
    {
        AudioManager.instance.playSFX(clip);
    }
}
