using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private DifficultyLevelSO difficultySettings;

    private List<(WeightedSpawnableObjectSO, int)> spawnItems = new List<(WeightedSpawnableObjectSO, int)>();

    float maxHeight;
    float minheight;

    private void Awake()
    {
        StartCoroutine(SpawnTimer(difficultySettings.TimeBetweenSpawns));

        foreach (var _object in difficultySettings.possibleSpawns)
        {
            spawnItems.Add((_object, _object.weight));
        }

        maxHeight = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        minheight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
    }

    private void SpawnEnemy()
    {
        WeightedSpawnableObjectSO objectToSpawn = Utility.RandomTools.PickRandomWeightedItem.PickRandomItemWeighted(spawnItems);

        var spawnedObject = Instantiate(objectToSpawn.prefab);

        spawnedObject.transform.position = transform.position + new Vector3(0, Random.Range(minheight, maxHeight), 0);

        if (spawnedObject.CompareTag("Projectile"))
        {
            var projectileScript = spawnedObject.GetComponent<Projectile>();
            projectileScript.Init(objectToSpawn.projectileSettings);
        }

        StartCoroutine(SpawnTimer(difficultySettings.TimeBetweenSpawns));
    }

    private IEnumerator SpawnTimer(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnEnemy();
    }
}
