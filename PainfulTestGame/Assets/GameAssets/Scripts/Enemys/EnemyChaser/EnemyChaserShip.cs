using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaserShip : Ship
{
    [SerializeField] private Transform _playerTransform;
    private float _shipSpeed;
    private float _shipRotationSpeed;
    private float _shipHealth;
    private float _shipAttack;
    private void OnEnable() 
    {
        _shipSpeed = ShipsAttributes.ShipSpeed;
        _shipRotationSpeed = ShipsAttributes.ShipRotationSpeed;
        _shipHealth = ShipsAttributes.ShipHealth;
        _shipAttack = ShipsAttributes.ShipDamageAttack;
    }

  
    void Update()
    {
        MoveShip();
        RotateShip();
    }

    public override void MoveShip()
    {        
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _shipSpeed * Time.deltaTime);
    }

    public override void  RotateShip()
    {
    }

    public override void TakeDamage(float value)
    {
        _shipHealth -= value;
        Debug.Log(_shipHealth);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            Ship playerShip;
            if(other.TryGetComponent(out playerShip))
            {
                playerShip.TakeDamage(_shipAttack);
            }

            Destroy(this.gameObject);
        }
        else
        {
            Bullet bullet;
            if(other.TryGetComponent(out bullet))
            {
                TakeDamage(bullet.BulletDamage);
            }
        }
    }
}
