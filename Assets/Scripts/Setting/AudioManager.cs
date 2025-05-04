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
        
        public static AudioManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
    }
    private void Start()
        {   
            
            Music.clip = backgroud;
            Music.loop = true;
            Music.Play();
        }
        public void playSFX(AudioClip clip)
        {
            SFX.PlayOneShot(clip);
        }
    }
