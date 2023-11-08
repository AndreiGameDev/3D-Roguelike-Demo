using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Character/BaseCharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string characterName;
    public int health;
    public int armor;
    public int movementSpeed;
    public int shield;
}