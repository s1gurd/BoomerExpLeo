using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Select Multiple Prefabs")]
    public class GameObjectListNode : ComponentNode
    {
        [Output(ShowBackingValue.Always)] [AssetsOnly] [LabelWidth(1)]public ListConnection<GameObject> prefabs;

        private void OnValidate()
        {
            var c = prefabs.value.Count;
            name = $"{c} prefab{(c > 1 ? 's' : string.Empty)}";
        }
    }
}