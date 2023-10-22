using System;
using CoreLogic.Common;
using CoreLogic.Common.DataTypes;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XNode;

namespace CoreLogic.Nodes
{
    [Serializable]
    [NodeWidth(180)]
    //[LabelWidth(300)]
    [CreateNodeMenu("Movement Settings")]
    public class MovementSettingsNode : ComponentNode
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [Output(typeConstraint = TypeConstraint.Strict)]
        [LabelWidth(1)]
        public MovementSettings settingsOutput;
        
        public override object GetValue(NodePort port)
        {
            if (port.fieldName.Equals(nameof(settingsOutput), StringComparison.Ordinal))
            {
                return new MovementSettings()
                {
                    maxSpeed = maxSpeed,
                    acceleration = acceleration,
                    deceleration = deceleration
                };
            }
            return base.GetValue(port);
        }

        public override void OnValidate()
        {
            if (!GetPort(nameof(settingsOutput)).IsConnected)
            {
                base.OnValidate();
                return;
            }
            else
            {
                this.name = ObjectNames.NicifyVariableName(GetPort(nameof(settingsOutput)).Connection.fieldName);
            }
        }
    }
}