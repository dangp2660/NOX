    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerHealth : MonoBehaviour
    {
        private Damageable Damegeable;
        private Animator  animator;
        private DeathFade deathFade;
        [SerializeField] private GameObject blood;
        private bool isRespawning = false;
        private bool canRespawn = false;
    // Update is called once per frame
    private void Awake()
    {
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
        Die();
    }
        
        
        
    public void deadth(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Damegeable.TakeDamage(10000000, 1);
        }
    }

    public void Die()
    {
        if (!isAlive() && !isRespawning)
        {
            isRespawning = true;
            if(AudioManager.instance != null) 
                AudioManager.instance.playSFX(AudioManager.instance.dealth);
            animator.SetBool(AnimationStringList.isAlive, false);
            deathFade.showDeathScreen();

            GameObject[] allObject = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObject)
            {
                if (!obj.CompareTag("Untagged"))
                {
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.color = Color.black;
                    }
                }
            }
            blood.SetActive(true);
            StartCoroutine(Delay());
        }
    }

    private void respawnPlayer()
    {
        ResetHealth();
        RespawnScript respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnScript>();
        respawn.RespawnPlayer();    
        isRespawning = false;
        animator.SetBool(AnimationStringList.isAlive, true);
        ResetSprite();
        Time.timeScale = 1;
    }

    private void ResetSprite()
    {
        blood.SetActive(false);
        deathFade.HideDeathScreen();
        GameObject[] allObj = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObj)
        {
            SpriteRenderer sp = obj.GetComponent<SpriteRenderer>();
            if (sp != null)
            {
                sp.color = Color.white;
            }
        }
    }
    public void ResetHealth()
    {
        Damegeable.CurrentHealth = Damegeable.getMaxHealth();
        Damegeable.IsAlive = true;
        canRespawn = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
        canRespawn = true;
    }
    public void OnRestart(InputAction.CallbackContext context)
    {
        if(canRespawn && context.started && !Damegeable.IsAlive)
        {
            respawnPlayer();
        }
    }
}
