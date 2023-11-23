using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour, IDamageable
{
    [Header("Movement Variables")]
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _playerRotationSpeed;
    
    [Header("Bullets")]
    [SerializeField] private BulletPool _bulletStorage;
    [SerializeField] private float _yCannonBallOfsset;
    
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
    }

    private void OnDisable() 
    {
        _playerInput.Disable();    
    }

    private void Update() 
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        bool playerIsMoving = _playerInput.Movement.Move.ReadValue<float>() > 0.1f;
        
        if(playerIsMoving)
        {
            Vector3 moveDirection = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * Vector2.up;
            transform.Translate(-moveDirection * _playerSpeed * Time.deltaTime, Space.World);
        }
    }

    private void RotatePlayer()
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
            GameObject canonBall = _bulletStorage.GetBullet();
            
            Vector3 spawnPosition = transform.position - transform.up * _yCannonBallOfsset;
            canonBall.transform.position = spawnPosition;
            
            Bullet bullet;
            if(canonBall.TryGetComponent(out bullet))
            {
                bullet.SetStorageToReturn(_bulletStorage);
                bullet.SetDestiny(transform.rotation);
            }
        }
    }

    private void HeavyAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {       
        }
    }

    public void TakeDamage(int damageValue)
    {

    }
}
