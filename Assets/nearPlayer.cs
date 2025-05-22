using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nearPlayer : MonoBehaviour
{
    [SerializeField] GameObject image;
    public Vector3 offset = new Vector3(0, 2f, 0); // Vị trí trên đầu NPC

    void LateUpdate()
    {
        if (image != null)
        {
            image.transform.position = transform.position + offset;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            image.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            image.SetActive(false);
        }
    }
}
