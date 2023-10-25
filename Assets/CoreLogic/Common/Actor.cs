using System;
using System.Collections.Generic;
using System.Linq;
using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using CoreLogic.Components.Transformation;
using CoreLogic.Graph;
using CoreLogic.Nodes;
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
                if (graph.nodes.Any(n => n is IInstanceNode))
                {
                    if (graph.Copy() is not ComponentNodeGraph instance) continue;
                    
                    instance.actor = this;
                    instance.gameObject = gameObject;
                    output.Add(instance);
                }
                else
                {
                    output.Add(graph);
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
            
            LinkUnityComponents(newEntity);
            
            var components = GetComponents<IAbility>(); 
            foreach (var component in components)
            {
                try
                {
                    component.ConvertToEntity(world, newEntity);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Entity Conversion - Components] {gameObject.name} on Component {component.ToString()} caused an exception:\n{e.Message}\n{e.StackTrace}");
                }
                
            }

            foreach (var graph in graphInstances)
            {
                foreach (var node in graph.nodes)
                {
                    if (node is IAbility ability)
                    {
                        try
                        {
                            ability.ConvertToEntity(world, newEntity);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"[Entity Conversion - Graph] {gameObject.name} in Graph {graph.name} in Node {node.name} caused an exception:\n{e.Message}\n{e.StackTrace}");
                        }
                    }
                }
            }

            return newEntity;
        }

        private void LinkUnityComponents(int entity)
        {
            var ecsWorld = EcsStartup.DefaultConversionWorld;

            ecsWorld.AddComponent(entity, new ActorRef
            {
                value = this
            });
            
            ecsWorld.AddComponent(entity, new TransformRef
            {
                Value = transform
            });

            if (transform is RectTransform rectTransform)
            {
                ecsWorld.AddComponent(entity, new RectTransformRef
                {
                    Value = rectTransform
                });
            }

            ecsWorld.AddComponent(entity, new GameObjectRef
            {
                Value = gameObject
            });
            
            if (TryGetComponent<Rigidbody>(out var rb))
            {
                ecsWorld.AddComponent(entity, new RigidbodyRef
                {
                    Value = rb
                });
                ecsWorld.AddComponent(entity,new FixedUpdateTransform());
            }
            
            if (TryGetComponent<Rigidbody2D>(out var rb2d))
            {
                ecsWorld.AddComponent(entity, new Rigidbody2DRef
                {
                    Value = rb2d
                });
            }

            if (TryGetComponent<CharacterController>(out var cc))
            {
                ecsWorld.AddComponent(entity, new CharacterRef
                {
                    Value = cc
                });
                ecsWorld.AddComponent(entity,new FixedUpdateTransform());
            }
        }
    }
}