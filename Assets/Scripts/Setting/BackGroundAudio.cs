using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip limgraveClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = limgraveClip;
        audioSource.loop = true;
        audioSource.volume = 0.35f;
        audioSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
