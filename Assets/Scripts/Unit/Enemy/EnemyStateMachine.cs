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
        public float _attackDistance = 5f;

        //Cached references
        [HideInInspector]
        public EnemyUnit _enemyUnit;
        [HideInInspector]
        public UnitFieldOfView _fieldOfView;
        [HideInInspector]
        public bool waitToPatrol = false;
        private EnemyUnitPath _unitPath;

        public bool isChasing = false;

        #endregion

        #region Execution
        void Awake()
        {
            Init();
        }

        private void FixedUpdate()
        {
            State.Update();
        }
        #endregion

        #region Specific methods

        private void Init()
        {
            _unitPath = GetComponent<EnemyUnitPath>();
            _fieldOfView = GetComponentInChildren<UnitFieldOfView>();
            _enemyUnit = GetComponent<EnemyUnit>();
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
                StartCoroutine(Co_ChaseTime(target, 0.4f));
            }
        }

        public void EnemyAttack(Transform target)
        {
            _enemyUnit.EnemyAttack(target);
        }


        public void EnemyWaitToPatrol()
        {
            StartCoroutine("Co_WaitToPatrol", _waitToPatrolTime);
        }

        private IEnumerator Co_ChaseTime(Transform target, float delay)
        {
            while (_fieldOfView.visibleTargets.Count > 0)
            {
                yield return new WaitForSeconds(delay);
                _enemyUnit.EnemyChase(target, true);
            }
        }

        private IEnumerator Co_WaitToPatrol(float delay)
        {
            waitToPatrol = true;
            yield return new WaitForSeconds(delay);
            if (_fieldOfView.visibleTargets.Count == 0)
            {
                SetState(new PatrolState(this));
            }
            waitToPatrol = false;
        }


        #endregion
    }
}
