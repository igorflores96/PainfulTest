using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysPool : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _poolEnemyQuantity;

    private Queue<GameObject> _enemyPool = new Queue<GameObject>();

    private void Awake() 
    {
        for(int enemyIndex = 0; enemyIndex < _poolEnemyQuantity; enemyIndex++)
            CreateEnemy();
    }

    private void CreateEnemy()
    {
        GameObject enemy = Instantiate(_enemyPrefab);
        enemy.SetActive(false);
        _enemyPool.Enqueue(enemy);
    }

    public GameObject GetEnemy()
    {
        if(_enemyPool.Count == 0)
            CreateEnemy();
        
        GameObject enemy = _enemyPool.Dequeue();
        enemy.SetActive(true);

        return enemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        _enemyPool.Enqueue(enemy);
    }
}
