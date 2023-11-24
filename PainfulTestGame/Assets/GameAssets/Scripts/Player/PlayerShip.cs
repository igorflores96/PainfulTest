using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShip : Ship
{   
    [Header("Bullets")]
    [SerializeField] private BulletPool _singleBulletStorage;
    [SerializeField] private float _yOffSetSingleAttack;
    [SerializeField] private float _xOffSetHeavyAttack;
    [SerializeField] private int _bulletsForHeavyAttack;
    
    [Header("Life Bar To Fill")]
    [SerializeField] private Image _lifeBar;
    private float _playerSpeed;
    private float _playerRotationSpeed;
    private float _playerHealth;
    private float _playerAttack;
    
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

    }

    private void OnDisable() 
    {
        _playerInput.Disable();    
    }

    private void Update() 
    {
        MoveShip();
        RotateShip();
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

    private void SingleAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
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
        if(context.performed)
        {
            for(int bulletQuantity = 0; bulletQuantity < _bulletsForHeavyAttack; bulletQuantity++)
            {
                GameObject canonBall = _singleBulletStorage.GetBullet();

                Vector3 spawnPosition = transform.position - transform.right * _xOffSetHeavyAttack;
                canonBall.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + bulletQuantity, spawnPosition.z);

                
                Bullet bullet;
                if(canonBall.TryGetComponent(out bullet))
                {
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
    }
}
