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

    private void SpawnObject(SpawnableObjectSO objectToSpawn)
    {
        var spawnedObject = Instantiate(objectToSpawn.prefab);
        spawnedObject.transform.position = transform.position;

        if (objectToSpawn.isProjectile)
        {
            var projectileScript = spawnedObject.GetComponent<Projectile>();
            projectileScript.Init(objectToSpawn.projectileSettings, this.gameObject);
        }
    }

    public IEnumerator SpawnSequence(float time, List<SpawnableObjectSO> spawnObjects, int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {

            foreach (var _object in spawnObjects)
            {
                yield return new WaitForSeconds(time);

                SpawnObject(_object);
            }
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

internal struct NewStruct
{
    public WeightedSpawnSequenceSO WeightedSpawnSequenceSO;
    public int Item2;

    public NewStruct(WeightedSpawnSequenceSO weightedSpawnSequenceSO, int item2)
    {
        WeightedSpawnSequenceSO = weightedSpawnSequenceSO;
        Item2 = item2;
    }

    public override bool Equals(object obj)
    {
        return obj is NewStruct other &&
               EqualityComparer<WeightedSpawnSequenceSO>.Default.Equals(WeightedSpawnSequenceSO, other.WeightedSpawnSequenceSO) &&
               Item2 == other.Item2;
    }

    public override int GetHashCode()
    {
        int hashCode = -761140548;
        hashCode = hashCode * -1521134295 + EqualityComparer<WeightedSpawnSequenceSO>.Default.GetHashCode(WeightedSpawnSequenceSO);
        hashCode = hashCode * -1521134295 + Item2.GetHashCode();
        return hashCode;
    }

    public void Deconstruct(out WeightedSpawnSequenceSO weightedSpawnSequenceSO, out int item2)
    {
        weightedSpawnSequenceSO = WeightedSpawnSequenceSO;
        item2 = Item2;
    }

    public static implicit operator (WeightedSpawnSequenceSO WeightedSpawnSequenceSO, int)(NewStruct value)
    {
        return (value.WeightedSpawnSequenceSO, value.Item2);
    }

    public static implicit operator NewStruct((WeightedSpawnSequenceSO WeightedSpawnSequenceSO, int) value)
    {
        return new NewStruct(value.WeightedSpawnSequenceSO, value.Item2);
    }
}