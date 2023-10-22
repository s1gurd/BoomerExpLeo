using System;
using System.Collections.Generic;
using CoreLogic.Common;
using CoreLogic.Common.DataTypes;
using CoreLogic.Graph;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoreLogic.Nodes
{
    [Serializable][NodeWidth(400)][LabelWidth(180)][CreateNodeMenu("Spawn Objects")]
    public class AbilitySpawnNode : ExecComponentNode
    {
        [Input(connectionType = ConnectionType.Override)]
        public ListConnection<GameObject> objectsToSpawn;
        [Input(ShowBackingValue.Never,
            connectionType = ConnectionType.Override)]
        public ListConnection<GameObject> spawnPoints;
        [SerializeField] private FillMode fillSpawnPoints;
        [ShowIf(nameof(fillSpawnPoints), FillMode.PlaceEachObjectXTimes)]
        [SerializeField] private int x;
        [SerializeField] private bool skipBusySpawnPoints;
        [SerializeField] private RotationOfSpawns rotationOfSpawns;
        [SerializeField] private List<ComponentNodeGraph> applyAdditionalGraphs;
        [Input(connectionType = ConnectionType.Override)]
        public ListConnection<GameObject> copyComponentsFrom;
        [Output()] public ListConnection<GameObject> spawnedObjects;
        [Output()] public ListConnection<Actor> spawnedActors;

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