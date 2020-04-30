using UnityEngine;
using System.Collections;
using System;

namespace SG
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] AudioClip _audioTeleport;
        [SerializeField] public Transform _endPoint;
        [SerializeField] float _teleportTime;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void ShouldTeleport(Transform player, Action callback)
        {
            StartCoroutine(Co_Teleport(player, callback));
        }

        private IEnumerator Co_Teleport(Transform player, Action callback)
        {
            player.gameObject.SetActive(false);
            _audioSource.PlayOneShot(_audioTeleport);
            yield return new WaitForSeconds(_teleportTime);
            player.gameObject.SetActive(true);
            player.position = _endPoint.position;
            callback();
        }
    }
}
