using DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace DOTS.Entities
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class Player : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private Settings settings;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var data = new PlayerMovement()
            {
                speed = settings.playerSpeed
            };
            dstManager.AddComponent(entity, typeof(PlayerInput));
            dstManager.AddComponentData(entity, data);
        }
    }
}