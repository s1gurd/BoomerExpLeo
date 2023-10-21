using System.Linq;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Shuffle List")][NodeWidth(140)]
    public class ShuffleListNode : ComponentNode
    {
        [Input(ShowBackingValue.Never)] public ListConnection<object> input;
        [Output] [LabelWidth(1)] public ListConnection<object> output;
        
        public override object GetValue(NodePort port)
        {
            input = GetInputValue<ListConnection<object>>(nameof(input));
            if (port.fieldName == nameof(output))
                return new ListConnection<object>(input.value.OrderBy(_ => Random.value));
            
            return base.GetValue(port);
        }
    }
}