using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Systems;
using CoreLogic.Systems.Transformation;
using Leopotam.EcsLite;

namespace CoreLogic
{
    public class DebugFeature : IEcsFeature
    {
        public void SetupUpdateSystems(IEcsSystems systems)
        {
            systems
                .Add(new TestSystem())
                .Add(new SpawnSystem())
                .Add(new RotationSystem())
                .Add(new PositionSystem())
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
                .Add (new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem ())
                ;
        }

        public void SetupLateUpdateSystems(IEcsSystems systems)
        {
        }

        public void SetupFixedUpdateSystems(IEcsSystems systems)
        {
            systems
                .Add(new CharacterMovementSystem())
                .Add(new LookSystem())
                .Add(new RotationFixedSystem())
                .Add(new PositionFixedSystem())
            ;
        }

        public void SetupInjector(IEcsInjector injector)
        {
        
        }
    }
}