using System;
using UnityEngine;

namespace SG.Unit
{
    public class UnitCombat : MonoBehaviour
    {
        public event Action DeathEvent;

        [SerializeField] LayerMask enemyLayers;
        [SerializeField] float _attackRange;
        [SerializeField] float _shakeDuration = 0.3f;
        [SerializeField] float _shakeIntensity = 0.9f;

        private IUnit _unit;
        private Animator _animator;
        private MainCamera _camera;

        private void Awake()
        {
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
                DeathEvent();
            }
        }

        public void ShouldAttack()
        {
            _animator.SetTrigger("isAttacking");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _attackRange, enemyLayers);

            foreach (Collider2D enemy in colliders)
            {
                enemy.GetComponentInChildren<UnitCombat>().TakeDamage(_unit.Damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
