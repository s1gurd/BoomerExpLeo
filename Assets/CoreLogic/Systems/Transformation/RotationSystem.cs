using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using CoreLogic.Components.Transformation;
using Leopotam.EcsLite;

namespace CoreLogic.Systems.Transformation
{
    public class RotationSystem : SystemBase
    {
        private EcsFilter _filterRotate;
        private EcsFilter _filterSetRotation;

        public override void Init(IEcsSystems systems)
        {
            _filterRotate = World.Filter<RotateComponent>()
                .Inc<TransformRef>()
                .Exc<FixedUpdateTransform>()
                .End();
            _filterSetRotation = World.Filter<SetRotationComponent>()
                .Inc<TransformRef>()
                .Exc<FixedUpdateTransform>()
                .End();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterRotate)
            {
                World.GetComponent<TransformRef>(entity)
                    .Value.localRotation *=
                        World.GetComponent<RotateComponent>(entity).delta;
                World.RemoveComponent<RotateComponent>(entity);
            }
            
            foreach (var entity in _filterSetRotation)
            {
                World.GetComponent<TransformRef>(entity)
                        .Value.localRotation =
                    World.GetComponent<SetRotationComponent>(entity).rotation;
                World.RemoveComponent<SetRotationComponent>(entity);
            }
        }
    }
}
    