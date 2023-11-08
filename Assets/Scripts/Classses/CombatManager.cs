using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CombatManager : MonoBehaviour, IDamagable {
    [Header("Testing_References")] // remove when done verifying the core system work
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
        
        Debug.Log("Initializing Stats");
        LoadBaseStats();
        LoadWeaponStats();
        baseMaxHealth = health;
        baseMaxShield = baseShield;
        maxHealth = baseMaxHealth;
        maxShield = baseMaxShield;
    }
    public void AmplifyStats() {
        armor = baseArmor * armorAmplifier;
        movementSpeed = baseMovementSpeed * movementSpeedAmplifier;
        maxShield = baseMaxShield * shieldAmplifier;
        maxHealth = baseMaxHealth * armorAmplifier;
    }
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
    public void doDamage(int dmgAmount, bool isPlayer, PlayerCombatManager player) {
        if (baseShield > 0) {
            baseShield -= dmgAmount;
            // Check if there's still damage left after the shield is depleted
            if (baseShield < 0) {
                float remainingDamage = Mathf.Abs(baseShield);
                baseShield = 0;
                // Deduct the remaining damage from the health
                health -= remainingDamage;
            }
        } else {
            // If the shield is already depleted, deduct damage directly from health
            health -= dmgAmount;
        }
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
