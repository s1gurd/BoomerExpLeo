using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using Leopotam.EcsLite;

namespace CoreLogic.Systems.Transformation
{
    public class PositionSystem : SystemBase
    {
        private EcsFilter _filterTranslate;
        private EcsFilter _filterSetPosition;

        public override void Init(IEcsSystems systems)
        {
            _filterTranslate = World.Filter<TranslateComponent>()
                .Inc<TransformRef>()
                .Exc<FixedUpdateTransform>()
                .End();
            _filterSetPosition = World.Filter<SetPositionComponent>()
                .Inc<TransformRef>()
                .Exc<FixedUpdateTransform>()
                .End();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterTranslate)
            {
                World.GetComponent<TransformRef>(entity)
                    .Value.position +=
                        World.GetComponent<TranslateComponent>(entity).delta;
                World.RemoveComponent<TranslateComponent>(entity);
            }
            foreach (var entity in _filterSetPosition)
            {
                World.GetComponent<TransformRef>(entity)
                        .Value.position =
                    World.GetComponent<SetPositionComponent>(entity).position;
                World.RemoveComponent<SetPositionComponent>(entity);
            }
        }
    }
}
    