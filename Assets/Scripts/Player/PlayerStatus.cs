using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField] private Transform feetTransform;
        [SerializeField] private PlayerActionSettings actionSettings;
        
        public bool IsGrounded { private set; get; }
        
        private Rigidbody rb;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            CheckIsGrounded();
            UpdateDrag();
        }

        private void UpdateDrag()
        {
            rb.drag = IsGrounded ? actionSettings.groundDrag : actionSettings.airDrag;
        }

        private void CheckIsGrounded()
        {
            var distanceToGround = Vector3.Distance(transform.position, feetTransform.position);
            IsGrounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround + 0.1f);
        }
    }
}
