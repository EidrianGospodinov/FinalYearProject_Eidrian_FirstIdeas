using _Scripts.Units.Player.Core;
using UnityEngine;

public class CampFire : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject skillTree;
    [SerializeField] private GameObject flames;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        skillTree.SetActive(true);
    }

    public void ActivateFire(bool activate)
    {
        flames.SetActive(activate);
    }
}
