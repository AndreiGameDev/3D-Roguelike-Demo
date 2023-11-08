using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICombatManager : CombatManager
{
    [SerializeField] public float AttackRange;
    [SerializeField] AIWeaponData AIWeaponData;
    [SerializeField] public WeaponHitboxComponent meleeHitbox = null;
    bool isMeleeHitboxActive;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnpoint;
    public override void LoadWeaponStats() {
        weaponName = AIWeaponData.weaponName;
        AttackRange = AIWeaponData.attackRange;
        basePrimaryDamage = AIWeaponData.baseDamage;
    }
    public void ToggleHitbox() {
        if(isMeleeHitboxActive) {
            isMeleeHitboxActive = false;
            meleeHitbox.enabled = false;
        }else if(!isMeleeHitboxActive){
            isMeleeHitboxActive = true;
            meleeHitbox.enabled = true;
        }
    }
    public void Attack(IDamagable target) {
        target.doDamage(PrimaryDamageCalculate(basePrimaryDamage), false);
    }

    public void LookAt() {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    public void SpawnProjectile() {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnpoint.position, Quaternion.LookRotation(player.transform.position - projectileSpawnpoint.transform.position) );
        WeaponHitboxComponent projectileScript = projectile.GetComponent<WeaponHitboxComponent>();
        projectileScript.combatManager = this;
    }
}
