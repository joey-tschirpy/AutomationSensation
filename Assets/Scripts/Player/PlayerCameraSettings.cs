using UnityEngine;

namespace Player
{
    [CreateAssetMenu]
    public class PlayerCameraSettings : ScriptableObject
    {
        [Tooltip("How fast the player can run (units/s)")]
        [Min(0)]
        public float mouseXSensitivity = 1000;

        [Tooltip("How high the player can jump (units)")]
        [Min(0)]
        public float mouseYSensitivity = 1000;

        [Tooltip("Max angle the player can look up in degrees (0 forward, 90 up)")]
        [Range(0, 90)]
        public float maxAngleUp = 90;

        [Tooltip("Max angle the player can look down in degrees (0 forward, 90 down)")]
        [Range(0, 90)]
        public float maxAngleDown = 80;

        [Tooltip("Speed of player rotation in third person mode")]
        [Min(0)]
        public float thirdPersonPlayerRotationSpeed = 10f;
    }
}