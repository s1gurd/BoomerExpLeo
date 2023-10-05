using System.Collections.Generic;
using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace CoreLogic
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private List<EcsInjectionContext> _injectionContexts;

        public static EcsWorld DefaultConversionWorld { get; set; }
        
        private EcsWorld _world;
        private EcsManager _ecsManager;

        private void Awake()
        {
            _world = new EcsWorld();
            EcsStartup.DefaultConversionWorld = _world;
        
            _ecsManager = new EcsManager();
            _ecsManager.SetWorld(_world);
        
            foreach (var injectionContext in _injectionContexts)
            {
                injectionContext.InitInjector();
                _ecsManager.AddInjector(injectionContext.GetInjector());   
            }
        }

        

        private void Start()
        {
            _ecsManager.InstallModule(new MainEcsModuleInstaller());
        }

        private void Update()
        {
            _ecsManager.Update();
        }

        private void LateUpdate()
        {
            _ecsManager.LateUpdate();
        }

        private void FixedUpdate()
        {
            _ecsManager.FixedUpdate();
        }

        private void OnDestroy()
        {
            _ecsManager.Destroy();
        }
    }
}