using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG.DateSim;

namespace SG.Unit
{
    public class Player : MonoBehaviour, IUnit
    {
        public event Action<Transform> NoiseEvent;

        [SerializeField] public PlayerUnitState unitState;
        [SerializeField] public float generalRange;

        private GameManager _gameManager;
        private PlayerMovement _move;
        private UnitCombat _combat;
        private UnitGrab _grab;
        private UnitNoise _noise;
        private Animator _animator;
        private UnitTeleport _teleport;
        private CellPhone _cellPhone;

        [HideInInspector]
        public Vector2 InputMovement { get; set; }

        public float WalkingSpeed { get; private set; }
        public bool FacingRight { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
        public bool IsDead { get; set; } = false;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();

            _move = GetComponent<PlayerMovement>();
            _combat = GetComponentInChildren<UnitCombat>();
            _grab = GetComponent<UnitGrab>();
            _noise = GetComponentInChildren<UnitNoise>();
            _animator = GetComponentInChildren<Animator>();
            _teleport = GetComponent<UnitTeleport>();
            _cellPhone = FindObjectOfType<CellPhone>();

            _cellPhone.RingEvent += MakeNoise;
            _combat.DeathEvent += Dead;
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            WalkingSpeed = unitState._walkingSpeed;
            FacingRight = unitState._facingRight;
            Health = unitState._health;
            Damage = unitState._damage;
        }

        private void FixedUpdate()
        {
            if (IsDead) return;

            Move();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("EndWaypoint"))
            {
                _gameManager.LoadNextScene(other.transform);
            }
        }

        public void Move()
        {
            _move.ShouldMove();
        }

        public void Attack()
        {
            _combat.ShouldAttack();
        }

        public void Grab()
        {
            _grab.ShouldGrab();
        }

        public void Teleport()
        {
            _teleport.ShouldTeleport();
        }

        public void OpenClosePhone()
        {
            _animator.SetBool("isOnPhone", !_cellPhone.openedPhone);
            _cellPhone.OpenClosePhone();
        }

        public void MakeNoise()
        {
            _noise.Pulse();
        }

        public void Dead()
        {
            IsDead = true;

            _gameManager.PlayerDeath();
            _combat.DeathEvent -= Dead;
        }
    }
}
