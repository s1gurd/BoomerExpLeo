using System.Linq;
using System.Reflection;
using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.Utils;
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

    public class TestSystem : SystemBase
    {
        private EcsFilter _filter;
        
        public override void Init(IEcsSystems systems)
        {
            _filter = World.Filter<TestComponent>().Inc<TransformRef>().End ();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var tr = ref World.GetComponent<TransformRef>(entity);
                var pos = tr.Value.position;
                pos += Vector3.left/100;
                tr.Value.position = pos;
            }
        }
    }
}