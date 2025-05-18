using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDoor : MonoBehaviour
{
    private bool isBroken = false;
    private Animator animator;
    private Collider2D doorCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        doorCollider = GetComponent<Collider2D>();
    }

    public void Break()
    {
        if (isBroken) return;
        isBroken = true;
        // Play break animation
        animator.SetTrigger("Break");

        AudioManager.instance.playSFX(AudioManager.instance.breakdoor);
        // Tắt collider sau khi animation kết thúc
        StartCoroutine(DisableColliderAfterAnimation());
    }

    private IEnumerator DisableColliderAfterAnimation()
    {
        // Chờ thời gian bằng với độ dài của animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        doorCollider.enabled = false;
    }
}
