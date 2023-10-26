using CoreLogic.Components;
using CoreLogic.Components.Transformation;
using Leopotam.EcsLite;
using UnityEngine;

namespace CoreLogic.Common.Utils
{
    public static class ComponentsExtensions
    {
        public static bool HasComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            return world.GetPool<T>().Has(entity);
        }
        
        public static ref T GetComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            return ref world.GetPool<T>().Get(entity);
        }

        public static ref T AddComponent<T>(this EcsWorld world, int entity, T value = default) where T : struct
        {
            ref var newComp = ref world.GetPool<T>().Add(entity);
            newComp = value;
            return ref newComp;
        }
        public static ref T SetComponent<T>(this EcsWorld world, int entity, T value = default) where T : struct
        {
            if (world.HasComponent<T>(entity))
            {
                ref var comp = ref world.GetComponent<T>(entity);
                comp = value;
                return ref comp;
            }
            
            return ref world.AddComponent(entity, value);
        }
        
        public static void RemoveComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            world.GetPool<T>().Del(entity);
        }

        public static void RemoveComponentSafe<T>(this EcsWorld world, int entity) where T : struct
        {
            if (!world.HasComponent<T>(entity)) return;
            
            world.RemoveComponent<T>(entity);
        }
        
        public static ref TranslateComponent AddTranslation(this EcsWorld world, int entity, Vector3 delta)
        {
            if (world.HasComponent<TranslateComponent>(entity))
            {
                ref var comp = ref world.GetComponent<TranslateComponent>(entity);
                comp.delta += delta;
                return ref comp;
            }
            else
            {
                return ref world.AddComponent(entity,new TranslateComponent
                {
                    delta = delta
                });
            }
        }
        
        public static ref RotateComponent AddRotation(this EcsWorld world, int entity, Quaternion delta)
        {
            if (world.HasComponent<RotateComponent>(entity))
            {
                ref var comp = ref world.GetComponent<RotateComponent>(entity);
                comp.delta *= delta;
                return ref comp;
            }
            else
            {
                return ref world.AddComponent(entity,new RotateComponent
                {
                    delta = delta
                });
            }
        }

    }
}