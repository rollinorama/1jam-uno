using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG.StateMachine
{
    public class AttackState : State
    {
        private Transform _target;
        public AttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override IEnumerator Start()
        {
            if (EnemyStateMachine._fieldOfView.visibleTargets.Count > 0)
            {
                _target = EnemyStateMachine._fieldOfView.visibleTargets[0];
                EnemyStateMachine.EnemyChase(_target);
            }
            yield break;
        }

        public override void Update()
        {
            /*if (EnemyStateMachine._fieldOfView.visibleTargets.Count > 0)
            {
                if (Vector2.Distance(EnemyStateMachine.transform.position, _target.position) < EnemyStateMachine._attackDistance)
                {
                    EnemyStateMachine.EnemyAttack(_target);
                }
                else
                {
                    EnemyStateMachine.EnemyChase(_target);
                }
            }
            else if (!EnemyStateMachine.waitToPatrol)
            {
                EnemyStateMachine.EnemyWaitToPatrol();
            }*/
        }

    }
}
