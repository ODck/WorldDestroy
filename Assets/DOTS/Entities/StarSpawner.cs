using System.Collections.Generic;
using DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace DOTS.Entities
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class StarSpawner : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        [SerializeField] private Settings starSpawnSettings;

        // Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
        public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
        {
            gameObjects.Add(starSpawnSettings.starPrefab);
        }
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var starComponent = new Components.StarSpawner
            {
                starPrefab = conversionSystem.GetPrimaryEntity(starSpawnSettings.starPrefab),
                spawnRate = starSpawnSettings.spawnRate,
                initialStars = starSpawnSettings.initialStars
            };
            dstManager.AddComponentData(entity,starComponent);
        }
    }
}
