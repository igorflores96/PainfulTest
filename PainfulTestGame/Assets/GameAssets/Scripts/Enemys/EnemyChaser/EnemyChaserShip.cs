using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyChaserShip : Ship
{
    
    [Header("Lifer Bar To Fill")]
    [SerializeField] private Image _lifeBar;

    [Header("Ship Destroyed Event")]
    public UnityEvent _OnShipDestroyed;
    private float _shipSpeed;
    private float _shipRotationSpeed;
    private float _shipHealth;
    private float _shipAttack;
    private Transform _playerTransform;
    private EnemysPool _enemyHarbor;
    private void OnEnable() 
    {
        _shipSpeed = ShipsAttributes.ShipSpeed;
        _shipRotationSpeed = ShipsAttributes.ShipRotationSpeed;
        _shipHealth = ShipsAttributes.ShipHealth;
        _shipAttack = ShipsAttributes.ShipDamageAttack;
        _lifeBar.fillAmount = _shipHealth / ShipsAttributes.ShipHealth;
    }

  
    void Update()
    {
        MoveShip();
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
        _lifeBar.fillAmount = _shipHealth / ShipsAttributes.ShipHealth;


        if(_shipHealth <= 0)
        {
            _enemyHarbor.ReturnEnemy(this.gameObject);
            _OnShipDestroyed?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Bullet bullet;
        if(other.TryGetComponent(out bullet))
        {
            TakeDamage(bullet.BulletDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {

        if(other.gameObject.CompareTag("Player"))
        {
            Ship playerShip;
            if(other.gameObject.TryGetComponent(out playerShip))
            {
                playerShip.TakeDamage(_shipAttack);
            }

            _enemyHarbor.ReturnEnemy(this.gameObject);
        }
        
    }

    public Transform PlayerTransform
    {
        set {_playerTransform = value;}
    }

    public EnemysPool EnemyHarbor
    {
        set {_enemyHarbor = value;}
    }

    public override UnityEvent OnShipDestroyed 
    { 
        get {return _OnShipDestroyed;}
        set {_OnShipDestroyed = value; }
    }
}
