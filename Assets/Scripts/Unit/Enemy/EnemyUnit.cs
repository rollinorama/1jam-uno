using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace SG.Unit
{
    public class EnemyUnit : MonoBehaviour, IUnit
    {
        [SerializeField] EnemyUnitState _unitState;
        [SerializeField] List<Transform> _waypoints;
        [SerializeField] float _attackDistance = 5f;


        private EnemyUnitPath _unitPath;
        private UnitCombat _combat;
        private Animator _animator;
        private Light2D _light2D;

        //Patrol Behaviour Vars
        private int _actualWaypointIndex = 0;
        private Transform _actualWaypoint;
        private bool _waitToWalk = false;
        private bool _isWalking = false;
        private float _walkingSpeed;

        public float WalkingSpeed { get; private set; }
        public float WalkingSpeedChase { get; private set; }
        public float IdleTime { get; private set; }
        public bool FacingRight { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
        public bool IsDead { get; set; } = false;

        public bool isPatrolling;

        private void Awake()
        {
            _unitPath = GetComponent<EnemyUnitPath>();
            _combat = GetComponentInChildren<UnitCombat>();
            _animator = GetComponentInChildren<Animator>();
            _light2D = GetComponentInChildren<Light2D>();
            
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
            if (IsDead) return;

            isPatrolling = true;
            StartCoroutine(Co_Patrol());
        }

        private IEnumerator Co_Patrol()
        {
            while (isPatrolling)
            {
                yield return new WaitForSeconds(!_isWalking ? IdleTime : .3f);
                if (_isWalking && Vector2.Distance(transform.position, _actualWaypoint.position) < 0.5f)
                {
                    _animator.SetBool("isWalking", false);
                    SetNextWaypoint();
                }
                else if (!_isWalking)
                {
                    _isWalking = true;
                    SetMove(_actualWaypoint, _walkingSpeed);
                    Rotate(_actualWaypoint);
                }
            }
            yield return null;
        }

        private void SetNextWaypoint()
        {
            _actualWaypointIndex = (_actualWaypointIndex + 1) % _waypoints.Count;
            _isWalking = false;
            _actualWaypoint = _waypoints[_actualWaypointIndex];

        }

        public void SetMove(Transform target, float walkingSpeed)
        {
            if (IsDead) return;

            _animator.SetBool("isWalking", true);
            _unitPath.RequestPath(target, walkingSpeed);
        }

        public void Rotate(Transform target)
        {
            Vector2 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(Vector3.back, direction);
            _light2D.transform.rotation = rotation;
            Flip(direction.x);
        }

        public void Move()
        {

        }

        public void EnemyAttack(Transform target)
        {
            if (IsDead) return;

            if (Vector2.Distance(transform.position, target.position) < _attackDistance)
            {
                SetMove(target, _walkingSpeed);
                _combat.ShouldAttack();
                Rotate(target);
            }
            else
            {
                SetMove(target, _walkingSpeed);
                Rotate(target);
            }

        }

        public void Attack()
        {

        }

        public void Dead()
        {
            IsDead = true;
            _light2D.enabled = false;
            gameObject.layer = 12;
            gameObject.tag = "EnemyDead";

            FindObjectOfType<GameManager>().EnemyDeath();
            _combat.DeathEvent -= Dead;
        }

        //REFATORAR!!!!
        private void Flip(float directionX)
        {
            if (directionX < 0 && FacingRight)
            {
                transform.GetChild(0).localScale = new Vector2(1f, transform.localScale.y);
                FacingRight = false;
            }
            else if (directionX > 0 && !FacingRight)
            {
                transform.GetChild(0).localScale = new Vector2(-1f, transform.localScale.y);
                FacingRight = true;
            }
        }

    }
}
