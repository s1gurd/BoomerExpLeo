using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Systems;
using Leopotam.EcsLite;

namespace CoreLogic
{
    public class MainEcsModuleInstaller : IEcsModuleInstaller
    {
        public IEcsModule Install()
        {
            var module = new EcsModule();
        
            module
                .AddFeature(new DebugFeature())
                ;

            return module;
        }
    }

    public class DebugFeature : IEcsFeature
    {
        public void SetupUpdateSystems(IEcsSystems systems)
        {
            systems
                //.Add(new MyFirstSystem())
                .Add(new TestSystem())
                .Add(new SpawnSystem())
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
                .Add (new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem ())
                ;
        }

        public void SetupLateUpdateSystems(IEcsSystems systems)
        {
        }

        public void SetupFixedUpdateSystems(IEcsSystems systems)
        {
        }

        public void SetupInjector(IEcsInjector injector)
        {
        
        }
    }
}