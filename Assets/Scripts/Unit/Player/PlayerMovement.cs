using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


namespace SG.Unit
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] AudioClip _audioMovement;
        private Player _unit;
        private Animator _animator;
        private Light2D _light2D;
        private AudioSource _audioSource;

        private void Awake()
        {
            _unit = GetComponent<Player>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponentInChildren<Animator>();
            _light2D = GetComponentInChildren<Light2D>();
        }

        private void Sound()
        {
            if (_unit.InputMovement == Vector2.zero)
                _audioSource.Stop();
            else if (_audioSource.isPlaying == false)
            {
                _audioSource.clip = _audioMovement;
                _audioSource.Play();
            }

        }

        public void ShouldMove()
        {
            Animation();
            Sound();
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
