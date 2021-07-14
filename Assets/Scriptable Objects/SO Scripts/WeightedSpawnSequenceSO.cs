using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeightedSpawnSequenceSO", menuName = "Game/ScriptableObjects/Spawns/Weighted Spawn Sequence")]
public class WeightedSpawnSequenceSO : ScriptableObject
{
    public int weight = 1;

    public List<SpawnableObjectSO> orderedSpawns = new List<SpawnableObjectSO>();

    [Min(0.1f)] public float timeBetweenSpawns = 1f;

    [Min(1)] public int sequenceRepetitions = 1;

    public void StartSpawnSequence(Spawner spawner)
    {
        spawner.StartCoroutine(spawner.SpawnSequence(timeBetweenSpawns, orderedSpawns, sequenceRepetitions));
    }
}
