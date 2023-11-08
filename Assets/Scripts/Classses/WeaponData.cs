using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    Physical,
    Magic
}

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Character/BaseWeaponStats")]
public class WeaponData : ScriptableObject {
    public string weaponName;
    public int baseDamage;
    public WeaponType weaponType;
}

