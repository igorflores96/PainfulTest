using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MenuTimerManager : MonoBehaviour
{
    [SerializeField] private TimerData _timeData;
    [SerializeField] private TextMeshProUGUI _sessionTimeFeedback;
    [SerializeField] private TextMeshProUGUI _spawnTimeFeedback;

    private void OnEnable()
    {
        _sessionTimeFeedback.text = _timeData.SessionTime.ToString();
        _spawnTimeFeedback.text = _timeData.SpawnEnemyTime.ToString();
    }

    public void SetTimeSession(float time)
    {
        time = Mathf.RoundToInt(time);
        _timeData.SessionTime = (float)time;
        _sessionTimeFeedback.text = _timeData.SessionTime.ToString();
    }

    public void SetTimeSpawn(float time)
    {
        time = Mathf.RoundToInt(time);
        _timeData.SpawnEnemyTime = (float)time;
        _spawnTimeFeedback.text = _timeData.SpawnEnemyTime.ToString();
    }

    
}
