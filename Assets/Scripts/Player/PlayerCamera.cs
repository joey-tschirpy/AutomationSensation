using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerCameraSettings cameraSettings;

        [SerializeField] private Transform player;
        [SerializeField] private Transform playerCameraHolder;

        private float cameraRotationX;
        private float cameraRotationY;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            // Mouse input
            var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraSettings.mouseXSensitivity;
            var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraSettings.mouseYSensitivity;

            cameraRotationY += mouseX;
            cameraRotationX -= mouseY;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -cameraSettings.maxAngleUp, cameraSettings.maxAngleDown);

            // Rotate Player and Camera
            transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);
            player.rotation = Quaternion.Euler(0, cameraRotationY, 0);
        
            // Update Camera position to stay with player
            transform.position = playerCameraHolder.position;
        }
    }
}
