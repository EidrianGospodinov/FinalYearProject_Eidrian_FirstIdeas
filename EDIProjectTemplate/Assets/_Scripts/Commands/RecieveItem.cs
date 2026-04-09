using UnityEngine;

namespace _Scripts.Commands
{
    public class ReceiveItem : MonoBehaviour
    {
        
        [SerializeField] Transform receiveItemPlaceholderTransform;

        public void SetReceiveTransform(Transform receiveTransform)
        {
            receiveTransform.SetParent(receiveItemPlaceholderTransform);
            receiveTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            Debug.Log("Item received: "  + receiveTransform);
        }
    }
}