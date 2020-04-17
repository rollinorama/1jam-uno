using UnityEngine;
using System.Collections;
using System;

namespace SG.Unit
{
    public class UnitTeleport : MonoBehaviour
    {
        [SerializeField] LayerMask _teleportLayers;
        [SerializeField] float _teleportDelay;
        [SerializeField] float _teleportTime;

        private Player _player;
        private Collider2D _teleportable;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            StartCoroutine(Co_CheckTeleport());
        }


        private IEnumerator Co_CheckTeleport()
        {
            while (true)
            {
                Collider2D teleport = Physics2D.OverlapCircle(transform.position, _player.generalRange, _teleportLayers);
                _teleportable = teleport;
                yield return new WaitForSeconds(_teleportDelay);
            }
        }

        public void CheckTeleport()
        {
            StartCoroutine(Co_CheckTeleport());
        }

        public void ShouldTeleport()
        {
            if (_teleportable != null)
            {
                _teleportable.GetComponent<Teleport>().ShouldTeleport(transform, CheckTeleport);
                _teleportable = null;
            }
        }
    }
}
