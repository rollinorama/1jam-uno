﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG.DateSim;

namespace SG.Unit
{
    public class Player : MonoBehaviour, IUnit
    {
        [SerializeField] public PlayerUnitState unitState;
        [SerializeField] public float generalRange;

        private PlayerMovement _move;
        private UnitCombat _combat;
        private UnitGrab _grab;
        private UnitNoise _noise;
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
            _move = GetComponent<PlayerMovement>();
            _combat = GetComponentInChildren<UnitCombat>();
            _grab = GetComponent<UnitGrab>();
            _noise = GetComponentInChildren<UnitNoise>();
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
           Move();
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
            _cellPhone.OpenClosePhone();
        }

        public void MakeNoise()
        {
            _noise.Pulse();
        }

        public void Dead()
        {
            IsDead = true;

            _combat.DeathEvent -= Dead;
        }
    }
}
