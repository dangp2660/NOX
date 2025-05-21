using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 
 
public class SpellCoolDown : MonoBehaviour 
{ 
    [SerializeField] private Image ImageCoolDown; 
    [SerializeField] protected TMP_Text TextCoolDown; 
 
    [SerializeField] private float coolDownTime = 3f; 
    private bool isCoolDown = false; 
    private float coolDownTimer = 0f; 
    
    // Start is called before the first frame update 
    void Start() 
    { 
        TextCoolDown.gameObject.SetActive(false); 
        ImageCoolDown.fillAmount = 0f; 
    } 
 
    // Update is called once per frame 
    void Update() 
    { 
        if (isCoolDown)
        {
            ApplyCoolDown();
        }
    } 
 
    public void UseSpell()
    {
        if (!isCoolDown)
        {
            // Spell cast logic would go here
            
            // Start cooldown
            isCoolDown = true;
            TextCoolDown.gameObject.SetActive(true);
            coolDownTimer = coolDownTime;
        }
    }
    
    private void ApplyCoolDown() 
    { 
        coolDownTimer -= Time.deltaTime; 
        if(coolDownTimer <= 0) 
        { 
            isCoolDown = false; 
            TextCoolDown.gameObject.SetActive(false); 
            ImageCoolDown.fillAmount = 0.0f; 
        } 
        else 
        { 
            TextCoolDown.text = Mathf.RoundToInt(coolDownTimer).ToString(); 
            ImageCoolDown.fillAmount = coolDownTimer / coolDownTime; 
        } 
    } 
}