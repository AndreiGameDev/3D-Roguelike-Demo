using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CombatManager : MonoBehaviour, IDamagable {
    public CharacterStats characterStats;
    public WeaponData weaponData;

    [Header("Stats")]
    [Space(10)]
    [Header("CharacterStats")]
    [HideInInspector] public string characterName;
    [HideInInspector] public float baseHealth;
    public float health;
    [HideInInspector] public float baseArmor;
    public float armor;
    [HideInInspector] public float baseMovementSpeed;
    public float movementSpeed;
    [HideInInspector] public float baseShield;
    public float shield;
    [HideInInspector] public float baseMaxHealth;
    public float maxHealth;
    [HideInInspector] public float baseMaxShield;
    public float maxShield;
    [Space(5)]
    [Header("WeaponStats")]
    [HideInInspector] public string weaponName;
    [HideInInspector] public float basePrimaryDamage;
    [HideInInspector] public WeaponType weaponType;
    [Space(10)]
    [Header("Amplifiers")]
    public float baseAplifierValue = 1f;
    public float abilityDamageAmplifier = 1f;
    public float primaryDamageAmplifier = 1f;
    public float healingAmplifier = 1f;
    public float movementSpeedAmplifier = 1f;
    public float attackSpeedAmplifier = 1f;
    public float shieldAmplifier = 1f;
    public float armorAmplifier = 1f;
    public float healthAmplifier = 1f;
    public float critDamageAmplifier = 1.5f;

    public void Start() {
        LoadBaseStats();
        LoadWeaponStats();
        baseMaxHealth = health;
        baseMaxShield = baseShield;
        maxHealth = baseMaxHealth;
        maxShield = baseMaxShield;
    }
    
    // Amplifies stats
    public void AmplifyStats() {
        armor = baseArmor * armorAmplifier;
        movementSpeed = baseMovementSpeed * movementSpeedAmplifier;
        maxShield = baseMaxShield * shieldAmplifier;
        maxHealth = baseMaxHealth * armorAmplifier;
    }

    // Checks if the hit component is a head or body
    bool IsHitHeadshot(string ColliderShot) {
        if (ColliderShot == "Head") {
            return true;
        } else if (ColliderShot == "Body") {
            return false;
        } else {
            Debug.Log("Body part not found, returning false");
            return false;
        }
    }

    // Calculates ability damage with multipliers
    public int AbilityDamageCalculate(float AbilityDamage, bool canHeadshot = false, string HitBodypartName = "") {
        if (canHeadshot) {
            if (IsHitHeadshot(HitBodypartName)) {
                return Mathf.FloorToInt((AbilityDamage * critDamageAmplifier) * abilityDamageAmplifier);
            } else {
                return Mathf.FloorToInt(AbilityDamage * abilityDamageAmplifier);
            }
        } else {
            return Mathf.FloorToInt(AbilityDamage * abilityDamageAmplifier);
        }
    }

    // Calculates basic attack damage with multipliers
    public int PrimaryDamageCalculate(float BaseDamage, bool canHeadshot = false, string HitBodypartName = "") {
        if (canHeadshot) {
            if (IsHitHeadshot(HitBodypartName)) {
                return Mathf.FloorToInt((BaseDamage * critDamageAmplifier) * primaryDamageAmplifier);
            } else {
                return Mathf.FloorToInt(BaseDamage * primaryDamageAmplifier);
            }
        } else {
            return Mathf.FloorToInt(BaseDamage * primaryDamageAmplifier);
        }

    }

    // Adds health and clamps it so it doesn't go above max health
    public void Heal(int healthToAdd) {
        health += healthToAdd;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public virtual void LoadWeaponStats() {
        weaponName = weaponData.weaponName;
        weaponType = weaponData.weaponType;
        basePrimaryDamage = Mathf.RoundToInt(weaponData.baseDamage);
    }

    private void LoadBaseStats() {
        characterName = characterStats.characterName;
        baseHealth = characterStats.health;
        baseArmor = characterStats.armor;
        baseMovementSpeed = characterStats.movementSpeed;
        baseShield = characterStats.shield;
        health = baseHealth;
        armor = baseArmor;
        movementSpeed = baseMovementSpeed;
        shield = baseShield;
    }

    // Does damage calculation subtracting shields and armors and if it's a player killing the target then do player specific functions
    public void doDamage(int dmgAmount, bool isPlayer, PlayerCombatManager player) {
        if (baseShield > 0) {
            baseShield -= dmgAmount;
            if (baseShield < 0) {
                float remainingDamage = Mathf.Abs(baseShield);
                baseShield = 0;
                health -= remainingDamage;
            }
        } else {
            health -= dmgAmount;
        }

        // If the player kills this object do this
        if (isPlayer) {
            if (health <= 0) {
                player.OnKillItemEffect();
                player.UIManager.IncreaseScore(10);
                GameManager.Instance.Score += 10f;
                Destroy(gameObject);
            }
        }
    }
}
