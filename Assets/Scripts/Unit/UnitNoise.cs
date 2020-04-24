using UnityEngine;
using System.Collections;
using System;

namespace SG.Unit
{
    public class UnitNoise : MonoBehaviour
    {
        public event Action<Transform> NoiseEvent;

        [SerializeField] float _noiseRadius;
        [SerializeField] float _noiseTime;
        [SerializeField] LayerMask _enemyMask;

        private ParticleSystem _noiseParticles;

        private bool _makingNoise;

        private void Awake()
        {
            _noiseParticles = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            var em = _noiseParticles.emission;
            em.enabled = false;
        }

        public void Pulse()
        {
            StartCoroutine(Co_Pulse());
        }

        private IEnumerator Co_Pulse()
        {
            var em = _noiseParticles.emission;
            em.enabled = true;
            _makingNoise = true;
            yield return new WaitForSeconds(_noiseTime);
            em.enabled = false;
            _makingNoise = false;
        }

        private void FixedUpdate()
        {
            if (_makingNoise)
            {
                if(Physics2D.OverlapCircleAll(transform.position, _noiseRadius, _enemyMask).Length > 0)
                {
                    NoiseEvent(transform.parent);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _noiseRadius);
        }
    }
}
