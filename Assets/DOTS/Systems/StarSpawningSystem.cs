using DOTS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DOTS.Systems
{
    //Creating and removing Entities can only be done on the main thread to prevent race conditions.
    //The system uses an EntityCommandBuffer to defer tasks that can't be done inside the Job.
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class StarSpawningSystem : JobComponentSystem
    {
        // BeginInitializationEntityCommandBufferSystem is used to create a command buffer which will then be played back
        // when that barrier system executes.
        // Though the instantiation command is recorded in the SpawnJob, it's not actually processed (or "played back")
        // until the corresponding EntityCommandBufferSystem is updated. To ensure that the transform system has a chance
        // to run on the newly-spawned entities before they're rendered for the first time, the SpawnerSystem_FromEntity
        // will use the BeginSimulationEntityCommandBufferSystem to play back its commands. This introduces a one-frame lag
        // between recording the commands and instantiating the entities, but in practice this is usually not noticeable.
        private BeginInitializationEntityCommandBufferSystem _entityCommandBufferSystem;
        protected override void OnCreate()
        {
            // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
            _entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        private struct StarSpawningSystemJob : IJobForEachWithEntity<StarSpawner, LocalToWorld>
        {
            public EntityCommandBuffer.Concurrent commandBuffer;
            //public Entity starPrefab;
            //public float spawnRate;
            //public float nextStarTimer;
        
        
        public void Execute(Entity entity, int index, [ReadOnly] ref StarSpawner starSpawner,
            [ReadOnly] ref LocalToWorld location)
        {
            for (var i = 0; i < starSpawner.initialStars; i++)
            {
                var instance = commandBuffer.Instantiate(index, starSpawner.starPrefab);
                //Place the instantiated entity
                var position = math.transform(location.Value, new Random(1234).NextFloat3());
                commandBuffer.SetComponent(index, instance, new Translation {Value = position});
            }

            commandBuffer.DestroyEntity(index, entity);
        }
        }
    
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            //Instead of performing structural changes directly, a Job can add a command to an EntityCommandBuffer to perform such changes on the main thread after the Job has finished.
            //Command buffers allow you to perform any, potentially costly, calculations on a worker thread, while queuing up the actual insertions and deletions for later.

            // Schedule the job that will add Instantiate commands to the EntityCommandBuffer.
            var job = new StarSpawningSystemJob
            {
                commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
            }.Schedule(this, inputDependencies);


            // SpawnJob runs in parallel with no sync point until the barrier system executes.
            // When the barrier system executes we want to complete the SpawnJob and then play back the commands (Creating the entities and placing them).
            // We need to tell the barrier system which job it needs to complete before it can play back the commands.
            _entityCommandBufferSystem.AddJobHandleForProducer(job);

            return job;
        }
    }
}