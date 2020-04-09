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

        [HideInInspector]
        public Vector2 Movement { get; set; }

        public float WalkingSpeed { get; private set; }
        public bool FacingRight { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
        public bool IsDead { get; set; } = false;

        private void Awake()
        {
            _move = GetComponent<PlayerMovement>();
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

        private void OnMove(InputValue value)
        {
            Movement = value.Get<Vector2>();
        }


        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Dead()
        {
            IsDead = true;
        }
    }
}
