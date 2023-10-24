using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreLogic.Components
{
    [Serializable]
    public struct LookComponent
    {
        public InputAction lookX;
        public InputAction lookY;
        public float xSensitivity;
        public float ySensitivity;
        public bool clampVerticalRotation;
        public float minimumY;
        public float maximumY;
        public bool smooth;
        public float smoothTime;

        public Quaternion characterTargetRot;
    }
}