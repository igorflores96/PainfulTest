using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Resources;

[CreateAssetMenu(fileName = "Timer Data", menuName = "TimersData/Timer Data", order = 1)]
public class TimerData : ScriptableObject
{
    public float SessionTime;
    public float SpawnEnemyTime;
}
