using System;
using CoreLogic.Common;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using XNode;

namespace CoreLogic.Nodes
{
    [Serializable]
    [NodeWidth(120)]
    [LabelWidth(1)]
    [CreateNodeMenu("Init Input Context")]
    public class InitInputContextNode : ExecComponentNode
    {
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public InputContext inputContext;
        public override void Execute(Actor target = null)
        {
            base.Execute(target);

            inputContext.actions.FindActionMap(inputContext.actionMap).Enable();
            
            TriggerOutputs();
        }

    }
}