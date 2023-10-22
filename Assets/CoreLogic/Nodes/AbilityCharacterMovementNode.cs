using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using CoreLogic.Graph;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Character Movement")][NodeWidth(140)]
    public class AbilityCharacterMovementNode : ComponentNode, IAbility
    {
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public InputContext inputContext;
        
        [Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
        public string moveAction;

        private InputAction _move;

        public void ConvertToEntity(EcsWorld ecsWorld, int entity)
        {
            RefreshFields();
            
            _move = inputContext.actions.FindActionMap(inputContext.actionMap).FindAction(moveAction);
            Debug.Log($"got {moveAction} - {_move.ToString()}");
            
            ecsWorld.AddComponent(entity, new CharacterMovementComponent
            {
                move = _move
            });
        }
    }
}