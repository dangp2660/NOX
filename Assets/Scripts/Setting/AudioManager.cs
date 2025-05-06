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
            if (instance != null)
            {
                if (instance.backgroud == this.backgroud)
                {
                    Destroy(gameObject); // Giữ bản cũ vì cùng nhạc
                }
                else
                {
                    Destroy(instance.gameObject); // Hủy bản cũ nếu khác nhạc
                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {   
            
                Music.clip = backgroud;
                Music.loop = true;
                Music.Play();
        }
        public void playSFX(AudioClip clip)
        {
            if (SFX != null && clip != null)
            {
                SFX.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning("SFX or clip is null");
            }
        }
    }
