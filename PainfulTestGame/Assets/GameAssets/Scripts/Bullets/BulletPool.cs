using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject _singleBulletPrefab;

    [SerializeField] private int _poolBulletQuantity;

    private Queue<GameObject> _bulletPool = new Queue<GameObject>();

    private void Awake() 
    {
        for(int bulletIndex = 0; bulletIndex < _poolBulletQuantity; bulletIndex++)
            CreateBullet();
    }

    private void CreateBullet()
    {
        GameObject bullet = Instantiate(_singleBulletPrefab);
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }

    public GameObject GetBullet()
    {
        if(_bulletPool.Count == 0)
            CreateBullet();
        
        GameObject bullet = _bulletPool.Dequeue();
        bullet.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _bulletPool.Enqueue(bullet);
    }
}
