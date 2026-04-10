using _Scripts.StateMachine.EnemyStatemMachine;
using _Scripts.Units.Player;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Units.Enemy
{
    public class EnemyHealth: Health
    {
        [Inject] private CurrencyManager currencyManager;
        AiAgent agent;
        public Slider healthBar;
        protected override void OnStart()
        {
            agent = GetComponent<AiAgent>();
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
        protected override void OnDeath()
        {
            currencyManager.AddCurrency(10);
            Destroy(gameObject, agent.agentConfig.DeathAnimDuration);
            if (agent != null)
            {
                agent.stateMachine.ChangeState(EnemyStateId.Death);
            }
        }
        protected override void OnDamage()
        {
            healthBar.value = currentHealth;
        }
    }
}