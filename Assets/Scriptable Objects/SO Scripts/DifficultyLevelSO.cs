using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevelSettingsSO", menuName = "Game/ScriptableObjects/Difficulty Level Settings")]
public class DifficultyLevelSO : ScriptableObject
{
    public float TimeBetweenSpawns = 1f;
    public List<WeightedSpawnableObjectSO> possibleSpawns = new List<WeightedSpawnableObjectSO>();
}
