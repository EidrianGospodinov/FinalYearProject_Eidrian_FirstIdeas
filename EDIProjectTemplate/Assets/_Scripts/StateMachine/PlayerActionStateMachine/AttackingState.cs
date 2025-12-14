using System;
using _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class AttackingState :  IState<PlayerController, ActionStateId>
    {
        private StateMachine<PlayerController, ComboStateId> comboStateMachine;
        public StateMachine<PlayerController, ComboStateId> GetComboStateMachine => comboStateMachine;

        // State
        private int attackCount;

        public float ComboWindowTimer { get; private set; }
        private int attackSequenceID = 0;
        
        public ActionStateId GetId()
        {
            return ActionStateId.Attacking;
        }

        public void Enter(PlayerController agent)
        {
            Debug.Log("Attack state enter");
            comboStateMachine = new StateMachine<PlayerController, ComboStateId>(agent);
            
            comboStateMachine.RegisterState(new BasicAttackState());
            comboStateMachine.RegisterState(new ComboWindDownState());
            comboStateMachine.RegisterState(new SecondBasicAttack());
            comboStateMachine.RegisterState(new SpecialAttackComboState());
            
            comboStateMachine.Initialize(ComboStateId.BasicAttack);
            Attack(agent);
        }

        public void Update(PlayerController agent)
        {
            if (ComboWindowTimer > 0)
            {
                ComboWindowTimer -= Time.deltaTime;
            }

            comboStateMachine.Update();
            
            if (comboStateMachine.CurrentStateId == ComboStateId.WindDown)
            {
                var windDownState = comboStateMachine.GetState(ComboStateId.WindDown);
                var actualWindDownState = (ComboWindDownState)windDownState;
                if (actualWindDownState.IsTimerDone)
                {
                    agent.ActionStateMachine.ChangeState(ActionStateId.Ready);
                }
            }
        }

        public void Exit(PlayerController agent)
        {
            ComboWindowTimer = 0f;
        }

        private void Attack(PlayerController agent)
        {
            var attackData=agent.AttackData;
            int nextAttackID = 1 - attackSequenceID;
            agent.IsAttacking = true;

            // trigger attack anim based on sequence id
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, ComboStateId.BasicAttack));
            attackSequenceID = nextAttackID;
            

            // SFX
            /*agent.AudioSource.pitch = Random.Range(0.9f, 1.1f);
            agent.AudioSource.PlayOneShot(attackData.swordSwing);*/

        }
        public void ResetComboTimer(float duration)
        {
            ComboWindowTimer = duration;
        }
        
        
    }
}