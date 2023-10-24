using System;
using System.Linq;
using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.DataTypes;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using CoreLogic.Graph;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using XNode;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Ability Look")][NodeWidth(300)][LabelWidth(200)]
    public class AbilityLookNode : ComponentNode, IAbility
    {
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public InputContext inputContext;
        
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public string lookActionX;
        
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public string lookActionY;
        
        [SerializeField] private float xSensitivity = 2f;
        [SerializeField] private float ySensitivity = 2f;
        [SerializeField] private bool clampVerticalRotation = true;
        [SerializeField] [ShowIf(nameof(clampVerticalRotation))]private float minimumY = -90F;
        [SerializeField] [ShowIf(nameof(clampVerticalRotation))]private float maximumY = 90F;
        [SerializeField] private bool smooth = false;
        [SerializeField][ShowIf(nameof(smooth))] private float smoothTime = 5f;
        
        private InputAction _lookX;
        private InputAction _lookY;

        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            RefreshFields();

            if (!lookActionX.IsNullOrEmpty())
            {
                _lookX = inputContext.actions.FindActionMap(inputContext.actionMap).FindAction(lookActionX);
            }
            
            if (!lookActionY.IsNullOrEmpty())
            {
                _lookY = inputContext.actions.FindActionMap(inputContext.actionMap).FindAction(lookActionY);
            }

            ecsWorld.AddComponent(entity, new LookComponent
            {
                lookX = _lookX,
                lookY = _lookY,
                xSensitivity = xSensitivity,
                ySensitivity = ySensitivity,
                clampVerticalRotation = clampVerticalRotation,
                minimumY = minimumY,
                maximumY = maximumY,
                smooth = smooth,
                smoothTime = smoothTime,
                characterTargetRot = ecsWorld.GetComponent<TransformRef>(entity).Value.localRotation
            });

        }
    }
}