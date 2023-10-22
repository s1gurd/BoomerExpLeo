using System;
using CoreLogic.Common;
using CoreLogic.Common.DataTypes;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Nodes
{
    [Serializable]
    [NodeWidth(140)]
    [LabelWidth(1)]
    [CreateNodeMenu("Init Input Context")]
    public class InitInputContextNode : ExecComponentNode
    {
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public InputContext inputContext;
        public override void Execute(Actor target = null)
        {
            RefreshFields();
            
            inputContext.actions.FindActionMap(inputContext.actionMap).Enable();
            
            TriggerOutputs();
        }

    }
}