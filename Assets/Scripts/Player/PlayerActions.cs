using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerActions : MonoBehaviour
    {
        [SerializeField] private InputActionReference movementInput;
        [SerializeField] private InputActionReference jumpInput;
        [SerializeField] private PlayerActionSettings actionSettings;
        [SerializeField] private Transform playerOrientation;
        
        private Rigidbody rb;
        private PlayerStatus status;
        
        private Vector3 currentMoveDirection;

        private bool canJump = true;
        private float jumpTimer = 0f;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            status = GetComponent<PlayerStatus>();
        }

        private void Update()
        {
            ProcessVerticalMovementInput();
            SpeedControl();
        }

        private void FixedUpdate()
        {
            ProcessHorizontalMovementInput();
            MovePlayer();
        }

        #region ProcessingInput

        private void ProcessHorizontalMovementInput()
        {
            var movementInputValue = movementInput.action.ReadValue<Vector2>();
            if (movementInputValue.sqrMagnitude <= 0)
            {
                // No input to process
                currentMoveDirection = Vector3.zero;
                return;
            }
            
            currentMoveDirection = (playerOrientation.forward * movementInputValue.y +
                                    playerOrientation.right * movementInputValue.x).normalized;
        }

        private void ProcessVerticalMovementInput()
        {
            if (canJump && status.IsGrounded && jumpInput.action.IsPressed())
            {
                    Jump();
                    Invoke(nameof(ResetJump), actionSettings.jumpCooldown);
            }
        }
        
        #endregion

        #region HorizontalMovement

        private void MovePlayer()
        {
            // Calculate move force in look direction
            var movementForce = actionSettings.movementSpeed * 10f * currentMoveDirection;

            if (!status.IsGrounded)
            {
                movementForce *= actionSettings.airMovementMultiplier;
            }
            
            rb.AddForce(movementForce, ForceMode.Force);
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
        
        #endregion

        #region VerticalMovment

        private void Jump()
        {
            // Reset vertical velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * actionSettings.jumpForce, ForceMode.Impulse);
            
            canJump = false;
            jumpTimer = actionSettings.jumpCooldown;
        }

        private void ResetJump()
        {
            canJump = true;
        }
        
        #endregion
    }
}
