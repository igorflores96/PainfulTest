using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship Info", menuName = "ScriptableObjects/Infos/ShipInfo", order = 1)]
public class ShipsInfo : ScriptableObject
{
    public float ShipSpeed;
    public float ShipRotationSpeed;
    public float ShipHealth;
    public float ShipDamageAttack;
}
