using _Scripts.Units.Player;

namespace _Scripts.Units.Enemy
{
    public class EnemyHealth: Health
    {
        //AiAgent agent;
        protected override void OnStart()
        {
            //agent = GetComponent<AiAgent>();
            
        }
        protected override void OnDeath()
        {
           // if (agent != null)
            {

                //agent.stateMachine.ChangeState(AiStateId.Death);
            }

            Destroy(gameObject, 3f);
        }
        protected override void OnDamage()
        {

        }
    }
}