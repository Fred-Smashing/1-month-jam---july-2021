using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private DifficultyLevelSO difficultySettings;

    private List<(WeightedSpawnSequenceSO, int)> spawnSequences = new List<(WeightedSpawnSequenceSO, int)>();

    float maxHeight;
    float minheight;

    private void Awake()
    {
        StartCoroutine(SpawnSequenceTimer(difficultySettings.TimeBetweenSpawnSequences));

        foreach (var _object in difficultySettings.possibleSpawns)
        {
            spawnSequences.Add((_object, _object.weight));
        }

        maxHeight = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        minheight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
    }

    [SerializeField] private WeightedSpawnSequenceSO previousSequence = null;
    private List<(WeightedSpawnSequenceSO, int)> selectableSpawns = new List<(WeightedSpawnSequenceSO, int)>();
    private void SelectSpawnSequence()
    {
        selectableSpawns.Clear();

        foreach (var spawn in spawnSequences)
        {
            selectableSpawns.Add(spawn);
        }

        if (selectableSpawns.Count > 1)
        {
            for (int i = selectableSpawns.Count - 1; i >= 0; i--)
            {
                if (selectableSpawns[i].Item1 == previousSequence)
                {
                    selectableSpawns.RemoveAt(i);
                }
            }
        }

        var spawnSequence = Utility.RandomTools.PickRandomWeightedItem.PickRandomItemWeighted(selectableSpawns);
        StartSpawnSequence(spawnSequence);
    }

    private void StartSpawnSequence(WeightedSpawnSequenceSO spawnSequence)
    {
        previousSequence = spawnSequence;
        spawnSequence.StartSpawnSequence(this);
    }

    private void SpawnObject(SpawnableObjectSO objectToSpawn, Vector2 spawnOffset)
    {
        var spawnedObject = Instantiate(objectToSpawn.prefab);
        spawnedObject.transform.position = transform.position + (Vector3)spawnOffset;

        if (objectToSpawn.isProjectile)
        {
            var projectileScript = spawnedObject.GetComponent<Projectile>();
            projectileScript.Init(objectToSpawn.projectileSettings, this.gameObject);
        }
    }

    public IEnumerator SpawnSequence(float time, List<SpawnableObjectSO> spawnObjects, int repetitions, List<Vector2> offsets = null)
    {
        currentOffset = 0;

        if (offsets == null || offsets.Count <= 0)
        {
            offsets = new List<Vector2>();
            offsets.Add(Vector2.zero);
        }

        for (int i = 0; i < repetitions; i++)
        {

            foreach (var _object in spawnObjects)
            {
                yield return new WaitForSeconds(time);

                var spawnOffset = GetSpawnOffset(offsets);

                SpawnObject(_object, spawnOffset);
            }
        }

        yield return new WaitForSeconds(time);

        StartCoroutine(SpawnSequenceTimer(difficultySettings.TimeBetweenSpawnSequences));
    }

    private int currentOffset = 0;
    private Vector2 GetSpawnOffset(List<Vector2> offsets)
    {
        var chosenOffset = offsets[currentOffset];

        currentOffset++;

        if (currentOffset > offsets.Count - 1)
        {
            currentOffset = 0;
        }

        return chosenOffset;
    }

    private IEnumerator SpawnSequenceTimer(float time)
    {
        yield return new WaitForSeconds(time);

        SelectSpawnSequence();
    }
}