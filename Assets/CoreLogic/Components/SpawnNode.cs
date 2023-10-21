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
    [Serializable][NodeWidth(400)][LabelWidth(180)][CreateNodeMenu("Spawn Objects")]
    public class SpawnNode : ExecComponentNode, IAbility
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
        public RotationOfSpawns rotationOfSpawns;
        public List<ComponentNodeGraph> applyAdditionalGraphs;
        [Input(connectionType = ConnectionType.Override)]
        public ListConnection<GameObject> copyComponentsFrom;
        [Output()] public ListConnection<GameObject> spawnedObjects;
        [Output()] public ListConnection<Actor> spawnedActors;

        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            
        }

        public override void Execute(Actor target = null)
        {
            base.Execute(target);
            
            var settings = new SpawnSettings
            {
                objectsToSpawn = objectsToSpawn.value,
                spawnPoints = spawnPoints.value,
                spawnPosition = SpawnPosition.UseSpawnPoints,
                x = x,
                fillSpawnPoints = fillSpawnPoints,
                spawnPointsFillingMode = FillOrder.SequentialOrder,
                skipBusySpawnPoints = skipBusySpawnPoints,
                rotationOfSpawns = rotationOfSpawns,
                applyAdditionalGraphs = applyAdditionalGraphs,
                copyComponentsFromSamples = copyComponentsFrom.value,
                parentOfSpawns = TargetType.None
            };
            spawnedActors = new ListConnection<Actor>();
            spawnedObjects.value = ActorSpawn.Spawn(settings, out spawnedActors.value, (graph as ComponentNodeGraph)?.actor, null);
            
            TriggerOutputs();
        }
    }
}