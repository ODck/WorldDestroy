using DOTS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DOTS.Systems
{
    public class PlayerMovementSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct PlayerMovementSystemJob : IJobForEach<Translation, PlayerInput, PlayerMovement>
        {

            public void Execute(ref Translation translation, ref PlayerInput playerInput, [ReadOnly] ref PlayerMovement playerMovement)
            {
                translation.Value += new float3(playerInput.movement.x, playerInput.movement.y,0) * playerMovement.speed;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new PlayerMovementSystemJob
            {
                //Movement = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))
            };
            return job.Schedule(this, inputDependencies);
        }
    }
}