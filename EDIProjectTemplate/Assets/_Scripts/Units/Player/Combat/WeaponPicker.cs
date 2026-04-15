using UnityEngine;

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
        }
    }
    
}
