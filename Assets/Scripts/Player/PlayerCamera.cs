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
        [SerializeField] private Transform playerObject;
        [SerializeField] private Transform playerOrientation;
        [SerializeField] private Transform playerCameraHolder;

        [SerializeField] private PlayerCameraMode cameraMode;
        [SerializeField] private GameObject thirdPersonCamera;
        [SerializeField] private GameObject thirdPersonActionCamera;

        private float lookRotationX;
        private float lookRotationY;

        private float mouseXInput;
        private float mouseYInput;
        private float horizontalInput;
        private float verticalInput;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // Start in first person
            cameraMode = PlayerCameraMode.FirstPerson;
            SetCameraMode(cameraMode);
        }

        private void Update()
        {
            UpdateInput();
            
            if (cameraModeInput.action.WasPressedThisFrame())
            {
                UpdateCameraMode();
            }

            switch (cameraMode)
            {
                case PlayerCameraMode.FirstPerson:
                    UpdateFirstPerson();
                    break;
                case PlayerCameraMode.ThirdPerson:
                case PlayerCameraMode.ThirdPersonAction:
                    UpdateThirdPerson();
                    break;
                default:
                    // Should never get here
                    throw new NotSupportedException($"{cameraMode}");
            }
        }

        private void UpdateInput()
        {
            // Mouse Input
            mouseXInput = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraSettings.mouseXSensitivity;
            mouseYInput = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraSettings.mouseYSensitivity;

            // Directional Input
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }

        private void UpdateCameraMode()
        {
            var playerCameraModeCount = Enum.GetValues(typeof(PlayerCameraMode)).Length;
            var newCameraModeValue = Mathf.Repeat((int)cameraMode + 1, playerCameraModeCount);
            cameraMode = (PlayerCameraMode)newCameraModeValue;

            SetCameraMode(cameraMode);
        }

        private void SetCameraMode(PlayerCameraMode mode)
        {
            thirdPersonCamera.SetActive(false);
            thirdPersonActionCamera.SetActive(false);
            
            switch (mode)
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
                    throw new NotSupportedException($"{mode}");
            }
        }

        private void UpdateThirdPerson()
        {
            var viewVector = playerObject.position -
                                new Vector3(transform.position.x, playerObject.position.y, transform.position.z);

            if (viewVector == Vector3.zero)
            {
                return;
            }
            
            playerOrientation.forward = viewVector.normalized;
            var inputDirection =
                playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;
            
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized,
                Time.deltaTime * cameraSettings.thirdPersonPlayerRotationSpeed);
        }

        private void UpdateFirstPerson()
        {
            lookRotationY += mouseXInput;
            lookRotationX -= mouseYInput;
            lookRotationX = Mathf.Clamp(lookRotationX, -cameraSettings.maxAngleUp, cameraSettings.maxAngleDown);
            
            // Rotate Player and Camera
            transform.rotation = Quaternion.Euler(lookRotationX, lookRotationY, 0);
            playerOrientation.rotation = Quaternion.Euler(0, lookRotationY, 0);
        
            // Update Camera position to stay with player
            transform.position = playerCameraHolder.position;
        }
    }
}
