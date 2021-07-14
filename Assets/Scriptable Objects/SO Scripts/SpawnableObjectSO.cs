using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableObjectSO", menuName = "Game/ScriptableObjects/Spawns/Spawnable Object")]
public class SpawnableObjectSO : ScriptableObject
{
    public GameObject prefab;

    public bool isProjectile = false;
    public ProjectileSettingsSO projectileSettings;
}
