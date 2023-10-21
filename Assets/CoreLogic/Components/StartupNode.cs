
using System;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Components
{
    [CreateNodeMenu("Startup")][NodeWidth(115)]
    public class StartupNode : ComponentNode
    {
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited)]
        [LabelWidth(1)]
        public ExecComponentNode outputTrigger;
        
        public void Startup()
        {
            this.TriggerOutputs(nameof(outputTrigger));
        }
        
        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(outputTrigger))
                return outputTrigger;
            
            return null;
        }
    }
}