using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG.Unit
{
    public class EnemyUnit : MonoBehaviour, IUnit
    {
        [SerializeField] EnemyUnitState _unitState;
        [SerializeField] List<Transform> _waypoints;


        private EnemyUnitPath _unitPath;
        private UnitCombat _combat;

        //Patrol Behaviour Vars
        private Transform _actualWaypoint;
        private bool _waitToWalk = false;
        private bool _isWalkingToPath = false;
        private Status _status = Status.Patrol;
        private float _walkingSpeed;
        private Queue<Transform> _waypointQueue;

        public float WalkingSpeed { get; private set; }
        public float WalkingSpeedChase { get; private set; }
        public float IdleTime { get; private set; }
        public bool FacingRight { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
        public bool IsDead { get; set; } = false;

        //TEMP
        private SpriteRenderer _sprite;

        private void Awake()
        {
            _unitPath = GetComponent<EnemyUnitPath>();
            _combat = GetComponentInChildren<UnitCombat>();
            _sprite = GetComponent<SpriteRenderer>();
            _combat.DeathEvent += Dead;
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            InitStates();
            transform.position = _waypoints[0].position;
            _waypointQueue = new Queue<Transform>(_waypoints);
            _walkingSpeed = WalkingSpeed;
            SetNextWaypoint();
        }

        private void InitStates()
        {
            WalkingSpeed = _unitState._walkingSpeed;
            WalkingSpeedChase = _unitState._walkingSpeedChase;
            IdleTime = _unitState._idleTime;
            FacingRight = _unitState._facingRight;
            Health = _unitState._health;
            Damage = _unitState._damage;
        }

        public void EnemyPatrol()
        {
            if (_waitToWalk || IsDead) return;

            if (Vector2.Distance(transform.position, _actualWaypoint.position) < 0.5f)
            {
                StartCoroutine("Co_IdlePatrol");
            }
            else
            {
                SetMove(_actualWaypoint, 0);
                Rotate(_actualWaypoint);
            }
        }

        public void EnemyChase(Transform target, bool rePath)
        {
            if (IsDead) return;

            SetMove(target, 1, rePath);
            Rotate(target);
        }

        public void SetMove(Transform target, int status, bool rePath = false)
        {
            CheckStatus(status, rePath);
            if (!_isWalkingToPath)
            {
                _isWalkingToPath = true;
                _unitPath.RequestPath(target, _walkingSpeed);
            }
        }

        public void Move()
        {

        }

        private void CheckStatus(int status, bool rePath)
        {
            if (_status != (Status)status || rePath)
            {
                _isWalkingToPath = false;
                _status = (Status)status;
                _walkingSpeed = status != 0 ? WalkingSpeedChase : WalkingSpeed;
                if (status != 0)
                {
                    StopCoroutine("Co_IdlePatrol");
                }
            }
        }

        private IEnumerator Co_IdlePatrol()
        {
            _waitToWalk = true;
            yield return new WaitForSeconds(IdleTime);
            SetNextWaypoint();
            _waitToWalk = false;
            _isWalkingToPath = false;
        }

        private void SetNextWaypoint()
        {
            _actualWaypoint = _waypointQueue.Dequeue();
            _waypointQueue.Enqueue(_actualWaypoint);

        }


        private void Rotate(Transform target)
        {
            Vector2 direction = transform.position - target.position;
            float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + 90);
        }

        public void EnemyAttack(Transform target)
        {
            if (IsDead) return;

            _combat.ShouldAttack();
            Rotate(target);
        }

        private enum Status
        {
            Patrol,
            Chase,
            Attack
        }

        public void Attack()
        {

        }

        public void Dead()
        {
            IsDead = true;
            _sprite.color = Color.green;

            _combat.DeathEvent -= Dead;
        }

    }
}
