using System;
using System.Collections;
using System.Linq;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Gameobjects by Tag")][NodeWidth(200)]
    public class FindByTagNode : ComponentNode
    {
        [ValueDropdown(nameof(Tags))][LabelWidth(1)]
        public string tag;
        [Output] [LabelWidth(1)] public ListConnection<GameObject> output;
        
        public override object GetValue(NodePort port)
        {
            if (port.fieldName.Equals(nameof(output), StringComparison.Ordinal))
            {
                if (tag.Equals(String.Empty))
                    return null;
                return new ListConnection<GameObject>(GameObject.FindGameObjectsWithTag(tag).ToList());

            }

            return base.GetValue(port);
        }
        
        public static IEnumerable Tags()
        {
#if UNITY_EDITOR
            return UnityEditorInternal.InternalEditorUtility.tags;
#else
            return null;
#endif
        }
    }
}