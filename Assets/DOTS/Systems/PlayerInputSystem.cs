using DOTS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS.Systems
{
    public class PlayerInputSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct PlayerMovementSystemJob : IJobForEach<PlayerInput>
        {
            public float2 movement;
        
        
            public void Execute(ref PlayerInput player)
            {
                player.movement = movement;
            }
        }
    
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var job = new PlayerMovementSystemJob
            {
                movement = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))
            };
            return job.Schedule(this, inputDependencies);
        }
    }
}