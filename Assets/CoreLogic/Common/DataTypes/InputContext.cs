using System;
using UnityEngine.InputSystem;

namespace CoreLogic.Common.DataTypes
{
    [Serializable]
    public class InputContext
    {
        public InputActionAsset actions;
        public string actionMap;
    }
}