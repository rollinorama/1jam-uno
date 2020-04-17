using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using System;

namespace SG.Unit
{
    public class UnitFieldOfView : MonoBehaviour
    {
        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask obstacleMask;

        public float viewRadius;
        [Range(0, 360)] public float viewAngle;
        //[HideInInspector]
        public List<Transform> visibleTargets = new List<Transform>();


        private Light2D _light2D;

        private void Awake()
        {
            _light2D = GetComponent<Light2D>();
            SetLight2D();
        }

        private void Start()
        {
            StartCoroutine("FindTargetsWithDelay", .2f);

        }

        private void SetLight2D()
        {
            _light2D.pointLightInnerAngle = viewAngle;
            _light2D.pointLightOuterAngle = viewAngle * 2;
            _light2D.pointLightInnerRadius = viewRadius;
            _light2D.pointLightOuterRadius = viewRadius;
        }

        private IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        private void FindVisibleTargets()
        {
            visibleTargets.Clear();
            Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector2 dirToTarget = (target.position - transform.position).normalized;
                if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector2.Distance(transform.position, target.position);

                    if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }


        public Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.z;
            }
            return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}