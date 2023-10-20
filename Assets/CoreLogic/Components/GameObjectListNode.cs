using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Components
{
    [CreateNodeMenu("Select Multiple Prefabs")]
    public class GameObjectListNode : ComponentNode
    {
        [Output(ShowBackingValue.Always)] [AssetsOnly] [LabelWidth(1)]public ListConnection<GameObject> prefabs;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(prefabs))
                return prefabs;
            return null;
        }
        
        private void OnValidate()
        {
            var c = prefabs.value.Count;
            name = $"{c} prefab{(c > 1 ? 's' : string.Empty)}";
        }
    }
}