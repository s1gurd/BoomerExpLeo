using Leopotam.EcsLite;

namespace CoreLogic.Common
{
    public abstract class SystemBase : IEcsRunSystem, IEcsInitSystem, IEcsPreInitSystem
    {
        protected EcsWorld World;

        public abstract void Init(IEcsSystems systems);
        public abstract void Run(IEcsSystems systems);

        public void PreInit(IEcsSystems systems)
        {
            World = systems.GetWorld();
        }


    }
}