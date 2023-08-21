using UnityEngine;

namespace Player
{
    [CreateAssetMenu]
    public class PlayerActionSettings : ScriptableObject
    {
        [Tooltip("How fast the player can run (units/s)")]
        public float movementSpeed = 10;
    }
}
