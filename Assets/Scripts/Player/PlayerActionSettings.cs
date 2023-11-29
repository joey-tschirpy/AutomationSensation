using UnityEngine;

namespace Player
{
    [CreateAssetMenu]
    public class PlayerActionSettings : ScriptableObject
    {
        // Horizontal Movement
        
        [Tooltip("How fast the player can run (units/s)")]
        public float movementSpeed = 10;

        [Tooltip("Multiplier on the force applied when moving while airborne (not grounded)")]
        public float airMovementMultiplier = 0.5f;

        [Tooltip("Drag applied to player when grounded")]
        public float groundDrag = 2f;

        [Tooltip("Drag applied to player when airborne (not grounded)")]
        public float airDrag = 0f;
        
        // Vertical Movement

        [Tooltip("Upward force applied when player jumps")]
        public float jumpForce = 10f;
        
        [Tooltip("Time (in seconds) before the player can jump after jumping")]
        public float jumpCooldown = 0.2f;
    }
}
