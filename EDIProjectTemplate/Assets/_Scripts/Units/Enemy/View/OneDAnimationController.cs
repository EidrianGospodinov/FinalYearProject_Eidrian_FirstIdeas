using System;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Units.Enemy
{
    public class OneDAnimationController : MonoBehaviour
    {
        private static readonly int VelocityHash = Animator.StringToHash("Velocity X");
        
        private Animator animator;
        private NavMeshAgent agent;
        
        [Header("Settings")]
        public float rotationSpeed = 10f;

        private void Start()
        {
           
            animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();

            
            agent.updateRotation = false;
        }

        private void Update()
        {
            if (agent.desiredVelocity.sqrMagnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(agent.desiredVelocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

            
            float speed = agent.velocity.magnitude;

            animator.SetFloat(VelocityHash, speed, 0.1f, Time.deltaTime);
        }
    }
}