namespace _Scripts.Units.Player
{
    using UnityEngine;

    public class PlayerCameraLook : MonoBehaviour
    {
        [Header("Camera")] 
        [SerializeField] private Camera cam;
        public float sensitivity = 15f;

        private float xRotation = 0f;
        private Vector2 currentLookInput;
    
        void Awake()
        {
            if (cam == null) cam = Camera.main;

            // Initial setup for first-person control
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Called by PlayerController in Update()
        public void SetLookInput(Vector2 input)
        {
            currentLookInput = input;
        }

        // Called by PlayerController in LateUpdate()
        public void HandleCameraRotation()
        {
            float mouseX = currentLookInput.x;
            float mouseY = currentLookInput.y;
        
            // Vertical Camera Rotation (Look up/down)
            xRotation -= (mouseY * Time.deltaTime * sensitivity);
            xRotation = Mathf.Clamp(xRotation, -80, 80);

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            // Horizontal Body Rotation (Turn left/right)
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime * sensitivity));
        }
    }
}