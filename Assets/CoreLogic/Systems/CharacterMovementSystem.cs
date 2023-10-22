using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace CoreLogic.Systems
{
    public class CharacterMovementSystem : SystemBase
    {
        private EcsFilter _filter;

        public override void Init(IEcsSystems systems)
        {
            _filter = World.Filter<CharacterMovementComponent>().End();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var movement = World.GetComponent<CharacterMovementComponent>(entity).move;
                var delta = movement?.ReadValue<Vector2>();
                Debug.Log(delta);
                
            }
        }
    }
}