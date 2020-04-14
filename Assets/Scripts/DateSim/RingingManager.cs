using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG.DateSim
{
    public class RingingManager : MonoBehaviour
    {
        [SerializeField] CellPhone _cellphone;
        [SerializeField] float _waypointRadius;
        [SerializeField] float _checkCollideTime;
        [SerializeField] LayerMask _playerMask;

        private List<Transform> _ringingWaypoints = new List<Transform>();

        private void Start()
        {
            foreach (Transform child in transform)
            {
                _ringingWaypoints.Add(child);
            }
            StartCoroutine(Co_CheckRingingCollide());
        }

        private IEnumerator Co_CheckRingingCollide()
        {
            while (_ringingWaypoints.Count > 0)
            {
                yield return new WaitForSeconds(_checkCollideTime);
                for (int i = 0; i < _ringingWaypoints.Count; i++)
                {
                    if (Physics2D.OverlapCircle(_ringingWaypoints[i].position, _waypointRadius, _playerMask) != null)
                    {
                        _cellphone.SetRing();
                        _ringingWaypoints.Remove(_ringingWaypoints[i]);
                        Debug.Log("message");

                    }
                }
            }
            yield return null;
        }

        private void OnDrawGizmosSelected()
        {
            foreach (Transform rw in _ringingWaypoints)
            {
                Gizmos.DrawWireSphere(rw.position, _waypointRadius);
            }
        }
    }
}
