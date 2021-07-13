using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private DifficultyLevelSO difficultySettings;

    private void Awake()
    {
        StartCoroutine(SpawnTimer(difficultySettings.TimeBetweenSpawns));
    }

    private void SpawnEnemy()
    {
        List<(GameObject, int)> items = new List<(GameObject, int)>();

        foreach (var _object in difficultySettings.possibleSpawns)
        {
            items.Add((_object.prefab, _object.weight));
        }

        GameObject objectToSpawn = Utility.RandomTools.PickRandomWeightedItem.PickRandomItemWeighted(items);

        var spawnedObject = Instantiate(objectToSpawn);

        spawnedObject.transform.position = transform.position;

        if (spawnedObject.CompareTag("Projectile"))
        {
            var projectileScript = spawnedObject.GetComponent<Projectile>();

            foreach (var _object in difficultySettings.possibleSpawns)
            {
                if (_object.prefab == objectToSpawn)
                {
                    projectileScript.Init(_object.projectileSettings);
                }
            }
        }

        StartCoroutine(SpawnTimer(difficultySettings.TimeBetweenSpawns));
    }

    private IEnumerator SpawnTimer(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnEnemy();
    }
}
