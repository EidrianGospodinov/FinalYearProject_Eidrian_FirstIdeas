using System.Collections.Generic;
using System.Linq;
using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class ReadyToAttackState : IState<AiAgent, EnemyStateId>
    {
        public EnemyStateId GetId()
        {
            return EnemyStateId.ReadyToAttack;
        }

        public void Enter(AiAgent agent)
        {
            agent.navMeshAgent.isStopped = true;
            /*DecideNextAttack(agent);*/
        }

        public void Update(AiAgent agent)
        {

            if (agent.IsPlayerDetected())
            {

                DecideNextAttack(agent);
            }
            else
            {
                agent.stateMachine.ChangeState(EnemyStateId.Idle);
            }


        }

        public void Exit(AiAgent agent)
        {
            
        }

        private void DecideNextAttack(AiAgent agent)
        {
            var attackTypes = agent.agentConfig.EnemyAttackTypes;
            float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
    
            // Filter attacks that are valid for this distance
            var validAttacks = attackTypes.Where(a => 
                distanceToPlayer >= a.minDistance && 
                distanceToPlayer <= a.maxDistance
            ).ToList();

            if (validAttacks.Count > 0)
            {
                EnemyAttackTypesData chosenAttack = GetRandomAttackType(validAttacks);
                Debug.Log($"chosen attack: {chosenAttack}");
                //temp only charge
                if (chosenAttack != null)
                {
                    agent.stateMachine.ChangeState(EnemyStateId.Charge);
                }
            }
        }
        private EnemyAttackTypesData GetRandomAttackType(List<EnemyAttackTypesData> validAttacks)
        {
           
            // Calculate the total weight of all possible attacks
            int totalWeight = 0;
            foreach (var attack in validAttacks)
            {
                totalWeight += attack.weight;
            }

            int randomNumber = Random.Range(0, totalWeight);

            //Iterate through again to find which "slice" the random number landed in
            int currentWeightSum = 0;
            foreach (var attack in validAttacks)
            {
                currentWeightSum += attack.weight;
                if (randomNumber < currentWeightSum)
                {
                    return attack;
                }
            }

            return validAttacks[0]; // Fallback
        }
    }
}