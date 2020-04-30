using System.Collections;
using UnityEngine;
using SG.Path;

namespace SG.Unit
{
    public class EnemyUnitPath : MonoBehaviour
    {
        private float WalkingSpeed { get; set; }
        Vector2[] _path;
        int _targetIndex;
        public bool unitDead = false;

        public void RequestPath(Transform target, float walkingSpeed)
        {
            WalkingSpeed = walkingSpeed;
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }

        private void OnPathFound(Vector2[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                _path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        public IEnumerator FollowPath()
        {
            _targetIndex = 0;
            Vector3 currentWaypoint = _path[0];

            while (!unitDead)
            {
                if (transform.position == currentWaypoint)
                {
                    _targetIndex++;
                    if (_targetIndex >= _path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = _path[_targetIndex];
                }
                transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, WalkingSpeed * Time.deltaTime);
                yield return null;
            }
            yield return null;

        }

        public void OnDrawGizmos()
        {
            if (_path != null)
            {
                for (int i = _targetIndex; i < _path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(_path[i], new Vector2(0.25f, 0.25f));

                    if (i == _targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, _path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(_path[i - 1], _path[i]);
                    }
                }
            }
        }
    }
}
