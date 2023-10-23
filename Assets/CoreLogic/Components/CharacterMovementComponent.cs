using System;
using CoreLogic.Common.DataTypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreLogic.Components
{
    [Serializable]
    public struct CharacterMovementComponent
    {
        public InputAction moveInput;
        public InputAction jumpInput;
        public float friction;
        public float gravity;
        public float jumpForce;
        public bool autoBunnyHop;
        public bool jumpQueued;
        public float airControl;
        public MovementSettings groundSettings;
        public MovementSettings airSettings;
        public MovementSettings strafeSettings;
        public AngleCompensate angleCompensation;

        public Vector3 velocity;
    }
}
