using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace CoreLogic.Common
{
    [Serializable]
    public struct SpawnSettingsSimple
    {
        public List<GameObject> objectsToSpawn;

        public SpawnPosition spawnPosition;

        [ShowIf(nameof(spawnPosition), SpawnPosition.UseSpawnPoints)]
        [EnumToggleButtons]
        public FillOrder spawnPointsFillingMode;

        public FillMode fillSpawnPoints;

        [ShowIf(nameof(fillSpawnPoints), FillMode.PlaceEachObjectXTimes)]
        public int x;

        [ShowIf(nameof(spawnPosition), SpawnPosition.UseSpawnPoints)]
        public bool skipBusySpawnPoints;

        [ShowIf(nameof(spawnPosition), SpawnPosition.UseSpawnPoints)]
        [EnumToggleButtons]
        public SpawnPointsSource spawnPointsFrom;

        [ShowIf("@spawnPosition == SpawnPosition.UseSpawnPoints && spawnPointsFrom == SpawnPointsSource.Manually")]
        [SceneObjectsOnly]
        public List<GameObject> spawnPoints;

        [ShowIf(nameof(spawnPosition), SpawnPosition.UseSpawnPoints)]
        [ShowIf(nameof(spawnPointsFrom), SpawnPointsSource.FindByTag)]
        [ValueDropdown(nameof(Tags))]
        public string spawnPointTag;

        [ShowIf(nameof(spawnPosition), SpawnPosition.UseSpawnPoints)]
        public bool useChildrenObjects;

        public RotationOfSpawns rotationOfSpawns;

        [EnumToggleButtons] public TargetType parentOfSpawns;

        [ShowIf(nameof(parentOfSpawns), TargetType.ComponentName)]
        public string actorWithComponentName;

        [ShowIf(nameof(parentOfSpawns), TargetType.ChooseByTag)]
        [ValueDropdown(nameof(Tags))]
        public string parentTag;

        [HideIf("@parentOfSpawns == TargetType.Spawner || parentOfSpawns == TargetType.None")]
        [EnumToggleButtons]
        public ChooseTargetStrategy chooseParentStrategy;

        public List<GameObject> copyComponentsFromSamples;

        [ShowIf("@copyComponentsFromSamples.Count > 0")]
        [InfoBox(
            "Transforms and IActors will not be copied in any case!\n Only components of chosen type will be replaced!")]
        public ComponentsOfType copyComponentsOfType;

        [ShowIf("@copyComponentsFromSamples.Count > 0")]
        public bool deleteExistingComponents;
        
        public bool destroyAbilityAfterSpawn;
        
        public int randomSeed;
        public Random random;
        
        public bool spawnerDisabled;

        public static IEnumerable Tags()
        {
#if UNITY_EDITOR
            return UnityEditorInternal.InternalEditorUtility.tags;
#else
            return null;
#endif
        }


    }
}