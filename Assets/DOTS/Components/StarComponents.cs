using System;
using Unity.Entities;

namespace DOTS.Components
{
    [Serializable]
    public struct StarSpawner : IComponentData
    {
        public Entity starPrefab;
        public float spawnRate;
        public int initialStars;
    }
}
