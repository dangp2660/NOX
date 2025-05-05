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
            if (context.started && SpellCooldownManager.Instance.CanUseSpell())
            {
                Debug.Log("UseSpell");

                SpellCooldownManager.Instance.TriggerCooldown();

                if (changingSpell != null && changingPosition != null)
                {
                    GameObject spell = Instantiate(changingSpell, changingPosition.position, Quaternion.identity);
                    Destroy(spell, 2f);
                }

                animator.SetTrigger(AnimationStringList.MagicAttack);
            }
        }


        public void OnDestroy()
        {
            Destroy(activeSpell, 0.5f);
        }
    

    }