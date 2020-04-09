using UnityEngine;

namespace SG.Unit
{
    public class UnitCombat : MonoBehaviour
    {
        [SerializeField] LayerMask enemyLayers;
        [SerializeField] float _attackRange;
        [SerializeField] float _shakeDuration = 0.3f;
        [SerializeField] float _shakeIntensity = 0.9f;

        private IUnit _unit;
        private MainCamera _camera;

        private void Awake()
        {
            _unit = GetComponentInParent<IUnit>();
            _camera = FindObjectOfType<MainCamera>();
        }

        public void TakeDamage(float damageTaken)
        {
            _unit.Health -= damageTaken;
            _camera.ShakeCamera(_shakeDuration, _shakeIntensity);
            Debug.Log("TakeDamage " + _unit.Health);
            if (_unit.Health <= 0)
            {
                Die();
            }
        }

        public void Attack()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _attackRange, enemyLayers);

            foreach (Collider2D enemy in colliders)
            {
                enemy.GetComponentInChildren<UnitCombat>().TakeDamage(_unit.Damage);
            }
        }

        private void Die()
        {

            _unit.Dead();
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
