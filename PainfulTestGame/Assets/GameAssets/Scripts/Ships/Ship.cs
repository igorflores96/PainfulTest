using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ship : MonoBehaviour, IDamageable
{
    [Header("Player Ship's Attributes")]
    public ShipsInfo ShipsAttributes;
    public abstract void MoveShip();
    public abstract void RotateShip();
    public abstract void TakeDamage(float value);
    public abstract UnityEvent OnShipDestroyed { get; set; }

}
