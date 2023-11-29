using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private enum PlayerCameraMode
        {
            FirstPerson,
            ThirdPerson,
            ThirdPersonAction,
        }
        
        [SerializeField] private PlayerCameraSettings cameraSettings;

        [SerializeField] private InputActionReference cameraModeInput;
        
        [SerializeField] private Transform player;
        [SerializeField] private Transform playerCameraHolder;

        [SerializeField] private PlayerCameraMode cameraMode;
        [SerializeField] private GameObject thirdPersonCamera;
        [SerializeField] private GameObject thirdPersonActionCamera;

        private float lookRotationX;
        private float lookRotationY;

        private float mouseXInput;
        private float mouseYInput;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            UpdateMouseInput();
            
            if (cameraModeInput.action.WasPressedThisFrame())
            {
                UpdateCameraMode();
            }

            if (cameraMode == PlayerCameraMode.FirstPerson)
            {
                UpdateFirstPerson();
            }
        }

        private void UpdateMouseInput()
        {
            mouseXInput = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraSettings.mouseXSensitivity;
            mouseYInput = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraSettings.mouseYSensitivity;
        }

        private void UpdateCameraMode()
        {
            thirdPersonCamera.SetActive(false);
            thirdPersonActionCamera.SetActive(false);

            var playerCameraModeCount = Enum.GetValues(typeof(PlayerCameraMode)).Length;
            var newCameraModeValue = Mathf.Repeat((int)cameraMode + 1, playerCameraModeCount);
            cameraMode = (PlayerCameraMode)newCameraModeValue;

            switch (cameraMode)
            {
                case PlayerCameraMode.FirstPerson:
                    // Do nothing. All other cameras disabled.
                    break;
                case PlayerCameraMode.ThirdPerson:
                    thirdPersonCamera.SetActive(true);
                    break;
                case PlayerCameraMode.ThirdPersonAction:
                    thirdPersonActionCamera.SetActive(true);
                    break;
                default:
                    // Should never get here
                    throw new NotSupportedException($"{cameraMode}");
            }
        }

        private void UpdateFirstPerson()
        {
            lookRotationY += mouseXInput;
            lookRotationX -= mouseYInput;
            lookRotationX = Mathf.Clamp(lookRotationX, -cameraSettings.maxAngleUp, cameraSettings.maxAngleDown);

            // Rotate Player and Camera
            transform.rotation = Quaternion.Euler(lookRotationX, lookRotationY, 0);
            player.rotation = Quaternion.Euler(0, lookRotationY, 0);
        
            // Update Camera position to stay with player
            transform.position = playerCameraHolder.position;
        }
    }
}
