using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Data", menuName = "Character/GunProperties")]
public class GunWeaponData : ScriptableObject
{
    public float fireRate;
    public float maxBulletHitRange;
    public int maxTargetsPenetrate;
}