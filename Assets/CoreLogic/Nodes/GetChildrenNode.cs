using System.Collections.Generic;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Get Children")][NodeWidth(140)]
    public class GetChildrenNode : ComponentNode
    {
        [Input(ShowBackingValue.Never)] public ListConnection<GameObject> input;
        [Output] [LabelWidth(1)] public ListConnection<GameObject> output;
        
        public override object GetValue(NodePort port)
        {
            input = GetInputValue<ListConnection<GameObject>>(nameof(input));
            if (port.fieldName == nameof(output))
            {
                var children = new List<GameObject>();
                foreach (var obj in input)
                {
                    foreach (Transform child in obj.transform)
                    {
                        if (child == obj.transform) continue;
                        
                        children.Add(child.gameObject);
                    }
                }
                return new ListConnection<object>(children);

            }
                
            return base.GetValue(port);
        }
    }
}