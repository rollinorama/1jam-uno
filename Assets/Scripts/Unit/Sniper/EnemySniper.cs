using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace SG.Unit
{
    public class EnemySniper : MonoBehaviour
    {
        [SerializeField] Transform[] _waypoints;
        [SerializeField] float _idleTime;
        [SerializeField] float _movementSpeed;

        private Light2D _light2D;
        private UnitFieldOfView _fieldOfView;
        private UnitCombat _combat;

        private int _actualWaypointIndex = 0;
        private Transform _actualWaypoint;
        private bool _isAlerting;
        private bool _isRotating;
        private float _timer;
        private float _checkPlayerTime = .3f;

        private void Awake()
        {
            _light2D = GetComponentInChildren<Light2D>();
            _fieldOfView = GetComponentInChildren<UnitFieldOfView>();
            _combat = GetComponent<UnitCombat>();
        }

        private void FixedUpdate()
        {
            if (_fieldOfView.visibleTargets.Count > 0 && !_isAlerting)
            {
                StartCoroutine(Co_AlertTime());
            }

            SetMoveCamera();
        }

        private void SetMoveCamera()
        {
            if (!_isRotating)
                StartCoroutine(Co_Rotate());
        }

        private IEnumerator Co_Rotate()
        {
            _timer = _idleTime;
            _isRotating = true;
            _actualWaypointIndex = (_actualWaypointIndex + 1) % _waypoints.Length;
            _actualWaypoint = _waypoints[_actualWaypointIndex];
            while (_timer > 0)
            {
                _timer -= Time.deltaTime;
                Rotate();
                yield return null;
            }
            yield return new WaitForSeconds(_idleTime);
            _isRotating = false;
        }

        private void Rotate()
        {
            Vector3 targetDirection = _actualWaypoint.position - transform.position;

            float zRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion toRotation = Quaternion.Euler(new Vector3(0, 0, zRotation - 90));
            _light2D.transform.rotation = Quaternion.RotateTowards(_light2D.transform.rotation, toRotation, _movementSpeed * Time.deltaTime);
        }

        private IEnumerator Co_AlertTime()
        {
            _isAlerting = true;
            Color initialColor = _light2D.color;
            _light2D.color = Color.yellow;
            yield return new WaitForSeconds(_checkPlayerTime);
            if (_fieldOfView.visibleTargets.Count > 0)
            {
                SetCameraAction();
            }
            else
            {
                _isAlerting = false;
                _light2D.color = initialColor;
            }
        }

        private void SetCameraAction()
        {
            _light2D.color = Color.red;
            Transform player = FindObjectOfType<Player>().transform;

            if (_fieldOfView.visibleTargets[0].CompareTag("Player")) // Refatorar
            {
                _combat.ShouldAttack(); //PAREI AQUI!!!
            }
            else
            {
                Debug.Log("GameOver");
            }
        }
    }
}
