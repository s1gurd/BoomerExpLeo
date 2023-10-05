using System;
using System.Collections.Generic;
using System.Linq;
using CoreLogic.Common.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;


namespace CoreLogic.Common
{
    public static class ActorSpawn
    {
        public static List<GameObject> Spawn(SpawnSettings spawnSettings, Actor spawner = null, Actor owner = null)
        {
            Quaternion tempRot = Quaternion.identity;
            Vector3 tempPos = default;

            int spawnCount;

            List<Component> sampledComponents = new List<Component>();
            List<GameObject> spawnedObjects = new List<GameObject>();

            spawnSettings.random = new Random(spawnSettings.randomSeed);

            switch (spawnSettings.fillSpawnPoints)
            {
                case FillMode.UseEachObjectOnce:
                    spawnCount = spawnSettings.objectsToSpawn.Count;
                    break;
                case FillMode.FillAllSpawnPoints:
                    spawnCount = spawnSettings.spawnPoints.Count;
                    break;
                case FillMode.PlaceEachObjectXTimes:
                    spawnCount = spawnSettings.objectsToSpawn.Count * spawnSettings.x;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (spawnSettings.skipBusySpawnPoints && spawnSettings.spawnPosition == SpawnPosition.UseSpawnPoints)
            {
                for (var i = 0; i < spawnSettings.spawnPoints.Count; i++)
                {
                    var actorSpawnedOnPoint = spawnSettings.spawnPoints[i].GetComponent<ActorSpawnedOnPoint>();
                    
                    if (actorSpawnedOnPoint != null && actorSpawnedOnPoint.actor != null)
                    {
                        spawnSettings.spawnPoints.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (spawnSettings.spawnPointsFillingMode == FillOrder.RandomOrder)
            {
                spawnSettings.objectsToSpawn =
                    spawnSettings.objectsToSpawn.OrderBy(item => spawnSettings.random.Next()).ToList();
                spawnSettings.spawnPoints =
                    spawnSettings.spawnPoints.OrderBy(item => spawnSettings.random.Next()).ToList();
            }

            for (var i = 0; i < spawnCount; i++)
            {
                switch (spawnSettings.spawnPosition)
                {
                    case SpawnPosition.UseSpawnPoints:
                    {
                        if (spawnSettings.spawnPoints.Count == 0)
                        {
                            Debug.LogError(
                                "[ACTOR SPAWNER] In Use Spawn Points mode you have to provide some spawning points!");
                            return null;
                        }

                        tempPos = spawnSettings.spawnPoints[i % spawnSettings.spawnPoints.Count].transform.position;
                        if (spawnSettings.rotationOfSpawns == RotationOfSpawns.UseSpawnPointRotation)
                        {
                            tempRot = spawnSettings.spawnPoints[i % spawnSettings.spawnPoints.Count].transform.rotation;
                        }

                        break;
                    }

                    case SpawnPosition.RandomPositionOnNavMesh:
                        //tempPos = NavMeshRandomPointUtil.GetRandomLocation();
                        break;
                    case SpawnPosition.UseSpawnerPosition:
                        if (spawner == null)
                        {
                            Debug.LogError("[ACTOR SPAWNER] You are using Use Spawner Position, but Spawner is NULL!");
                            return null;
                        }

                        if (spawner.gameObject == null) return null;

                        tempPos = spawner.gameObject.transform.position;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (spawnSettings.rotationOfSpawns)
                {
                    case RotationOfSpawns.UseRandomRotationY:
                        tempRot = Quaternion.Euler(0f, spawnSettings.random.Next() % 360f, 0f);
                        break;
                    case RotationOfSpawns.UseRandomRotationXYZ:
                        tempRot = Quaternion.Euler(spawnSettings.random.Next() % 360f, spawnSettings.random.Next() % 360f,
                            spawnSettings.random.Next() % 360f);
                        break;
                    case RotationOfSpawns.UseSpawnPointRotation:
                        if (spawnSettings.spawnPosition == SpawnPosition.UseSpawnerPosition)
                        {
                            tempRot = spawner.gameObject.transform.rotation;
                        }

                        break;
                    case RotationOfSpawns.UseZeroRotation:
                        tempRot = Quaternion.Euler(Vector3.zero);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (spawnSettings.copyComponentsFromSamples.Count > 0)
                {
                    foreach (var sample in spawnSettings.copyComponentsFromSamples)
                    {
                        if (sample == null) continue;

                        foreach (var component in sample.GetComponents<Component>())
                        {
                            if (!component || component is Transform or IActor) continue;
                            switch (spawnSettings.copyComponentsOfType)
                            {
                                case ComponentsOfType.OnlyAbilities when !(component is IAbility):
                                    continue;
                                case ComponentsOfType.OnlySimpleBehaviours when component is IAbility:
                                    continue;
                                case ComponentsOfType.AllComponents:
                                default:
                                    sampledComponents.Add(component);
                                    break;
                            }
                        }
                    }

                    if (sampledComponents.Count == 0)
                        Debug.LogError("[LEVEL ACTOR SPAWNER] No suitable components found in sample game objects!");
                }

                GameObject tempObj = UnityEngine.Object.Instantiate(
                    spawnSettings.objectsToSpawn[i % spawnSettings.objectsToSpawn.Count], tempPos, tempRot);

                var actors = tempObj.GetComponents<Actor>();

                if (spawnSettings.parentOfSpawns != TargetType.None)
                {
                    var parents = FindActorsUtils.GetActorsList(tempObj, spawnSettings.parentOfSpawns,
                        spawnSettings.actorWithComponentName, spawnSettings.parentTag);

                    var parent = FindActorsUtils.ChooseActor(tempObj.transform, parents, spawnSettings.chooseParentStrategy);

                    tempObj.transform.SetParent(parent);
                }

                if (sampledComponents.Count > 0)
                {
                    if (spawnSettings.deleteExistingComponents)
                    {
                        foreach (var component in tempObj.GetComponents<Component>())
                        {
                            if (component is Transform || component is IActor) continue;
                            Object.Destroy(component);
                        }
                    }

                    tempObj.CopyComponentsWithLinks(sampledComponents);
                }

                if (actors.Length > 1)
                {
                    Debug.LogError("[ACTOR SPAWNER] Only one IActor Component for Actor allowed!");
                }
                else if (actors.Length == 1)
                {
                    actors.First().Spawner = spawner;
                    actors.First().Owner = owner ? owner : spawner ? spawner : actors.First();
                }

                if (spawnSettings.spawnPosition == SpawnPosition.UseSpawnPoints && spawnSettings.skipBusySpawnPoints)
                {
                    spawnSettings.spawnPoints[i % spawnSettings.spawnPoints.Count].AddComponent<ActorSpawnedOnPoint>()
                        .actor = tempObj;
                }

                spawnedObjects.Add(tempObj);
            }

            return spawnedObjects;
        }

    }
}