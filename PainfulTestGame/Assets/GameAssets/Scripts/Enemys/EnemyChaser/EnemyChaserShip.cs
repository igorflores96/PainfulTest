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
    
    [Header("Animator Ship")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Sprite _firstSprite;
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
        RotateShip();
    }

    public override void MoveShip()
    {   
        transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _shipSpeed * Time.deltaTime);
    }

    public override void  RotateShip()
    {
        Vector2 direction = _playerTransform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + _shipRotationSpeed));
    }

    public override void TakeDamage(float value)
    {

        _shipHealth -= value;
        _lifeBar.fillAmount = _shipHealth / ShipsAttributes.ShipHealth;

        if(_shipHealth < (ShipsAttributes.ShipHealth / 2))
            _animator.SetBool("SecondDamage", true);
        else if(_shipHealth < ShipsAttributes.ShipHealth)
            _animator.SetBool("FirstDamage",true);


        if(_shipHealth <= 0)
        {
            _animator.SetBool("Died", true);
            _animator.SetBool("FirstDamage",false);
            _animator.SetBool("SecondDamage", false);
            
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

    private void ReturnShip()
    {
        GetComponent<SpriteRenderer>().sprite = _firstSprite;
        _enemyHarbor.ReturnEnemy(this.gameObject);
        _OnShipDestroyed?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {

        if(other.gameObject.CompareTag("Player"))
        {
            Ship playerShip;
            if(other.gameObject.TryGetComponent(out playerShip))
            {
                playerShip.TakeDamage(_shipAttack);
                _animator.SetBool("Died", true);
                _animator.SetBool("FirstDamage",false);
                _animator.SetBool("SecondDamage", false);
            }


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
