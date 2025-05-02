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
            }

        }

    }
