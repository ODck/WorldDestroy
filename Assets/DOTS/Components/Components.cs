using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS.Components
{
    [Serializable]
    public struct PlayerInput : IComponentData
    {
        public float2 movement;
    }

    [Serializable]
    public struct PlayerMovement : IComponentData
    {
        public float speed;
    }
}