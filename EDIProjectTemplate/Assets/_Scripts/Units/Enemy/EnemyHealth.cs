using _Scripts.Units.Player;
using UnityEngine.UI;

namespace _Scripts.Units.Enemy
{
    public class EnemyHealth: Health
    {
        //AiAgent agent;
        public Slider healthBar;
        protected override void OnStart()
        {
            //agent = GetComponent<AiAgent>();
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
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
            healthBar.value = currentHealth;
        }
    }
}