using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public enum EnemyTypes { Normal, Boss };

    [SerializeField] private InputManager player;
    [SerializeField] private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    [SerializeField] private GameObject[] enemyPrefabs;

    bool bossIsSpawned = false;


    private void Awake()
    {
        Instance = Instance ?? (this);

        PopulateSpawnPointList();

        foreach (var _spawnPoint in spawnPoints)
        {
            //if (!_spawnPoint.IsBoss)
                InstantiateEnemies(_spawnPoint);
        }
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
            float _randomOffset = Random.Range(_spawnPoint.Offset.x, _spawnPoint.Offset.y);
            float _pathPos = _spawnPoint.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition + _randomOffset;

            GameObject _obj = Instantiate(enemyPrefabs[(int)_spawnPoint.TypeOfEnemy], _spawnPoint.transform.position, _spawnPoint.transform.rotation, transform);
            _obj.GetComponent<EnemyParent>().InitializeMovementCam(_pathPos);
            _obj.GetComponent<EnemyParent>().Player = player;

            _spawnPoint.ActiveEnemyList.Add(_obj);
        }
    }

    public float ActiveEnemiesInCurrentBoundary()
    {
        int _amount = 0;

        foreach (var _spawnPoint in spawnPoints)
        {
            if (_spawnPoint.BoundaryIndex == BoundaryManager.Instance.CurrentBoundaryIndex)
                _amount += _spawnPoint.ActiveEnemyList.Count;
        }

        return _amount;
    }

    public void RemoveEnemy(GameObject _obj)
    {
        foreach (var _spawnPoint in spawnPoints)
        {
            GameObject _enemyToBeRemoved;

            foreach (var _enemy in _spawnPoint.ActiveEnemyList)
            {
                if (_enemy == _obj)
                    _enemyToBeRemoved = _enemy;
            }

            _spawnPoint.ActiveEnemyList.Remove(_obj);
        }

        CheckEnemyAmount();
    }

    public void CheckEnemyAmount()
    {
        //SPAWN DE BOSS EN VERANDER CAMERA EN ANDERE DINGEN
        foreach (var _spawnPoint in spawnPoints)
        {
            if (_spawnPoint.BoundaryIndex == BoundaryManager.Instance.CurrentBoundaryIndex && _spawnPoint.IsBoss && !bossIsSpawned)
            {
                InstantiateEnemies(_spawnPoint);
                SpawnBossEffects();
                bossIsSpawned = true;
            }
        }

        if (ActiveEnemiesInCurrentBoundary() == 0)
        {
            BoundaryManager.Instance.LiftBoundary();
        }
    }

    void SpawnBossEffects()
    {
        InputManager.Instance.SwitchToBossCamera();
        CameraShake.Instance.ApplyShake(5, 10, 3);
    }
}
