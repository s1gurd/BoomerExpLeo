using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using CoreLogic.Components.Transformation;
using Leopotam.EcsLite;

namespace CoreLogic.Systems.Transformation
{
    public class PositionFixedSystem : SystemBase
    {
        private EcsFilter _filterTranslate;
        private EcsFilter _filterSetPosition;

        public override void Init(IEcsSystems systems)
        {
            _filterTranslate = World.Filter<TranslateComponent>().Inc<FixedUpdateTransform>().End();
            _filterSetPosition = World.Filter<SetPositionComponent>().Inc<FixedUpdateTransform>().End();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterTranslate)
            {
                var translate = World.GetComponent<TranslateComponent>(entity);
                if (World.HasComponent<CharacterRef>(entity))
                {
                    World.GetComponent<CharacterRef>(entity).Value.Move(translate.delta);
                }

                if (World.HasComponent<RigidbodyRef>(entity))
                {
                    var rb = World.GetComponent<RigidbodyRef>(entity).Value;
                    rb.Move(rb.position + translate.delta, rb.rotation);
                }
                
                World.RemoveComponent<TranslateComponent>(entity);
            }
            
            foreach (var entity in _filterSetPosition)
            {
                var pos = World.GetComponent<SetPositionComponent>(entity);
                if (World.HasComponent<CharacterRef>(entity))
                {
                    var character = World.GetComponent<CharacterRef>(entity).Value;
                    character.Move(character.transform.position + pos.position);
                }

                if (World.HasComponent<RigidbodyRef>(entity))
                {
                    var rb = World.GetComponent<RigidbodyRef>(entity).Value;
                    rb.Move(pos.position, rb.rotation);
                }
                
                World.RemoveComponent<SetPositionComponent>(entity);
            }
        }
    }
}