using System;
using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using Leopotam.EcsLite;

namespace CoreLogic.Systems
{
    public class SpawnSystem : SystemBase
    {
        private EcsFilter _filter;
        
        public override void Init(IEcsSystems systems)
        {
            _filter = World.Filter<SpawnComponent>().End();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ActorSpawn.Spawn(World.GetComponent<SpawnComponent>(entity).Settings, World.GetComponent<ActorRef>(entity).Value);
                World.RemoveComponent<SpawnComponent>(entity);
            }
        }
    }
}