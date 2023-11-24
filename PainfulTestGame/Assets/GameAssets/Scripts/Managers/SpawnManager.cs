using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Player Position")]
    [SerializeField] private Transform _playerTransform;

    [Header("Timer Data")]
    [SerializeField] private TimerData _timerData;

    [Header("Enemys Pool")]
    [SerializeField] private EnemysPool _enemyPool;

    [Header("Positions to Spawn")]
    [SerializeField] private Transform[] _positions;

    private float _timeToSpawn;

    private void OnEnable() 
    {
        _timeToSpawn = _timerData.SpawnEnemyTime;    
    }

    private void Update() {
        _timeToSpawn -= 1.0f * Time.deltaTime;

        if(_timeToSpawn < 0f)
        {
            SpawnEnemy();
            _timeToSpawn = _timerData.SpawnEnemyTime;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = _enemyPool.GetEnemy();
        enemy.transform.position = _positions[Random.Range(0, _positions.Length)].position;

        EnemyChaserShip enemyShip;
        
        if(enemy.TryGetComponent(out enemyShip))
        {
            enemyShip.PlayerTransform = _playerTransform;
            enemyShip.EnemyHarbor = _enemyPool;
        }

        
    }
}
