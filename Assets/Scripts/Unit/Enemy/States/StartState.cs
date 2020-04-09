using System.Collections;
using UnityEngine;

namespace SG.StateMachine
{
    public class StartState : State
    {
        public StartState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
        {
        }

        public override IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            EnemyStateMachine.SetState(new PatrolState(EnemyStateMachine));
        }
    }
}
