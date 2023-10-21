using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace CoreLogic.Graph
{
    [Serializable]//[LabelWidth(1)]
    public class ListConnection<T> : IEnumerable<T>
    {
        [AssetsOnly]
        public List<T> value;

        public ListConnection(List<T> input)
        {
            value = input;
        }
        
        public ListConnection(T input)
        {
            value = new List<T>
            {
                input
            };
        }

        public ListConnection()
        {
            value = new List<T>();
        }

        public static implicit operator ListConnection<T>(T v)
        {
            return new ListConnection<T> { value = new List<T>{v} };
        }
        
        public static implicit operator ListConnection<T>(List<T> v)
        {
            return new ListConnection<T> { value = v };
        }

        public static explicit operator List<T>(ListConnection<T> v)
        {
            return new List<T>(v.value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}