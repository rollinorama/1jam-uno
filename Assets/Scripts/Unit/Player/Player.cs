using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SG.Unit
{
    public class Player : MonoBehaviour, IUnit
    {
        [SerializeField] public PlayerUnitState unitState;
        private PlayerMovement _move;
        private UnitCombat _combat;

        [HideInInspector]
        public Vector2 InputMovement { get; set; }
        public float InputAttack { get; set; }

        public float WalkingSpeed { get; private set; }
        public bool FacingRight { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
        public bool IsDead { get; set; } = false;

        private void Awake()
        {
            _move = GetComponent<PlayerMovement>();
            _combat = GetComponentInChildren<UnitCombat>();
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
            Move();
        }

        public void Move()
        {
            if (IsDead) return;

            _move.ShouldMove();
        }

        public void Attack()
        {
            _combat.Attack();
        }

        public void Dead()
        {
            IsDead = true;
        }

        #region Input System

        private void OnMove(InputValue value)
        {
            InputMovement = value.Get<Vector2>();
        }

        private void OnAttack(InputValue value)
        {
            if (value.Get<float>() > 0)
                Attack();
        }
        #endregion

    }
}
