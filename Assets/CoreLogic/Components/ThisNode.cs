using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Components
{
    [CreateNodeMenu("This")][NodeWidth(110)]
    public class ThisNode : ComponentNode
    {
        [Output][LabelWidth(1)]public GameObject gameObject;
        
        private ComponentNodeGraph _compGraph;
        public override object GetValue(NodePort port) {
            _compGraph ??= graph as ComponentNodeGraph;
            
            if (_compGraph is not null) 
                gameObject = _compGraph.actor != null ? _compGraph.actor.gameObject : null;
            
            if (port.fieldName == nameof(gameObject))
                return new ListConnection<GameObject>(gameObject);
            
            return null;
        }
    }
}