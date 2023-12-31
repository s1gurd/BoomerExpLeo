using System;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Select Prefab")][LabelWidth(1)][NodeWidth(120)]
    public class GameObjectNode : ComponentNode
    {
        [Output(ShowBackingValue.Always)]
        [AssetsOnly]
        [PreviewField(80, ObjectFieldAlignment.Center)]
        public GameObject prefab;
        
        public override object GetValue(NodePort port)
        {
            if (port.fieldName.Equals(nameof(prefab), StringComparison.Ordinal))
                return new ListConnection<GameObject>(prefab);
            
            return base.GetValue(port);
        }
        
        public override void OnValidate()
        {
            name = prefab != null ? prefab.name : "Select Prefab";
        }
    }
}