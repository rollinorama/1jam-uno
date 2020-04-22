using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


namespace SG.Unit
{
    public class PlayerMovement : MonoBehaviour
    {
        private Player _unit;
        private Animator _animator;
        private Light2D _light2D;

        private void Awake()
        {
            _unit = GetComponent<Player>();
            _animator = GetComponentInChildren<Animator>();
            _light2D = GetComponentInChildren<Light2D>();
        }

        public void ShouldMove()
        {
            Animation();
            if (_unit.InputMovement == Vector2.zero) return;

            Move();
            Rotate();
            Flip();
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
                _light2D.transform.rotation = rotation;
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
                transform.GetChild(0).localScale = new Vector2(-1f, transform.localScale.y);
                _unit.FacingRight = false;
            }
            else if (_unit.InputMovement.x > 0 && !_unit.FacingRight)
            {
                transform.GetChild(0).localScale = new Vector2(1f, transform.localScale.y);
                _unit.FacingRight = true;
            }
        }
    }
}
