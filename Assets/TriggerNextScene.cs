using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNextScene : MonoBehaviour
{
    NextScene nextScene;
    private void Start()
    {
        nextScene = GetComponent<NextScene>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nextScene.enabled = true;
        }
    }
}
