using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XNode;

namespace CoreLogic.Graph
{
    [NodeWidth(300)]
    public abstract class ComponentNode : Node
    {
        public void RefreshFields()
        {
            var fields = this.GetType().GetFields()
                .Where(f => f.GetCustomAttributes(typeof(InputAttribute), true).Length > 0);

            foreach (var field in fields)
            {
                if (GetPort(field.Name).IsConnected)
                {
                    var temp = Convert.ChangeType(GetInputValue<object>(field.Name), field.FieldType);
                    field.SetValue(this, temp);
                }
            }
        }
        
        public List<T> GetInputListValue<T>(string fieldName, T fallback = default(T))
        {
            return GetInputValue<ListConnection<T>>(fieldName)?.value;
        }

        public override object GetValue(NodePort port)
        {
            
            var field = this.GetType()
                .GetFields().FirstOrDefault(f => f.GetCustomAttributes(typeof(OutputAttribute), true).Length > 0
                                                 && port.fieldName.StartsWith(f.Name, StringComparison.Ordinal));

            if (port.fieldName.Equals(field?.Name))
            {
                return field?.GetValue(this);
            } else if (typeof(IEnumerable).IsAssignableFrom(field?.FieldType))
            {
                var index = int.Parse(port.fieldName.Split(' ').Last());
                var tempEnumerable = (field?.GetValue(this) as IEnumerable<object>)?.ToArray();
                return tempEnumerable?.ElementAt(index);
            }

            return null;
        }
        
        public virtual void OnValidate()
        {
            var attr = this.GetType().GetCustomAttributes<CreateNodeMenuAttribute>().ToList();
            if (attr.Any()) name = attr.First().menuName;
        }
    }
}