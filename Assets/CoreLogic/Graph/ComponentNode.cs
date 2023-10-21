using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreLogic.Common;
using UnityEngine;
using XNode;

namespace CoreLogic.Graph
{
    [NodeWidth(300)]
    public abstract class ComponentNode : Node
    {
        public List<T> GetInputListValue<T>(string fieldName, T fallback = default(T))
        {
            return GetInputValue<ListConnection<T>>(fieldName)?.value;
        }

        public virtual void Execute(Actor target = null)
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

        public override object GetValue(NodePort port)
        {
            var field = this.GetType()
                .GetFields().FirstOrDefault(f => f.GetCustomAttributes(typeof(OutputAttribute), true).Length > 0 
                                                 && f.Name.EndsWith(port.fieldName));
            return field?.GetValue(this);
        }
    }
}