using UnityEngine;

namespace _Scripts.Units.Player.Combat
{
    public class WeaponPicker : MonoBehaviour
    {
        [SerializeField] private GameObject beam;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                var playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.FirstTimeEquipWeapon();
                beam.SetActive(false);
                EventBus<OnItemFound>.Trigger(new OnItemFound(SearchItemType.Sword));
            }
        }
    }

    public class OnItemFound : IEvent
    {
        public SearchItemType SearchItemTypeFound { get; private set; }
        public OnItemFound(SearchItemType searchItemType)
        {
            SearchItemTypeFound = searchItemType;
        }
    }

    public enum SearchItemType
    {
        Sword, 
        Guide
    }
}

