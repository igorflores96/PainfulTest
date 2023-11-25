using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShip : Ship
{   
    [Header("Bullets")]
    [SerializeField] private BulletPool _singleBulletStorage;
    [SerializeField] private float _yOffSetSingleAttack;
    [SerializeField] private float _xOffSetHeavyAttack;
    [SerializeField] private float _singleAttackCooldown;
    [SerializeField] private float _heavyAttackCooldown;
    [SerializeField] private int _bulletsForHeavyAttack;
    
    [Header("Life Bar To Fill")]
    [SerializeField] private Image _lifeBar;

    [Header("Ship Destroyed Event")]
    public UnityEvent _OnShipDestroyed;

    [Header("Animator Ship")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Sprite _firstSprite;
    private float _playerSpeed;
    private float _playerRotationSpeed;
    private float _playerHealth;
    private float _playerAttack;
    private float _currentSingleAttackTime;
    private float _currentHeavyAttackTime;

    private bool _canSingleAttack;
    private bool _canHeavyAttack;

    
    private PlayerInput _playerInput;

    private void Awake() 
    {
        _playerInput = new PlayerInput();
        _playerInput.Attack.SingleAttack.performed += SingleAttack;
        _playerInput.Attack.HeavyAttack.performed += HeavyAttack;
    }

    private void OnEnable() 
    {
        _playerInput.Enable();
        _playerSpeed = ShipsAttributes.ShipSpeed;
        _playerRotationSpeed = ShipsAttributes.ShipRotationSpeed;
        _playerHealth = ShipsAttributes.ShipHealth;
        _playerAttack = ShipsAttributes.ShipDamageAttack;
        _canSingleAttack = true;
        _canHeavyAttack = true;
        _currentHeavyAttackTime = _heavyAttackCooldown;
        _currentSingleAttackTime = _singleAttackCooldown;
        
    }

    private void OnDisable() 
    {
        _playerInput.Disable();    
    }

    private void Update() 
    {
        MoveShip();
        RotateShip();
        HandleAttacksCooldown();
    }

    public override void MoveShip()
    {
        bool playerIsMoving = _playerInput.Movement.Move.ReadValue<float>() > 0.1f;
        
        if(playerIsMoving)
        {
            Vector3 moveDirection = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * Vector2.up;
            transform.Translate(-moveDirection * _playerSpeed * Time.deltaTime, Space.World);
        }
    }

    public override void RotateShip()
    {

        bool playerRotatingRight = _playerInput.Movement.RotateR.ReadValue<float>() > 0.1f;
        bool playerRotatingLeft = _playerInput.Movement.RotateL.ReadValue<float>() > 0.1f;


        if(playerRotatingRight)
        {
            transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), _playerRotationSpeed);

        }
        else if(playerRotatingLeft)
        {
            transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), -_playerRotationSpeed);
        }
    }

    private void HandleAttacksCooldown()
    {
        if(!_canSingleAttack)
        {
            _currentSingleAttackTime -= 1.0f * Time.deltaTime;
            
            if(_currentSingleAttackTime < 0.0f)
            {
                _canSingleAttack = true;
                _currentSingleAttackTime = _singleAttackCooldown;
            }
                
        } 


        if(!_canHeavyAttack)
        {
            _currentHeavyAttackTime -= 1.0f * Time.deltaTime;
            
            if(_currentHeavyAttackTime < 0.0f)
            {
                _canHeavyAttack = true;
                _currentHeavyAttackTime = _heavyAttackCooldown;
            }
                
        }     
    }

    private void SingleAttack(InputAction.CallbackContext context)
    {
        if(context.performed && _canSingleAttack)
        {
            _canSingleAttack = false;

            GameObject canonBall = _singleBulletStorage.GetBullet();
            
            Vector3 spawnPosition = transform.position - transform.up * _yOffSetSingleAttack;
            canonBall.transform.position = spawnPosition;
            
            Bullet bullet;
            if(canonBall.TryGetComponent(out bullet))
            {
                bullet.BulletDamage = _playerAttack;
                bullet.SetStorageToReturn(_singleBulletStorage);
                bullet.SetDestinySingleShoot(transform.rotation);
            }
        }
    }

    private void HeavyAttack(InputAction.CallbackContext context)
    {
        if(context.performed && _canHeavyAttack)
        {
            _canHeavyAttack = false;

            for(int bulletQuantity = 0; bulletQuantity < _bulletsForHeavyAttack; bulletQuantity++)
            {
                GameObject canonBall = _singleBulletStorage.GetBullet();

                Vector3 spawnPosition = transform.position - transform.right * _xOffSetHeavyAttack;
                canonBall.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + bulletQuantity, spawnPosition.z);

                
                Bullet bullet;
                if(canonBall.TryGetComponent(out bullet))
                {
                    bullet.BulletDamage = _playerAttack;
                    bullet.SetStorageToReturn(_singleBulletStorage);
                    bullet.SetDestinyHeavyShoot(transform.rotation);
                }     
            }
        }
    }

    public override void TakeDamage(float damageValue)
    {
        _playerHealth -= damageValue;
        _lifeBar.fillAmount = _playerHealth / ShipsAttributes.ShipHealth;

        if(_playerHealth < (ShipsAttributes.ShipHealth / 2))
            _animator.SetBool("SecondDamage", true);
        else if(_playerHealth < ShipsAttributes.ShipHealth)
            _animator.SetBool("FirstDamage",true);


        if(_playerHealth <= 0)
        {
            _animator.SetBool("Died", true);
            _animator.SetBool("FirstDamage",false);
            _animator.SetBool("SecondDamage", false);
            
        }
    }

    private void PlayerDied()
    {
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

    public override UnityEvent OnShipDestroyed 
    { 
        get {return _OnShipDestroyed;}
        set {_OnShipDestroyed = value; }
    }
}
