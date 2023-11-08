using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitboxComponent : MonoBehaviour
{
    public AICombatManager combatManager;
    Transform UniqueTarget = null;
    [SerializeField] bool isProjectile = false;
    private void Awake() {
        if(combatManager == null) {
            combatManager = GetComponentInParent<AICombatManager>();
        }
    }
    private void OnDisable() {
        UniqueTarget = null;
    }
    public void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            if(UniqueTarget != other.transform.root) {
                IDamagable damagable = other.GetComponent<IDamagable>();
                combatManager.Attack(damagable);
                UniqueTarget = other.transform.root;
                if(isProjectile) {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
