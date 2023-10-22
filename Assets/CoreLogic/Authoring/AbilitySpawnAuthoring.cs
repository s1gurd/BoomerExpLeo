using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace CoreLogic.Authoring
{
    public class AbilitySpawnAuthoring : MonoBehaviour, IAbility
    {
	    public SpawnSettings settings;
        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            ecsWorld.AddComponent(entity, new SpawnComponent()
            {
                Settings = settings
            });
        }

        public void Execute(Actor other)
        {
            throw new System.NotImplementedException();
        }
    }
}