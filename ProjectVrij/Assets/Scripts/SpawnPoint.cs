using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private SpawnManager.EnemyTypes typeOfEnemy = SpawnManager.EnemyTypes.Normal;
    [SerializeField] private int amount;
    [SerializeField] private int boundaryIndex;
    [SerializeField] private bool useOffset = true;
    [SerializeField] private Vector2 offset;

    [SerializeField] private bool isBoss = false;

    public List<GameObject> ActiveEnemyList = new List<GameObject>();

    public SpawnManager.EnemyTypes TypeOfEnemy { get { return typeOfEnemy; } }
    public int Amount { get { return amount; } }
    public int BoundaryIndex { get { return boundaryIndex; } }
    public Vector2 Offset { get { return useOffset ? offset : Vector2.zero; } }
    public bool IsBoss { get { return isBoss; } }

    // bool spawnOnEnter or OnAwake ?
}
