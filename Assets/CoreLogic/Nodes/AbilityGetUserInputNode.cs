using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreLogic.Common;
using CoreLogic.Common.DataTypes;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using XNode;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("User Input")]//[NodeWidth(140)]
    public class AbilityGetUserInputNode : ComponentNode
    {
        [SerializeField] private InputActionAsset actionsAsset;
        [ValueDropdown(nameof(ActionMaps))]public string actionMap;

        [Output(ShowBackingValue.Never)] 
        public InputContext inputContext;
        
        [Output(ShowBackingValue.Always)][LabelWidth(100)]
        public List<string> inputActions;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName.Equals(nameof(inputContext), StringComparison.Ordinal))
            {
                return new InputContext
                {
                    actions = actionsAsset,
                    actionMap = actionMap
                };
            }
            return base.GetValue(port);
        }

        public override void OnValidate()
        {
            base.OnValidate();
            inputActions.Clear();
            foreach (var action in actionsAsset.FindActionMap(actionMap))
            {
                inputActions.Add(action.name);
            }
        }

        public IEnumerable ActionMaps()
        {
            return actionsAsset != null ? actionsAsset.actionMaps.Select(a => a.name) : null;
        }
    }
}