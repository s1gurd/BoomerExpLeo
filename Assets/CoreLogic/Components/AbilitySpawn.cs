using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.Utils;
using Leopotam.EcsLite;
using UnityEngine;

namespace CoreLogic.Components
{
    public class AbilitySpawn : MonoBehaviour, IAbility
    {
	    public SpawnSettings settings;
        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            ecsWorld.AddComponent(entity, new SpawnComponent()
            {
                Settings = settings
            });
        }
    }
}