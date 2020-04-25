using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG.Unit
{
    public class UnitGrab : MonoBehaviour
    {
        [SerializeField] LayerMask enemyLayers;
        [SerializeField] float _checkGrabTime;
        [SerializeField] float _grabRange;

        private Animator _animator;

        private bool _isEnemyGrabbed = false;
        private List<Transform> _enemiesGrabbable = new List<Transform>();
        private GameObject _enemyGrabbed;
        private PlayerUI _playerUI;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _playerUI = GetComponentInChildren<PlayerUI>();
        }

        private void Start()
        {
            StartCoroutine(Co_CheckGrab());
        }

        private IEnumerator Co_CheckGrab()
        {
            while (!_isEnemyGrabbed)
            {
                yield return new WaitForSeconds(_checkGrabTime);
                CheckGrabbable();
            }
            yield return null;
        }


        public void CheckGrabbable()
        {
            Collider2D[] _enemies = Physics2D.OverlapCircleAll(transform.position, _grabRange, enemyLayers);
            if (_enemies.Length > 0)
            {

                foreach (Collider2D enemy in _enemies)
                {
                    if (enemy.GetComponent<IUnit>().IsDead)
                        _enemiesGrabbable.Add(enemy.transform);
                }
                _playerUI.OpenUI("X", "Agarrar Corpo", PlayerUIButtonType.Grab);
            }
            else
            {
                _enemiesGrabbable.Clear();
                _playerUI.CloseUI(PlayerUIButtonType.Grab);
            }
        }

        public void ShouldGrab()
        {
            if (_isEnemyGrabbed)
            {
                Drop();
            }
            else if (_enemiesGrabbable.Count > 0)
            {
                Grab();
            };
        }

        private void Grab()
        {
            _enemyGrabbed = CheckNearestEnemy();
            _enemyGrabbed.SetActive(false);
            _isEnemyGrabbed = true;
            _animator.SetBool("isGrabbing", _isEnemyGrabbed);
            _enemiesGrabbable.Clear();
            StopCoroutine(Co_CheckGrab());
        }

        private void Drop()
        {
            _enemyGrabbed.transform.position = new Vector2(transform.position.x, transform.position.y + 0.4f);
            _enemyGrabbed.SetActive(true);
            _isEnemyGrabbed = false;
            _animator.SetBool("isGrabbing", _isEnemyGrabbed);
            _enemyGrabbed.GetComponentInChildren<Animator>().SetBool("isDead", true);
            StartCoroutine(Co_CheckGrab());
        }

        private GameObject CheckNearestEnemy()
        {
            Vector3 currentPos = transform.position;
            Transform nearestEnemy = null;
            float minDist = Mathf.Infinity;
            foreach (Transform enemy in _enemiesGrabbable)
            {
                float dist = Vector3.Distance(enemy.position, currentPos);
                if (dist < minDist)
                {
                    nearestEnemy = enemy;
                    minDist = dist;
                }
            }
            return nearestEnemy.gameObject;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _grabRange);
        }
    }
}

