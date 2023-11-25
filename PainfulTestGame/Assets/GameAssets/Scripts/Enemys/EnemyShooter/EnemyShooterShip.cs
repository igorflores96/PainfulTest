using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyShooterShip : Ship
{
    [Header("Lifer Bar To Fill")]
    [SerializeField] private Image _lifeBar;

    [Header("Ship Destroyed Event")]
    public UnityEvent _OnShipDestroyed;

    [Header("Bullets")]
    [SerializeField] private BulletPool _singleBulletStorage;
    [SerializeField] private float _yOffSetSingleAttack;
    [SerializeField] private float _timeToNextAttack;

    [Header("Distance to Shoot")]
    [SerializeField] private float _distanceToShoot;

    [Header("Animator Ship")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Sprite _firstSprite;

    private float _shipSpeed;
    private float _shipRotationSpeed;
    private float _shipHealth;
    private float _shipAttack;
    private float _currentAttackTime;
    private bool _canAttack;
    private Transform _playerTransform;
    private EnemysPool _enemyHarbor;
    private void OnEnable() 
    {
        _shipSpeed = ShipsAttributes.ShipSpeed;
        _shipRotationSpeed = ShipsAttributes.ShipRotationSpeed;
        _shipHealth = ShipsAttributes.ShipHealth;
        _shipAttack = ShipsAttributes.ShipDamageAttack;
        _lifeBar.fillAmount = _shipHealth / ShipsAttributes.ShipHealth;
        _currentAttackTime = _timeToNextAttack;
        _canAttack = true;
    }

  
    void Update()
    {
        Vector3 distancePlayer = transform.position - _playerTransform.position;
        float distanceResult = distancePlayer.magnitude;
        
        if(distanceResult > _distanceToShoot) 
            MoveShip();
        else if(_canAttack)
        {
            Fire();
        }


        RotateShip();
        HandleAttackTime();
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

    private void Fire()
    {
        GameObject canonBall = _singleBulletStorage.GetBullet();
        
        Vector3 spawnPosition = transform.position - transform.up * _yOffSetSingleAttack;
        canonBall.transform.position = spawnPosition;
        
        Bullet bullet;
        if(canonBall.TryGetComponent(out bullet))
        {
            bullet.BulletDamage = _shipAttack;
            bullet.SetStorageToReturn(_singleBulletStorage);
            bullet.SetDestinySingleShoot(transform.rotation);
        }

        _canAttack = false;
    }

    private void HandleAttackTime()
    {
        if(!_canAttack)
        {
            _currentAttackTime -= 1.0f * Time.deltaTime;
            
            if(_currentAttackTime < 0.0f)
            {
                _canAttack = true;
                _currentAttackTime = _timeToNextAttack;
            }
                
        }            
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

    private void ReturnShip()
    {
        GetComponent<SpriteRenderer>().sprite = _firstSprite;
        _enemyHarbor.ReturnEnemy(this.gameObject);
        _OnShipDestroyed?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Bullet bullet;
        if(other.TryGetComponent(out bullet))
        {
            TakeDamage(bullet.BulletDamage);
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

    public BulletPool PoolBullet
    {
        set {_singleBulletStorage = value;}
    }
}
