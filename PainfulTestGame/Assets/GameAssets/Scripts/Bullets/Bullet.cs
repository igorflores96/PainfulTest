using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletVelocity;
    [SerializeField] private Rigidbody2D _bulletRb;
    private BulletPool _bulletStorage;
    private float _bulletDamage;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        ReturnBullet();
    }

    public void SetDestinySingleShoot(Quaternion rotation)
    {
        Vector3 moveDirection = Quaternion.Euler(0, 0, rotation.eulerAngles.z) * Vector2.up;
        _bulletRb.AddForce(-moveDirection * _bulletVelocity, ForceMode2D.Impulse);
    }

    public void SetDestinyHeavyShoot(Quaternion rotation)
    {
        Vector3 moveDirection = Quaternion.Euler(0, 0, rotation.eulerAngles.z) * Vector2.right;
        _bulletRb.AddForce(-moveDirection * _bulletVelocity, ForceMode2D.Impulse);
    }
    public void SetStorageToReturn(BulletPool pool)
    {
        _bulletStorage = pool;
    }

    private void ReturnBullet()
    {
        _bulletStorage.ReturnBullet(this.gameObject);
    }

    public float BulletDamage
    {
        get {return _bulletDamage;}
        set {_bulletDamage = value;}
    }
}
