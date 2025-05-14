using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOneWay : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
            {
                this.GetComponent<PlatformEffector2D>().rotationalOffset = 180; 
            }
        }
    }
}
