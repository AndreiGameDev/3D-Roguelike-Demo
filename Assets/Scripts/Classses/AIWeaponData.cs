using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Weapon Data", menuName = "Character/AIWeaponStats")]
public class AIWeaponData : ScriptableObject
{
    public string weaponName;
    public int baseDamage;
    public float attackRange;
}
