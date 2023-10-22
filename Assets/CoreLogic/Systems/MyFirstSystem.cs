using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace CoreLogic.Systems {
    public class MyFirstSystem : IEcsRunSystem, IEcsInitSystem
    {    
        
        private EcsFilter _filter;
        private EcsPool<TransformRef> _transforms;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld ();
            _filter = world.Filter<TestComponent> ().Inc<TransformRef>().End ();
            
            _transforms = world.GetPool<TransformRef>();
        }

        public void Run (IEcsSystems systems) {
            foreach (var entity in _filter)
            {
                ref var tr = ref _transforms.Get(entity);
                var pos = tr.Value.position;
                pos += Vector3.left/100;
                tr.Value.position = pos;
            }
        }
    }
}