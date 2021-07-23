using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour
{
    [SerializeField] private DifficultyLevelSO difficultySettings;

    private List<(WeightedSpawnSequenceSO, int)> spawnSequences = new List<(WeightedSpawnSequenceSO, int)>();

    float maxHeight;
    float minHeight;

    public void Init(float _maxHeight, float _minHeight)
    {
        maxHeight = _maxHeight;
        minHeight = _minHeight;

        StartCoroutine(SpawnSequenceTimer(difficultySettings.TimeBetweenSpawnSequences));

        foreach (var _object in difficultySettings.possibleSpawns)
        {
            spawnSequences.Add((_object, _object.weight));
        }
    }

    public void CleanupLeftoverObjects()
    {
        StartCoroutine(CleanupLeftoverObjectsCoroutine());
    }

    private IEnumerator CleanupLeftoverObjectsCoroutine()
    {
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        StartCoroutine(DestroyObjectListCoroutine(enemies));

        yield return new WaitUntil(() => coroutineFinished == true);

        List<GameObject> projectiles = GameObject.FindGameObjectsWithTag("Projectile").ToList();

        StartCoroutine(DestroyObjectListCoroutine(projectiles));
    }

    private bool coroutineFinished = false;
    private IEnumerator DestroyObjectListCoroutine(List<GameObject> objects)
    {
        coroutineFinished = false;
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.1f);

            if (objects[i].CompareTag("Enemy"))
            {
                objects[i].GetComponent<EnemyController>().StopAllCoroutines();
                objects[i].GetComponent<EnemyController>().KillEnemy();
            }
            else if (objects[i].CompareTag("Projectile"))
            {
                objects[i].GetComponent<Projectile>().HitTarget();
            }
        }

        coroutineFinished = true;
    }

    [SerializeField] private List<WeightedSpawnSequenceSO> previousSequences = new List<WeightedSpawnSequenceSO>();
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
                if (previousSequences.Contains(selectableSpawns[i].Item1))
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
        previousSequences.Add(spawnSequence);

        if (previousSequences.Count > 3)
        {
            previousSequences.RemoveAt(0);
        }

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
        else
        {
            var controllerScript = spawnedObject.GetComponent<EnemyController>();
            controllerScript.Init(objectToSpawn.enemyShipSettings);
        }
    }

    public IEnumerator SpawnSequence(float time, List<SpawnableObjectSO> spawnObjects, int repetitions, List<Vector2> offsets = null, bool allowRandomOffsets = false)
    {
        currentOffset = 0;
        var randomOffset = Vector2.zero;

        if (allowRandomOffsets)
        {
            randomOffset.y = Random.Range(maxHeight / 2, minHeight / 2);
        }

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

                var spawnOffset = randomOffset + GetSpawnOffset(offsets);

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