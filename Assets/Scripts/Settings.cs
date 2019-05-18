using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    public float playerSpeed = 0.1f;
    [Header("Star Settings")] 
    public GameObject starPrefab;
    public float spawnRate;
    public int initialStars;
}
