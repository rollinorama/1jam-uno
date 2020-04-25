using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
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
        private UnitNoise _noise;
        private IUnit _target;
        private Light2D _light2D;

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
            _fieldOfView = GetComponentInChildren<UnitFieldOfView>();
            _light2D = GetComponentInChildren<Light2D>();
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
                _light2D.color = Color.yellow;
                StartCoroutine(Co_ChaseTime(target, 0.2f));
            }
        }

        public void ChaseAndAttack(Transform target, float delay = 0.4f)
        {
            _enemyUnit.isPatrolling = false;
            _target = target.GetComponent<IUnit>();
            Transform _targetCollider = target.Find("WalkCollider");
            StartCoroutine(Co_ChaseAndAttack(_targetCollider, delay)); //REFATORAR URGENTE!!!
        }

        public void AttackByAlarm(Transform target)
        {
            SetState(new AttackState(this));
            ChaseAndAttack(target);
        }


        private IEnumerator Co_ChaseTime(Transform target, float delay)
        {
            _enemyUnit.Rotate(target);
            yield return new WaitForSeconds(delay);
            if (_fieldOfView.visibleTargets.Count > 0)
            {
                _light2D.color = Color.red;
                ChaseAndAttack(target, delay);
            }
            else
            {
                SetState(new PatrolState(this));
            }
        }

        private IEnumerator Co_ChaseAndAttack(Transform target, float delay)
        {
            while (!_target.IsDead)
            {
                yield return new WaitForSeconds(delay);
                _enemyUnit.EnemyAttack(target);
            }
            EnemyPatrol();
        }

        private void SetChaseAndAttack(Transform target)
        {
            ChaseAndAttack(target, 0.4f);
            _noise.NoiseEvent -= SetChaseAndAttack;
        }
        #endregion
    }
}
