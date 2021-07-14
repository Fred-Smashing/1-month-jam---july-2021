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

    private void SelectSpawnSequence()
    {
        var spawnSequence = Utility.RandomTools.PickRandomWeightedItem.PickRandomItemWeighted(spawnSequences);

        spawnSequence.StartSpawnSequence(this);
    }

    private void SpawnObject(SpawnableObjectSO objectToSpawn)
    {
        if (objectToSpawn.isProjectile)
        {
            var spawnedObject = Instantiate(objectToSpawn.prefab);

            spawnedObject.transform.position = transform.position;

            var projectileScript = spawnedObject.GetComponent<Projectile>();
            projectileScript.Init(objectToSpawn.projectileSettings);
        }
    }

    public IEnumerator SpawnSequence(float time, List<SpawnableObjectSO> spawnObjects)
    {
        foreach (var _object in spawnObjects)
        {
            yield return new WaitForSeconds(time);

            SpawnObject(_object);
        }

        yield return new WaitForSeconds(time);

        StartCoroutine(SpawnSequenceTimer(difficultySettings.TimeBetweenSpawnSequences));
    }

    private IEnumerator SpawnSequenceTimer(float time)
    {
        yield return new WaitForSeconds(time);

        SelectSpawnSequence();
    }
}
