using System.Linq;
using CoreLogic.Common;
using CoreLogic.Common.DataTypes;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using CoreLogic.Graph;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Character Movement")][NodeWidth(300)][LabelWidth(200)]
    public class AbilityCharacterMovementNode : ComponentNode, IAbility
    {
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public InputContext inputContext;
        
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public string moveAction;
        
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public string jumpAction;
        
        [SerializeField] private float friction = 6;
        [SerializeField] private float gravity = 20;
        [SerializeField] private float jumpForce = 8;
        [SerializeField] private bool autoBunnyHop = true;
        [SerializeField] private float airControl = 0.3f;
        [Input(ShowBackingValue.Never)]public MovementSettings groundSettings;
        [Input(ShowBackingValue.Never)]public MovementSettings airSettings;
        [Input(ShowBackingValue.Never)]public MovementSettings airStrafeSettings;

        [SerializeField][LabelWidth(120)]private AngleCompensate angleCompensateMode;
        
        private InputAction _move;
        private InputAction _jump;

        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            RefreshFields();

            if (!moveAction.IsNullOrEmpty())
            {
                _move = inputContext.actions.FindActionMap(inputContext.actionMap).FindAction(moveAction);
            }

            if (!jumpAction.IsNullOrEmpty())
            {
                _jump = inputContext.actions.FindActionMap(inputContext.actionMap).FindAction(jumpAction);
            }

            ecsWorld.AddComponent(entity, new CharacterMovementComponent
            {
                moveInput = _move,
                jumpInput = _jump,
                friction = friction,
                gravity = gravity,
                airControl = airControl,
                jumpForce = jumpForce,
                autoBunnyHop = autoBunnyHop,
                groundSettings = groundSettings,
                airSettings = airSettings,
                strafeSettings = airStrafeSettings,
                angleCompensation = angleCompensateMode
            });
        }
    }
}