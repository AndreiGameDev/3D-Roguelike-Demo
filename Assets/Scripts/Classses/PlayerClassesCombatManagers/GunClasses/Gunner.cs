using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Gunner : PlayerCombatManager {

    //Gun variables
    [SerializeField] GunWeaponData gunData;
    public float fireRate;
    public float maxBulletHitRange;
    public int maxTargetsPenetrate;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] GameObject VFX_BloodSplatter;
    // Primary attack animation name
    string PrimaryAttackAnimationArms = "A_Arm_Fire";
    string PrimaryAttackAnimationWeapon = "A_Glock_Fire";
    [SerializeField] Recoil _recoil;
    // Ability 2
    [SerializeField] GameObject dynamitePrefab;
    [SerializeField] float cooldownAbility2 = 10f;
    bool canUseAbility2 = true;
    // Ability 1
    [SerializeField] float cooldownAbility1 = 5f;
    bool canUseAbility1 = true;
    [SerializeField] GameObject throwingKnifePrefab;
    //Secondary Ability
    [SerializeField] float cooldownSecondaryAbility = 20f;
    bool canUseSecondaryAbility = true;
    [SerializeField] Transform projectileTransform;
    private new void Start() {
        base.Start();
        fireRate = gunData.fireRate;
        maxBulletHitRange = gunData.maxBulletHitRange;
        maxTargetsPenetrate = gunData.maxTargetsPenetrate;
        ability1Damage = 10;
        ability2Damage = 15;

        basePrimaryDamage *= difficultyDamage;
        ability1Damage *= difficultyDamage;
        ability2Damage *= difficultyDamage;
    }

    // Shots a raycast, if it collides and detects damagable objects then do damage.
    public void WeaponFire() {
        _recoil.RecoilLogic();

        RaycastHit[] hits = Physics.RaycastAll(fpsCamera.transform.position, fpsCamera.transform.forward, maxBulletHitRange, layersToHit, QueryTriggerInteraction.Collide);
        Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance)); // Sort the hits by distance from the ray's origin in ascending order
        int penetratedTargets = 0; // Initialize a counter for penetrated targets
        Transform previousHitObject = null;

        foreach (RaycastHit hit in hits) {
            Transform currentHitObject = hit.transform.root;
            if (previousHitObject != currentHitObject) {
                GameObject hitObject = hit.transform.gameObject;
                HitboxComponent hitbox = hitObject.GetComponent<HitboxComponent>();
                IDamagable damagable = hitbox.parentIDamagable;

                // If the object hit has the damagable interface then apply damage functions
                if (damagable != null) {
                    penetratedTargets++;
                    int dmgAmount = PrimaryDamageCalculate(basePrimaryDamage, true, hitbox.bodyPartString);
                    damagable.doDamage(dmgAmount, true, this);

                    if (hitbox.bodyPartString == "Head") {
                        CreateNumberPopUp(hitObject.transform.position, dmgAmount.ToString(), Color.yellow);
                    } else {
                        CreateNumberPopUp(hitObject.transform.position, dmgAmount.ToString(), Color.white);
                    }

                    CreateBloodSplatter(hit);
                    previousHitObject = currentHitObject;

                    if (penetratedTargets >= maxTargetsPenetrate) {
                        break;
                    }
                } else {
                    // If the object hit has no damagable interface then continue to the next target
                    continue;
                }
            }
        }
    }

    // Create bloodsplatter at hit position
    void CreateBloodSplatter(RaycastHit hit) {
        Vector3 incomingVector = hit.point - transform.root.position;
        Vector3 reflectVector = Vector3.Reflect(incomingVector, hit.normal);

        GameObject temp = Instantiate(VFX_BloodSplatter, hit.point, Quaternion.Euler(reflectVector));
        GameObject.Destroy(temp, 1f);
    }

    // Plays primary attack animation, the animation has the weapon damage function
    public override void PrimaryAttackLogic() {
        animator.PlayTargetActionAnimation(PrimaryAttackAnimationArms, PrimaryAttackAnimationWeapon, true, attackSpeedAmplifier);
    }

    // Starts coroutine for secondary attack
    public override void SecondaryAttackLogic() {
        if (canUseSecondaryAbility) {
            StartCoroutine(SecondAttackAction());
        }
    }

    // Ability 1 logic - throws a throwing knife where the player's crosshair is aiming at
    public override void Abillity1Logic() {
        if (canUseAbility1) {
            canUseAbility1 = false;
            StartCoroutine(CooldownAbility1());
            GameObject throwKnife = Instantiate(throwingKnifePrefab, fpsCamera.transform.position, fpsCamera.transform.rotation);
            ThrowingKnife throwKnifeScript = throwKnife.GetComponentInChildren<ThrowingKnife>();
            throwKnifeScript.playerCombatManager = this;
        }
    }

    // Ability 2 logic - throws a dynamite where the player's crosshair is aiming at
    public override void Abillity2Logic() {
        if (canUseAbility2) {
            canUseAbility2 = false;
            StartCoroutine(CooldownAbility2());
            GameObject dynamite = Instantiate(dynamitePrefab, fpsCamera.transform.position, fpsCamera.transform.rotation);
            BombScript dynamiteScript = dynamite.GetComponentInChildren<BombScript>();
            dynamiteScript.playerCombatManager = this;
        }
    }

    //Cooldown logic lines 133-163, It decreases by 1s until it reaches 0 then sets the variable to true and resets the cd
    IEnumerator CooldownAbility1() {
        yield return new WaitForSecondsRealtime(1f);
        cooldownAbility1 -= 1f;
        if (cooldownAbility1 >= 0) {
            StartCoroutine(CooldownAbility1());
        } else {
            cooldownAbility1 = 5f;
            canUseAbility1 = true;
        }
    }
    IEnumerator CooldownAbility2() {
        yield return new WaitForSecondsRealtime(1f);
        cooldownAbility2 -= 1f;
        if (cooldownAbility2 >= 0) {
            StartCoroutine(CooldownAbility2());
        } else {
            cooldownAbility2 = 10f;
            canUseAbility2 = true;
        }
    }

    IEnumerator CooldownAbilitySecondaryAbility() {
        yield return new WaitForSecondsRealtime(1f);
        cooldownSecondaryAbility -= 1f;
        if (cooldownSecondaryAbility >= 0) {
            StartCoroutine(CooldownAbilitySecondaryAbility());
        } else {
            canUseSecondaryAbility = true;
            cooldownSecondaryAbility = 20f;
        }
    }

    // Secondary attack coroutine, this boosts the player's stats for 5seconds then reverts back to normal
    IEnumerator SecondAttackAction() {
        canUseSecondaryAbility = false;
        StartCoroutine(CooldownAbilitySecondaryAbility());
        attackSpeedAmplifier += .5f;
        movementSpeed += .5f;
        abilityDamageAmplifier += .5f;
        primaryDamageAmplifier += .5f;
        yield return new WaitForSeconds(5f);
        attackSpeedAmplifier -= .5f;
        movementSpeed -= .5f;
        abilityDamageAmplifier -= .5f;
        primaryDamageAmplifier -= .5f;
    }
}
