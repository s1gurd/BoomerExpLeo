using System;
using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using Leopotam.EcsLite;
using UnityEngine;

namespace CoreLogic.Components
{
    [Serializable]
    public struct TestComponent
    {
        public Rigidbody rb;
    }
    
    public class Converter : MonoBehaviour, IAbility
    {
        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            // custom components
            ecsWorld.GetPool<TestComponent>().Add(entity) = new TestComponent()
            {
                rb = this.GetComponent<Rigidbody>()
            };
        }
    }
}