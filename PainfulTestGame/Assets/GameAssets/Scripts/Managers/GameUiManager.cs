using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUiManager : MonoBehaviour
{
    [SerializeField] private TimerData _timerData;
    [SerializeField] private GameObject _uiTexts;
    [SerializeField] private GameObject _finalScreen;
    [SerializeField] private TextMeshProUGUI _timerSession;
    [SerializeField] private TextMeshProUGUI _scoreSession;
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _mainMenuButton;    

    private float _currentTime;
    private int _currentScore;

    public UnityEvent OnGameIsOver;

    private void OnEnable() 
    {
        _playAgainButton.onClick.AddListener(PlayAgainIsCliked);
        _mainMenuButton.onClick.AddListener(MainMenuIsClicked);

        _currentTime = _timerData.SessionTime;
        _currentScore = 0;
        _timerSession.text = _currentTime.ToString();
        _scoreSession.text = "Score: " + _currentScore.ToString();
    }

    private void Update() 
    {
        DecreaseTime();
    }

    private void DecreaseTime()
    {
        _currentTime -= 1.0f * Time.deltaTime;
        if(_currentTime > 100f)
            _timerSession.text = "Time: " + _currentTime.ToString("000");
        else
            _timerSession.text = "Time: " + _currentTime.ToString("00");

        if(_currentTime < 0f)
        {
            _currentTime = 0.0f;
            ShowEndScreen();
        }
    }

    public void IncreaseScore()
    {
        _currentScore++;
        _scoreSession.text = "Score: " + _currentScore.ToString();
    }

    public void ShowEndScreen()
    {
        _finalScoreText.text = "Final Score: " + _currentScore.ToString();
        _uiTexts.SetActive(false);
        _finalScreen.SetActive(true);

        OnGameIsOver?.Invoke();
    }

    public void ShowPauseGameScreen()
    {
        _finalScoreText.text = "Current Score: " + _currentScore.ToString();
        _finalScreen.SetActive(true);
    }

    public void HidePauseGameScreen()
    {
        _finalScreen.SetActive(false);   
    }

    private void PlayAgainIsCliked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void MainMenuIsClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
