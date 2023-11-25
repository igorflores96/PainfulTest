using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private bool _gameIsPaused;
    public UnityEvent OnGamePaused;
    public UnityEvent OnGameUnpaused;
    private void Awake() 
    {
        _playerInput = new PlayerInput();
        _playerInput.Pause.PauseGame.performed += PauseGame;
        _gameIsPaused = false;
        UnPause();
    }

    private void OnEnable() 
    {
        _playerInput.Enable();
    }

    private void OnDisable() 
    {
        _playerInput.Disable();    
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        if(context.performed)
        {

            if(!_gameIsPaused)
            {
                OnGamePaused?.Invoke();
                Time.timeScale = 0.0f;
                _gameIsPaused = true;
            } 
            else
            {
                OnGameUnpaused?.Invoke();
                Time.timeScale = 1.0f;
                _gameIsPaused = false;
            }
        }
    }

    private void UnPause()
    {
        Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
    }
}
