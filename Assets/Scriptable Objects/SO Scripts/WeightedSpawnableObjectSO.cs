using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeightedSpawnableObjectSO", menuName = "Game/ScriptableObjects/Spawns/Weighted Spawnable Object")]
public class WeightedSpawnableObjectSO : ScriptableObject
{
    public int weight = 0;
    public GameObject prefab;

    public bool isProjectile = false;
    public ProjectileSettingsSO projectileSettings;
}
