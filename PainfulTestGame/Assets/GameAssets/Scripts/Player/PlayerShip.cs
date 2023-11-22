using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _playerRotation;
    [SerializeField] private PlayerInput _playerInput;
    private Rigidbody2D _shipRb;

    private void Awake() {

        _shipRb = GetComponent<Rigidbody2D>();
        _playerInput = new PlayerInput();
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
            transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), _playerRotation);

        }
        else if(playerRotatingLeft)
        {
            transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), -_playerRotation);
        }
    }
}
