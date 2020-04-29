using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class MusicBGManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private void Awake()
        {
            var musicBGManagers = FindObjectsOfType<MusicBGManager>();
            _audioSource = GetComponent<AudioSource>();
            if (musicBGManagers.Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        public void PlayAudio()
        {
            if (_audioSource.isPlaying == false)
                _audioSource.Play();
        }
        public void StopAudio()
        {
            _audioSource.Stop();
        }
    }
}
