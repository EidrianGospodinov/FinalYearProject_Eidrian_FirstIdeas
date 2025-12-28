using System;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Units.Enemy
{
    public class TwoDAnimationController : MonoBehaviour
    {
        private static readonly int VelocityZ = Animator.StringToHash("Velocity Z");
        private static readonly int VelocityX = Animator.StringToHash("Velocity X");
        [SerializeField] Transform target;
        private Animator animator;
        private NavMeshAgent agent;
        
        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
        }

        private void Update()
        {
            if(agent.desiredVelocity.magnitude < 0.01)
            {
                animator.SetFloat(VelocityZ, 0,0.1f, Time.deltaTime);
                animator.SetFloat(VelocityX, 0,0.1f, Time.deltaTime);
                return;
            }
            //Rotate to face the player manually
            Vector3 direction = (agent.destination - transform.position).normalized;
            direction.y = 0; // Keep the minotaur upright
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            //Convert World Velocity to Local Velocity
            Vector3 worldVelocity = agent.desiredVelocity;
            Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);

            
            animator.SetFloat(VelocityZ, localVelocity.z,0.1f, Time.deltaTime);
            animator.SetFloat(VelocityX, localVelocity.x,0.1f, Time.deltaTime);
        }
    }
}