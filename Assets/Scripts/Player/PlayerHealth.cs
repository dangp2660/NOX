    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerHealth : MonoBehaviour
    {
        private Damageable Damegeable;
        private Animator  animator;
        private PlayerMovement movement;
        private DeathFade deathFade;
        [SerializeField] private GameObject blood;
        // Update is called once per frame
        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            animator = GetComponent<Animator>();
            Damegeable = GetComponent<Damageable>();

            deathFade = FindAnyObjectByType<DeathFade>();
        }
        public bool isAlive()
        {
            return Damegeable.IsAlive;
        }
        private void Update()
        {
            if (!isAlive())
            {
                animator.SetBool(AnimationStringList.isAlive, false);
                deathFade.showDeathScreen();
                GameObject[] allObject = GameObject.FindObjectsOfType<GameObject>();
                foreach (GameObject obj in allObject)
                {
                    if (obj.CompareTag("Player") || obj.CompareTag("Enemy") || obj.CompareTag("Ground"))
                    {
                        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                        if (sr != null)
                        {
                            sr.color = Color.black; // Đổi màu thành đen
                            blood.SetActive(true);
                        }
                    }
                }
            }
        }

        public void deadth(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                Damegeable.TakeDamage(10000000, 1);
            }
        }
        
    }
