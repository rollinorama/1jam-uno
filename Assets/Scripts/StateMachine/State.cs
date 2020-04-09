
using System.Collections;

namespace SG.StateMachine
{
    public abstract class State
    {
        protected EnemyStateMachine EnemyStateMachine;

        public State(EnemyStateMachine enemyStateMachine)
        {
            EnemyStateMachine = enemyStateMachine;
        }

        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual void Update()
        {
        }

    }
}
