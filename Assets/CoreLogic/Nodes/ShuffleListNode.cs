using System;
using System.Linq;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using XNode;
using Random = UnityEngine.Random;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Shuffle List")][NodeWidth(140)]
    public class ShuffleListNode : ComponentNode
    {
        [Input(ShowBackingValue.Never)]
        public ListConnection<object> input;
        [Output]
        [LabelWidth(1)]
        public ListConnection<object> output;
        
        public override object GetValue(NodePort port)
        {
            input = GetInputValue<ListConnection<object>>(nameof(input));
            if (port.fieldName.Equals(nameof(output), StringComparison.Ordinal))
                return new ListConnection<object>(input.value.OrderBy(_ => Random.value));
            
            return base.GetValue(port);
        }
    }
}