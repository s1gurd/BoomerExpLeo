using System.Collections.Generic;
using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Components;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoreLogic.Common
{
    public class Actor : MonoBehaviour
    {
        [HideIf("@graphInstances.Count != 0")]public List<ComponentNodeGraph> graphs;

        [HideIf("@graphInstances.Count == 0")]public List<ComponentNodeGraph> graphInstances;
        
        public int? Entity;
        [HideInInspector]public Actor spawner;
        [HideInInspector]public Actor owner;
        
        private void Start()
        {
            graphInstances = CreateRuntimeGraphs(graphs);
            Entity = ConvertActor();
            PerformStartups(graphInstances);
        }

        private void PerformStartups(List<ComponentNodeGraph> componentNodeGraphs)
        {
            foreach (var instance in componentNodeGraphs)
            {
                foreach (var node in instance.nodes)
                {
                    if (node is StartupNode startup) startup.Startup();
                }
            }
        }

        private List<ComponentNodeGraph> CreateRuntimeGraphs(List<ComponentNodeGraph> sourceGraphs)
        {
            if (sourceGraphs is null) return null;
            var output = new List<ComponentNodeGraph>();
            foreach (var graph in sourceGraphs)
            {
                if (graph.Copy() is ComponentNodeGraph instance)
                {
                    instance.actor = this;
                    instance.gameObject = gameObject;
                    output.Add(instance);
                }
            }

            return output;
        }

        private int? ConvertActor()
        {
            if (Entity != null)
            {
                return Entity;
            }

            var world = EcsStartup.DefaultConversionWorld;

            var newEntity = world.NewEntity();
            
            var components = GetComponents<IAbility>(); 
            foreach (var component in components)
            {
                component.ConvertToEntity(world, newEntity);
            }

            foreach (var graph in graphInstances)
            {
                foreach (var node in graph.nodes)
                {
                    if (node is IAbility ability) ability.ConvertToEntity(world, newEntity);
                }
            }
            
            LinkUnityComponents(newEntity);

            return newEntity;
        }

        private void LinkUnityComponents(int entity)
        {
            var ecsWorld = EcsStartup.DefaultConversionWorld;

            ecsWorld.GetPool<ActorRef>().Add(entity) = new ActorRef()
            {
                Value = this
            };
            
            ecsWorld.GetPool<TransformRef>().Add(entity) = new TransformRef()
            {
                Value = transform
            };

            if (transform is RectTransform rectTransform)
            {
                ecsWorld.GetPool<RectTransformRef>().Add(entity) = new RectTransformRef()
                {
                    Value = rectTransform
                };
            }

            ecsWorld.GetPool<GameObjectRef>().Add(entity) = new GameObjectRef()
            {
                Value = gameObject
            };
            
            if (TryGetComponent<Rigidbody>(out var rb))
            {
                ecsWorld.GetPool<RigidbodyRef>().Add(entity) = new RigidbodyRef()
                {
                    Value = rb
                };
            }
            
            if (TryGetComponent<Rigidbody2D>(out var rb2d))
            {
                ecsWorld.GetPool<Rigidbody2DRef>().Add(entity) = new Rigidbody2DRef()
                {
                    Value = rb2d
                };
            }
        }
    }

    public struct ActorRef
    {
        public Actor Value;
    }
}