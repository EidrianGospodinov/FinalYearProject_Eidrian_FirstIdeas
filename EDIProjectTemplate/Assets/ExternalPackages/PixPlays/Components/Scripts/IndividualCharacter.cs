using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PixPlays.ElementalVFX
{
    public class IndividualCharacter : MonoBehaviour
    {
        [SerializeField] Animator _Anim;
        [SerializeField] BindingPoints _BindingPoints;
        [SerializeField] Transform _Target;

        private AnimatorOverrideController _overrideController;
        public BindingPoints BindingPoints => _BindingPoints;

        private void Start()
        {
            if (_Anim.runtimeAnimatorController != null)
            {
                _overrideController = new AnimatorOverrideController(_Anim.runtimeAnimatorController);
                _Anim.runtimeAnimatorController = _overrideController;
            }
        }

        public void PlayAnimation(string clipId, AnimationClip clip)
        {
            if (_overrideController != null)
            {
                _overrideController[clipId] = clip;
                _Anim.SetTrigger("Play");
            }
        }

        public Transform GetTargetFallback()
        {
            Vector3 direction = (_Target.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                return hit.transform;
            }

            return _Target;
        }

        public Transform GetClosestEnemy(float radius)
        {
            // Only check objects on the "Enemy" layer for performance
            LayerMask enemyLayer = LayerMask.GetMask("Enemy");
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);

            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToTarget = hitCollider.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = hitCollider.transform;
                }
            }

            return bestTarget;
        }

        public bool HasLineOfSight(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, target.position);

            // If the ray hits something BEFORE the target, LOS is blocked
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance))
            {
                if (hit.transform == target) return true;
            }

            return false;
        }
    }
}