using UnityEngine;

namespace SG.Unit
{
    public class UnitCombat : MonoBehaviour
    {
        [SerializeField] LayerMask enemyLayers;
        [SerializeField] float _attackRange;

        private IUnit _unit;

        private void Awake()
        {
            _unit = GetComponentInParent<IUnit>();
        }

        public void TakeDamage(float damageTaken)
        {
            _unit.Health -= damageTaken;
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


        private void OnDrawGizmosSeleted()
        {
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
