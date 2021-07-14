using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyLevelSettingsSO", menuName = "Game/ScriptableObjects/Difficulty Level Settings")]
public class DifficultyLevelSO : ScriptableObject
{
    public float TimeBetweenSpawnSequences = 1f;
    public List<WeightedSpawnSequenceSO> possibleSpawns = new List<WeightedSpawnSequenceSO>();
}
