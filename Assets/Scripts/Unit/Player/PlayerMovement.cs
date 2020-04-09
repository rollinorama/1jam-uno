using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG.Unit
{
    public class PlayerMovement : MonoBehaviour
    {
        private Player _unit;
        private Animator _animator;


        private void Awake()
        {
            _unit = GetComponent<Player>();
            _animator = GetComponent<Animator>();
        }

        public void ShouldMove()
        {
            Move();
            Rotate();
            //Animation();
            //Flip();
        }

        private void Move()
        {
            transform.Translate(_unit.InputMovement * _unit.WalkingSpeed * Time.deltaTime, Space.World);
        }

        private void Rotate()
        {
            if (_unit.InputMovement.x != 0 || _unit.InputMovement.y != 0)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.back, _unit.InputMovement);
                transform.rotation = rotation;
            }
        }

        private void Animation()
        {
            if (_unit.InputMovement.x == 0 && _unit.InputMovement.y == 0)
                _animator.SetBool("isWalking", false);
            else
                _animator.SetBool("isWalking", true);
        }

        private void Flip()
        {
            if (_unit.InputMovement.x < 0 && _unit.FacingRight)
            {
                transform.GetChild(0).localScale = new Vector2(1f, transform.localScale.y);
                _unit.FacingRight = true;
            }
            else if (_unit.InputMovement.x > 0 && !_unit.FacingRight)
            {
                transform.GetChild(0).localScale = new Vector2(-1f, transform.localScale.y);
                _unit.FacingRight = false;
            }
        }
    }
}
