using System;
using AleVerDes.LeoEcsLiteZoo;
using CoreLogic.Common;
using CoreLogic.Common.DataTypes;
using CoreLogic.Common.Utils;
using CoreLogic.Components;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreLogic.Systems
{
    public class CharacterMovementSystem : SystemBase
    {
        public Camera CurrentCamera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = Camera.main;
                }

                return _camera;
            }
        }
        
        private EcsFilter _filter;

        private Camera _camera;

        public override void Init(IEcsSystems systems)
        {
            _filter = World.Filter<CharacterMovementComponent>().Inc<CharacterRef>().End();
        }

        public override void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var movement = ref World.GetComponent<CharacterMovementComponent>(entity);
                var character = World.GetComponent<CharacterRef>(entity).Value;
                var transform = World.GetComponent<TransformRef>(entity).Value;
                var input = movement.moveInput?.ReadValue<Vector2>();
                Vector3 velocity = Vector3.zero;

                QueueJump(ref movement, entity);
                
                if (character.isGrounded)
                {
                    GroundMove(ref movement, transform, character, ref velocity, input);
                }
                else
                {
                    //AirMove();
                    GroundMove(ref movement, transform, character, ref velocity, input);
                }
                character.Move(velocity * Time.fixedDeltaTime*5f);
            }
        }

        private void QueueJump(ref CharacterMovementComponent m, int entity)
        {
            var jumping = World.HasComponent<JumpPressedComponent>(entity);
            
            if (m.autoBunnyHop)
            {
                m.jumpQueued = jumping;
                return;
            }

            if (jumping && !m.jumpQueued)
            {
                m.jumpQueued = true;
            }

            if (jumping)
            {
                m.jumpQueued = false;
            }
        }
        
        private void GroundMove(ref CharacterMovementComponent m, 
            Transform transform, 
            CharacterController character, 
            ref Vector3 velocity, 
            Vector2? input)
        {
            // Do not apply friction if the player is queueing up the next jump
            if (!m.jumpQueued)
            {
                ApplyFriction(ref m, character, ref velocity, 1.0f);
            }
            else
            {
                ApplyFriction(ref m, character, ref velocity, 0);
            }

            if (input != null)
            {
                var wishDir = new Vector3(input.Value.x, 0, input.Value.y);
                switch (m.angleCompensation)
                {
                    case AngleCompensate.CompensateCameraAngle:
                        
                        wishDir = CurrentCamera.transform.TransformDirection(wishDir);
                        break;
                    case AngleCompensate.RelativeToObjectForward:
                        wishDir = transform.TransformDirection(wishDir);
                        break;
                    case AngleCompensate.RelativeToCameraView:
                        var cameraTransform = CurrentCamera.transform;
                        var forward = cameraTransform.position - transform.position;
                        var right = cameraTransform.right;
                        forward.y = 0f;
                        right.y = 0f;
                        forward.Normalize();
                        right.Normalize();
                        wishDir = forward * wishDir.z + right * wishDir.x;
                        break;
                    case AngleCompensate.DoNotCompensate:
                    default:
                        break;
                }
                wishDir = transform.TransformDirection(wishDir);
                wishDir.Normalize();

                var wishSpeed = wishDir.magnitude;
                wishSpeed *= m.groundSettings.maxSpeed;

                Accelerate(wishDir, wishSpeed, m.groundSettings.acceleration, ref velocity);
            }

            // Reset the gravity velocity
            velocity.y = -m.gravity * Time.deltaTime;

            if (m.jumpQueued)
            {
                velocity.y = m.jumpForce;
                m.jumpQueued = false;
            }
        }
        
        private void ApplyFriction(ref CharacterMovementComponent m, 
            CharacterController character, 
            ref Vector3 velocity, 
            float t)
        {
            // Equivalent to VectorCopy();
            var vec = velocity; 
            vec.y = 0;
            var speed = vec.magnitude;
            var drop = 0f;

            // Only apply friction when grounded.
            if (character.isGrounded)
            {
                float control = speed < m.groundSettings.deceleration ? m.groundSettings.deceleration : speed;
                drop = control * m.friction * Time.deltaTime * t;
            }

            float newSpeed = speed - drop;
            
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }

            if (speed > 0)
            {
                newSpeed /= speed;
            }

            velocity.x *= newSpeed;
            // playerVelocity.y *= newSpeed;
            velocity.z *= newSpeed;
        }
        
        private void Accelerate(Vector3 targetDir, float targetSpeed, float accel, ref Vector3 velocity)
        {
            float currentSpeed = Vector3.Dot(velocity, targetDir);
            float addSpeed = targetSpeed - currentSpeed;
            if (addSpeed <= 0)
            {
                return;
            }

            float accelSpeed = accel * Time.deltaTime * targetSpeed;
            if (accelSpeed > addSpeed)
            {
                accelSpeed = addSpeed;
            }

            velocity.x += accelSpeed * targetDir.x;
            velocity.z += accelSpeed * targetDir.z;
        }
    }
}