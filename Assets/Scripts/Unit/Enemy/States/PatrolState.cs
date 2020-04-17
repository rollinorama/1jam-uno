using UnityEngine;
using System.Collections;

namespace SG.StateMachine
{
    public class PatrolState : State
    {
        public PatrolState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            EnemyStateMachine.EnemyPatrol();
        }
    }
}
