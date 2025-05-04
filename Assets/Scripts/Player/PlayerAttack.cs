using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject changingSpell;
    [SerializeField] private Transform changingPosition;
    private GameObject activeSpell;
    [SerializeField] private float coolDown = 2f;
    private bool canUseSkill = true;


    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        

        if (context.started && canUseSkill)
        {
            Debug.Log("UseSpell");

            canUseSkill = false;

            if (changingSpell != null && changingPosition != null)
            {
                activeSpell = Instantiate(changingSpell, changingPosition.position, Quaternion.identity);
                Destroy(activeSpell, 2f);
            }

            animator.SetTrigger(AnimationStringList.MagicAttack);
            StartCoroutine(Delay(coolDown));
        }
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
       
        canUseSkill = true;
        Debug.Log(canUseSkill);
    }

    public void OnDestroy()
    {
        Destroy(activeSpell, 0.5f);
    }
    

}