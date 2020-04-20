using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG.Unit;

namespace SG.StateMachine
{
    public class EnemyStateMachine : StateMachine
    {
        #region Fields and props
        public float _waitToPatrolTime = 3f;

        //Cached references
        [HideInInspector]
        public EnemyUnit _enemyUnit;
        [HideInInspector]
        public UnitFieldOfView _fieldOfView;
        [HideInInspector]
        public bool waitToPatrol = false;
        private EnemyUnitPath _unitPath;
        private UnitNoise _noise;
        private IUnit _target;

        public bool isChasing = false;

        #endregion

        #region Execution
        void Awake()
        {
            Init();
        }

        private void FixedUpdate()
        {
            if (_fieldOfView.visibleTargets.Count > 0 && !isChasing)
            {
                SetState(new AttackState(this));
            }
        }
        #endregion

        #region Specific methods

        private void Init()
        {
            _unitPath = GetComponent<EnemyUnitPath>();
            _fieldOfView = GetComponentInChildren<UnitFieldOfView>();
            _enemyUnit = GetComponent<EnemyUnit>();
            _noise = FindObjectOfType<UnitNoise>(); //Get Player component
            _noise.NoiseEvent += SetChaseAndAttack;
            SetState(new StartState(this));
        }

        public void EnemyPatrol()
        {
            _enemyUnit.EnemyPatrol();
        }

        public void EnemyChase(Transform target)
        {
            if (!isChasing)
            {
                isChasing = true;
                StartCoroutine(Co_ChaseTime(target, 0.2f));
            }
        }

        public void ChaseAndAttack(Transform target)
        {
            StartCoroutine(Co_ChaseAndAttack(target, 0.4f));
        }


        private IEnumerator Co_ChaseTime(Transform target, float delay)
        {
            _enemyUnit.Rotate(target);
            yield return new WaitForSeconds(delay);
            if (_fieldOfView.visibleTargets.Count > 0)
            {
                StartCoroutine(Co_ChaseAndAttack(target, delay));
            }
            else
            {
                SetState(new PatrolState(this));
            }
        }

        private IEnumerator Co_ChaseAndAttack(Transform target, float delay)
        {
            _enemyUnit.isPatrolling = false;
            _target = target.GetComponent<IUnit>();
            while (!_target.IsDead)
            {
                yield return new WaitForSeconds(delay);
                _enemyUnit.EnemyAttack(target);
            }
            EnemyPatrol();
        }

        private void SetChaseAndAttack(Transform target)
        {
            StartCoroutine(Co_ChaseAndAttack(target, 0.4f));
            _noise.NoiseEvent -= SetChaseAndAttack;
        }
        #endregion
    }
}
