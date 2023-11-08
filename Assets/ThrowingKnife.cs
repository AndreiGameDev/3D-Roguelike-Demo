using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : MonoBehaviour {
    public PlayerCombatManager playerCombatManager;
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
    private void OnCollisionEnter(Collision collision) {
        if (!collision.gameObject.layer.Equals(LayerMask.GetMask("Player"))) {
            Destroy(transform.parent.gameObject);
        }
    }
}
