using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using Leopotam.EcsLite;

namespace CoreLogic.Systems.Transformation
{
    public class RotationFixedSystem : SystemBase
    {
        private EcsFilter _filterRotate;
        private EcsFilter _filterSetRotation;

        public override void Init(IEcsSystems systems)
        {
            _filterRotate = World.Filter<RotateComponent>().Inc<FixedUpdateTransform>().End();
            _filterSetRotation = World.Filter<SetRotationComponent>().Inc<FixedUpdateTransform>().End();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterRotate)
            {
                var rotate = World.GetComponent<RotateComponent>(entity);
                if (World.HasComponent<CharacterRef>(entity))
                {
                    World.GetComponent<CharacterRef>(entity).Value.transform.localRotation *= rotate.delta;
                }

                if (World.HasComponent<RigidbodyRef>(entity))
                {
                    var rb = World.GetComponent<RigidbodyRef>(entity).Value;
                    rb.Move(rb.position, rb.rotation * rotate.delta);
                }
                
                World.RemoveComponent<RotateComponent>(entity);
            }
            
            foreach (var entity in _filterSetRotation)
            {
                var rotation = World.GetComponent<SetRotationComponent>(entity);
                if (World.HasComponent<CharacterRef>(entity))
                {
                    World.GetComponent<CharacterRef>(entity).Value.transform.localRotation = rotation.rotation;
                }

                if (World.HasComponent<RigidbodyRef>(entity))
                {
                    var rb = World.GetComponent<RigidbodyRef>(entity).Value;
                    rb.Move(rb.position, rotation.rotation);
                }
                
                World.RemoveComponent<SetRotationComponent>(entity);
            }
        }
    }
}