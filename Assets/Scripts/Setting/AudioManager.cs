    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource Music;
        [SerializeField] private AudioSource SFX;

        [Header("Audio Clip")]
        public AudioClip backgroud;
        public AudioClip dealth;
        public AudioClip checkPoint;
       
    private void Start()
        {
            Music.clip = backgroud;
            Music.Play();
        }
        public void playSFX(AudioClip clip)
        {
            SFX.PlayOneShot(clip);
        }
    }
