using _Scripts;
using _Scripts.Units.Player.Core;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;

    [SerializeField] private Interactor interactor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        if (interactor.GetInteractableObject() != null)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        containerGameObject.gameObject.SetActive(true);
    }
    private void Hide()
    {
        containerGameObject.gameObject.SetActive(false);
    }
}
