using UnityEngine;

public class WeaponPicker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.FirstTimeEquipWeapon();
        }
    }
    
}
