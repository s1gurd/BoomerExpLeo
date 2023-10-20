using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreLogic.Common;
using CoreLogic.Graph;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace CoreLogic.Components
{
    [Serializable][NodeWidth(400)][LabelWidth(180)][CreateNodeMenu("Spawn Settings")]
    public class SpawnSettingsNode : ComponentNode, IAbility
    {
        [Input(connectionType = ConnectionType.Override)]
        public ListConnection<GameObject> objectsToSpawn;
        [Input(ShowBackingValue.Never,
            connectionType = ConnectionType.Override)]
        public ListConnection<GameObject> spawnPoints;
        public FillMode fillSpawnPoints;
        [ShowIf(nameof(fillSpawnPoints), FillMode.PlaceEachObjectXTimes)]
        public int x;
        public bool skipBusySpawnPoints;
        
        
        public SpawnSettingsSimple settings;
        
        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            
        }

    }
}