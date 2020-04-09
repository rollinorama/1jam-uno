using UnityEngine;
using System.Collections;

namespace SG.StateMachine
{
    public class PatrolState : State
    {
        public PatrolState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override void Update()
        {
            if (EnemyStateMachine._fieldOfView.visibleTargets.Count > 0)
                EnemyStateMachine.SetState(new AttackState(EnemyStateMachine));
            else if (EnemyStateMachine.isChasing)
            {
                EnemyStateMachine.isChasing = false;
            }
            EnemyStateMachine.EnemyPatrol();
        }

    }
}
