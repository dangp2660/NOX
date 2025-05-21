using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private BoxCollider2D boxCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player");
            animator.SetBool("OpenTrap", true);
            animator.SetBool("Hide", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DelayBeforeCloseTrap(1.5f));
        }
    }

    IEnumerator DelayBeforeCloseTrap(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("OpenTrap", false);
        animator.SetBool("Hide", true); // Giả sử "Hide" là trạng thái đóng trap
    }
}
