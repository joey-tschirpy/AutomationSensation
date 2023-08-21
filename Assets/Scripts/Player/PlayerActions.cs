using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField] private InputActionReference movementInput;
        [SerializeField] private PlayerActionSettings actionSettings;

        private Rigidbody rb;
        private Vector3 currentMoveDirection;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void Update()
        {
            ProcessInput();
            SpeedControl();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void ProcessInput()
        {
            var movementInputValue = movementInput.action.ReadValue<Vector2>();
            if (movementInputValue.sqrMagnitude <= 0)
            {
                // No input to process
                currentMoveDirection = Vector3.zero;
                return;
            }
            
            var rbTransform = rb.transform;
            currentMoveDirection = (rbTransform.forward * movementInputValue.y +
                                   rbTransform.right * movementInputValue.x).normalized;
        }

        private void MovePlayer()
        {
            // Calculate move force in look direction
            var movementForce = currentMoveDirection * actionSettings.movementSpeed;
            
            rb.AddForce(movementForce, ForceMode.VelocityChange);
        }

        private void SpeedControl()
        {
            var currentVelocity = rb.velocity;
            var horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            if (horizontalVelocity.magnitude > actionSettings.movementSpeed)
            {
                // Limit movement to max velocity
                var maxVelocity = horizontalVelocity.normalized * actionSettings.movementSpeed;
                rb.velocity = new Vector3(maxVelocity.x, currentVelocity.y, maxVelocity.z);
            }
        }
    }
}
