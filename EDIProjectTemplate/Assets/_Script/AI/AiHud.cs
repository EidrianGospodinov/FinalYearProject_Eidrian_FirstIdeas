using System;
using _Scripts.StateMachine.EnemyStatemMachine;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player;
using TMPro;
using UnityEngine;

namespace _Script.AI
{
    public class AiHud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statusText;

        private Health agentHealth;
        private AiAgent agent;
        
        private string agentID;
        private Transform mainCamTransform;
        private bool useAStar = false;

        private void Start()
        {
            mainCamTransform = Camera.main.transform;
            agent = GetComponentInParent<AiAgent>();
            agentHealth = GetComponentInParent<Health>();
            if (agentHealth == null || agent == null)
            {
                Debug.LogError("AiHUD does not have health or agent components.");
                return;
            }

            agentID = agent.agentConfig.name;


            UpdateDisplay();
        }
        void LateUpdate()
        {
           
            //keep the text facing the camera
            transform.LookAt(transform.position + mainCamTransform.rotation * Vector3.forward,
                mainCamTransform.rotation * Vector3.up);
            
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (agent.stateMachine == null)
            {
                return;
            }

            var stateId = agent.stateMachine.CurrentStateId;
            statusText.text =
                $"ID: {agentID + agent.GetUniqueID()}\n" +
                $"HP: {Mathf.CeilToInt(agentHealth.currentHealth)}\n" +
                $"STATE: {stateId}";




            //Change color based on HP percentage
            if (agentHealth.currentHealth <= agentHealth.maxHealth * 0.25f)
            {
                statusText.color = Color.red;
            }
            else if (stateId == EnemyStateId.AttackGeneric || stateId == EnemyStateId.Charge || stateId == EnemyStateId.ReadyToAttack) 
            {
                statusText.color = Color.yellow;
            }
            else
            {
                statusText.color = Color.white;
            }
        }
    }
}