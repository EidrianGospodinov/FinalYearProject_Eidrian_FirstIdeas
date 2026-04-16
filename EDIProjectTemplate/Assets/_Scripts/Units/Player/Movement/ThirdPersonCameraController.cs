using System;
using _Scripts.Units.Player.Core;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class ThirdPersonCameraController : MonoBehaviour
{

    [Inject] private GameManager gameManager;
    
    [SerializeField] private float zoomSpeed = 2f;

    [SerializeField] private float zoomLerpSpeed = 10f;

    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 15f;

    private PlayerInput playerInput;

    private CinemachineCamera cineCam;
    private CinemachineInputAxisController cineInputController;
    private Camera cam;

    private CinemachineOrbitalFollow orbital;

    private Vector2 scrollDelta;

    private float targetZoom;

    private float currentzoom;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.CameraControls.MouseZoom.performed += HandleMouseScroll;

        cineCam = GetComponent<CinemachineCamera>();
        cineInputController = GetComponent<CinemachineInputAxisController>();
        orbital = cineCam.GetComponent<CinemachineOrbitalFollow>();
        
        targetZoom = currentzoom = orbital.Radius;
        cam = Camera.main;
    }

    private void HandleMouseScroll(InputAction.CallbackContext obj)
    {
        scrollDelta = obj.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        bool shouldBeActive = gameManager.GetCurrentGameState == GameState.InGame;
        if (cineInputController.isActiveAndEnabled != shouldBeActive)
        {
            cineInputController.enabled = shouldBeActive;
        }

        if (!shouldBeActive)
        {
            return;
        }
        if (scrollDelta.y != 0)
        {
            if (orbital != null)
            {
                targetZoom = Mathf.Clamp(orbital.Radius - scrollDelta.y * zoomSpeed, minDistance, maxDistance);
                scrollDelta = Vector2.zero;
            }
        }

        currentzoom = Mathf.Lerp(currentzoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
        orbital.Radius = currentzoom;
    }
    
}
