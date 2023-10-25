using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using CoreLogic.Components.Transformation;
using Leopotam.EcsLite;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace CoreLogic.Systems
{
    public class LookSystem : SystemBase
    {
        private EcsFilter _filter;

        public override void Init(IEcsSystems systems)
        {
            _filter = World.Filter<LookComponent>().End();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var input = new Vector2();
                ref var look = ref World.GetComponent<LookComponent>(entity);
                
                if (look.lookX?.actionMap is not null)
                {
                    switch (look.lookX.expectedControlType)
                    {
                        case "Axis":
                            input.x = look.lookX.ReadValue<float>();
                            break;
                        case "Vector2":
                            input.x = look.lookX.ReadValue<Vector2>().x;
                            break;
                        default:
                            Debug.LogError($"[Look system] Unsupported input type!");
                            break;
                    }
                }
                
                if (look.lookY?.actionMap is not null)
                {
                    switch (look.lookY.expectedControlType)
                    {
                        case "Axis":
                            input.y = look.lookY.ReadValue<float>();
                            break;
                        case "Vector2":
                            input.y = look.lookY.ReadValue<Vector2>().y;
                            break;
                        default:
                            Debug.LogError($"[Look system] Unsupported input type!");
                            break;
                    }
                }

                Look(ref look, input);

                World.SetComponent(entity, new SetRotationComponent
                {
                    rotation = look.characterTargetRot
                });
            }
        }

        private void Look(ref LookComponent look, Vector2 input)
        {
            var inputSens = input;
            inputSens.x *= look.xSensitivity;
            inputSens.y *= look.ySensitivity;
            
            var tempRot = look.characterTargetRot * Quaternion.Euler(inputSens.y, inputSens.x, 0f);

            if (look.clampVerticalRotation)
            {
                tempRot = ClampRotationAroundXAxis(ref look, tempRot);
            }

            if (look.smooth)
            {
                look.characterTargetRot = Quaternion.Slerp(look.characterTargetRot, tempRot,
                    look.smoothTime * Time.fixedDeltaTime);
            }
            else
            {
                look.characterTargetRot = tempRot;
            }
        }
        
        private Quaternion ClampRotationAroundXAxis(ref LookComponent look, Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            var angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleY = Mathf.Clamp(angleY, look.minimumY, look.maximumY);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

            return q;
        }
    }
}