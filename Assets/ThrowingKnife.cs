using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : MonoBehaviour {
    public PlayerCombatManager playerCombatManager;

    //Auto destroy object
    private void Start() {
        Destroy(gameObject, 5f);
    }

    //When the objects collides with a trigger that is damagable then do damage
    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            IDamagable damagable = other.transform.GetComponentInParent<IDamagable>();
            HitboxComponent hitPoint = other.transform.GetComponent<HitboxComponent>();
            if (damagable != null) {
                int dmg = playerCombatManager.AbilityDamageCalculate(playerCombatManager.ability1Damage, true, hitPoint.bodyPartString);
                damagable.doDamage(dmg, true, playerCombatManager);
                playerCombatManager.CreateNumberPopUp(other.transform.position, dmg.ToString(), Color.cyan);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
