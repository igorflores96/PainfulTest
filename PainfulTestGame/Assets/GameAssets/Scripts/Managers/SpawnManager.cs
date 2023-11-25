using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Player Position")]
    [SerializeField] private Transform _playerTransform;

    [Header("Timer Data")]
    [SerializeField] private TimerData _timerData;

    [Header("Enemys Pool")]
    [SerializeField] private EnemysPool[] _enemysPool;

    [Header("Positions to Spawn")]
    [SerializeField] private Transform[] _positions;
    
    [Header("Bullets for Shooter")]
    [SerializeField] private BulletPool _bulletPool;

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
        int randEnemy = Random.Range(0, _enemysPool.Length);
        int randPosition = Random.Range(0, _positions.Length);

        GameObject enemy = _enemysPool[randEnemy].GetEnemy();
        enemy.transform.position = _positions[randPosition].position;

        EnemyChaserShip enemyChaserShip;
        
        if(enemy.TryGetComponent(out enemyChaserShip))
        {
            enemyChaserShip.PlayerTransform = _playerTransform;
            enemyChaserShip.EnemyHarbor = _enemysPool[randEnemy];
        }

        EnemyShooterShip enemyShooterShip;
        if(enemy.TryGetComponent(out enemyShooterShip))
        {
            enemyShooterShip.PoolBullet = _bulletPool;
            enemyShooterShip.PlayerTransform = _playerTransform;
            enemyShooterShip.EnemyHarbor = _enemysPool[randEnemy];
        }

        
    }
}
