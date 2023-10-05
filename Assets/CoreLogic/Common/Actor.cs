using System;
using AleVerDes.LeoEcsLiteZoo;
using UnityEngine;
using UnityEngine.UI;

namespace CoreLogic.Common
{
    public class Actor : MonoBehaviour, IActor
    {
        public int? Entity { get; private set; }
        public Actor Spawner { get; set; }
        public Actor Owner { get; set; }

        private bool _converted;

        private void Start()
        {
            Entity = ConvertActor();
            //LinkUnityComponents(Entity);
        }

        private int? ConvertActor()
        {
            if (_converted)
            {
                return Entity;
            }

            var components = GetComponents<IAbility>();

            var entity = EcsStartup.DefaultConversionWorld.NewEntity();

            foreach (var component in components)
            {
                component.ConvertToEntity(EcsStartup.DefaultConversionWorld, entity);
            }
            
            LinkUnityComponents(entity);

            _converted = true;

            return entity;
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