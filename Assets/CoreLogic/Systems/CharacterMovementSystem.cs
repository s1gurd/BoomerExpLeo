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
        private Camera CurrentCamera 
        {
            get
            {
                if (_camera is null) _camera = Camera.main;
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

                

                if (character.isGrounded)
                {
                    QueueJump(ref movement, entity);
                    GroundMove(ref movement, transform, character, input);
                }
                else
                {
                    if (movement.autoBunnyHop)
                    {
                        QueueJump(ref movement, entity);
                    }
                    AirMove(ref movement, transform, character, input);
                    ApplyGravity(ref movement);
                }
                
                character.Move(movement.velocity * Time.fixedDeltaTime);
            }
        }

        private void ApplyGravity(ref CharacterMovementComponent movement)
        {
            movement.velocity.y -= movement.gravity * Time.fixedDeltaTime;
        }

        private void QueueJump(ref CharacterMovementComponent move, int entity)
        {
            if (move.jumpInput.WasPressedThisFrame())
            {
                move.jumpQueued = true;
            }

            if (move.jumpInput.WasReleasedThisFrame())
            {
                move.jumpQueued = false;
            }
        }

        private void AirMove(ref CharacterMovementComponent move,
            Transform transform,
            CharacterController character,
            Vector2? input)
        {
            if (input is null) return;
            
            var wishDir = AdjustAngle(move, transform, new Vector3(input.Value.x, 0, input.Value.y));
            var wishSpeed = wishDir.magnitude * move.airSettings.maxSpeed;

            wishDir.Normalize();

            // CPM Air control.
            var wishSpeedTemp = wishSpeed;

            var accel = Vector3.Dot(move.velocity, wishDir) < 0 
                ? move.airSettings.deceleration 
                : move.airSettings.acceleration;

            // If the player is ONLY strafing left or right
            if (input.Value.y == 0 && input.Value.x != 0)
            {
                if (wishSpeed > move.strafeSettings.maxSpeed)
                {
                    wishSpeed = move.strafeSettings.maxSpeed;
                }

                accel = move.strafeSettings.acceleration;
            }

            Accelerate(wishDir, wishSpeed, accel, ref move.velocity);
            if (move.airControl > 0)
            {
                AirControl(ref move,wishDir, wishSpeedTemp, input);
            }

            if (!move.autoBunnyHop)
            {
                move.jumpQueued = false;
            }
        }

        private void AirControl(ref CharacterMovementComponent move,
            Vector3 targetDir,
            float targetSpeed,
            Vector2? input)
        {
            // Only control air movement when moving forward or backward.
            if (input is not null && (Mathf.Abs(input.Value.y) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)) return;
            
            var zSpeed = move.velocity.y;
            move.velocity.y = 0;
            /* Next two lines are equivalent to idTech's VectorNormalize() */
            var speed = move.velocity.magnitude;
            
            move.velocity.Normalize();

            var dot = Vector3.Dot(move.velocity, targetDir);
            var k = 32f;
            k *= move.airControl * dot * dot * Time.fixedDeltaTime;

            // Change direction while slowing down.
            if (dot > 0)
            {
                move.velocity.x *= speed + targetDir.x * k;
                move.velocity.y *= speed + targetDir.y * k;
                move.velocity.z *= speed + targetDir.z * k;

                move.velocity.Normalize();
            }

            move.velocity.x *= speed;
            move.velocity.y = zSpeed; // Note this line
            move.velocity.z *= speed;
        }
        
        private void GroundMove(ref CharacterMovementComponent move, 
            Transform transform, 
            CharacterController character,
            Vector2? input)
        {
            // Do not apply friction if the player is queueing up the next jump
            if (!move.jumpQueued)
            {
                ApplyFriction(ref move, character, 1.0f);
            }
            else
            {
                ApplyFriction(ref move, character, 0);
            }

            if (input is null) return;

            var wishDir = new Vector3(input.Value.x, 0, input.Value.y);
            wishDir = AdjustAngle(move, transform, wishDir);
            wishDir.Normalize();

            var wishSpeed = wishDir.magnitude;
            wishSpeed *= move.groundSettings.maxSpeed;

            Accelerate(wishDir, wishSpeed, move.groundSettings.acceleration, ref move.velocity);

            if (!move.jumpQueued) return;

            move.velocity.y = move.jumpForce;
            move.jumpQueued = false;
        }

        private Vector3 AdjustAngle(CharacterMovementComponent move, Transform transform, Vector3 wishDir)
        {
            switch (move.angleCompensation)
            {
                case AngleCompensate.CompensateCameraAngle:
                    //TODO: Doesn't work with Up direction. Fix it sometimes
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
                    wishDir = -1 * forward * wishDir.z + right * wishDir.x;
                    break;
                case AngleCompensate.DoNotCompensate:
                default:
                    break;
            }

            return wishDir;
        }

        private void ApplyFriction(ref CharacterMovementComponent move, 
            CharacterController character,
            float t)
        {
            // Equivalent to VectorCopy();
            var v = move.velocity; 
            v.y = 0;
            var speed = v.magnitude;
            var control = speed < move.groundSettings.deceleration ? move.groundSettings.deceleration : speed;
            var drop = control * move.friction * Time.fixedDeltaTime * t;

            var newSpeed = speed - drop;
            
            if (newSpeed < 0)
            {
                newSpeed = 0;
            }

            if (speed > 0)
            {
                newSpeed /= speed;
            }

            move.velocity.x *= newSpeed;
            // playerVelocity.y *= newSpeed;
            move.velocity.z *= newSpeed;
        }
        
        private void Accelerate(Vector3 targetDir, float targetSpeed, float accel, ref Vector3 velocity)
        {
            var currentSpeed = Vector3.Dot(velocity, targetDir);
            var addSpeed = targetSpeed - currentSpeed;
            
            if (addSpeed <= 0) return;
            
            var accelSpeed = accel * Time.fixedDeltaTime * targetSpeed;
            if (accelSpeed > addSpeed)
            {
                accelSpeed = addSpeed;
            }

            velocity.x += accelSpeed * targetDir.x;
            velocity.z += accelSpeed * targetDir.z;
        }
    }
}