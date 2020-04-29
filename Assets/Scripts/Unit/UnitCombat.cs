using System;
using UnityEngine;

namespace SG.Unit
{
    public class UnitCombat : MonoBehaviour
    {
        public event Action DeathEvent;

        [SerializeField] AudioClip _audioAttack;
        [SerializeField] AudioClip _audioDeath;
        [SerializeField] LayerMask enemyLayers;
        [SerializeField] float _attackRange;
        [SerializeField] float _attackLeftPosition;
        [SerializeField] float _attackRightPosition;
        [SerializeField] float _shakeDuration = 0.3f;
        [SerializeField] float _shakeIntensity = 0.9f;

        private AudioSource _audioSource;
        private IUnit _unit;
        private Animator _animator;
        private MainCamera _camera;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _unit = GetComponentInParent<IUnit>();
            _animator = transform.parent.GetComponentInChildren<Animator>();
            _camera = FindObjectOfType<MainCamera>();
        }

        public void TakeDamage(float damageTaken)
        {
            _unit.Health -= damageTaken;
            _camera.ShakeCamera(_shakeDuration, _shakeIntensity);
            if (_unit.Health <= 0 && DeathEvent != null)
            {
                _audioSource.PlayOneShot(_audioDeath);
                _animator.SetBool("isDead", true);
                DeathEvent();
            }
        }

        public void ShouldAttack()
        {
            _animator.SetTrigger("isAttacking");
            _audioSource.PlayOneShot(_audioAttack);
            float positionX = _animator.transform.localScale.x > 0 ? transform.position.x + _attackRightPosition : transform.position.x + _attackLeftPosition;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(positionX, transform.position.y), _attackRange, enemyLayers);

            foreach (Collider2D enemy in colliders)
            {
                Attack(enemy.transform, _unit.Damage);
            }
        }

        public void Attack(Transform enemy, float damage)
        {
            enemy.GetComponentInChildren<UnitCombat>().TakeDamage(damage);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(new Vector2(transform.position.x + _attackRightPosition, transform.position.y), _attackRange);
            Gizmos.DrawWireSphere(new Vector2(transform.position.x + _attackLeftPosition, transform.position.y), _attackRange);
        }
    }
}
