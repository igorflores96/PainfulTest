using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUiManager : MonoBehaviour
{
    [SerializeField] private TimerData _timerData;
    [SerializeField] private TextMeshProUGUI _timerSession;
    [SerializeField] private TextMeshProUGUI _scoreSession;
    private float _currentTime;

    private void OnEnable() 
    {
        _currentTime = _timerData.SessionTime;
        _timerSession.text = _currentTime.ToString();
        _scoreSession.text = "0";
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

    }
}
