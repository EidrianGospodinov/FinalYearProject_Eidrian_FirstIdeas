using UnityEngine;

namespace _Scripts.Units.Enemy
{
    public class UIHealthBar : MonoBehaviour
    {

        public Transform target;
        
        public Vector3 offset;
   
        void LateUpdate()
        {
            var mainCamTransform = Camera.main.transform;
            Vector3 direction= (target.position - mainCamTransform.position).normalized;
            bool isBehind=Vector3.Dot(direction,mainCamTransform.forward)<=0f;
            
            transform.position = Camera.main.WorldToScreenPoint(target.position+offset);
            transform.LookAt(transform.position + mainCamTransform.rotation * Vector3.forward,
                mainCamTransform.rotation * Vector3.up);
        }
        
    }
}