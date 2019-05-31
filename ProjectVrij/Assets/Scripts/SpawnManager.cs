using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public enum EnemyTypes { Normal, Boss };

    [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    [SerializeField] private GameObject[] enemyPrefabs;


    private void Awake()
    {
        Instance = Instance ?? (this);
    }

    void PopulateSpawnPointList()
    {
        spawnPoints.Clear();
        foreach (Transform _child in transform)
        {
            spawnPoints.Add(_child.GetComponent<SpawnPoint>());
        }
    }

    void InstantiateEnemies(SpawnPoint _spawnPoint)
    {
        for (int i = 0; i < _spawnPoint.Amount; i++)
        {
            Vector3 _randomOffset = new Vector3(Random.Range(_spawnPoint.Offset.x, _spawnPoint.Offset.y), 0, 0);

            Instantiate(enemyPrefabs[(int)_spawnPoint.TypeOfEnemy], _spawnPoint.transform.position + _randomOffset, _spawnPoint.transform.rotation, transform);
        }
    }
}
