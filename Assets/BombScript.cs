using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BombScript : MonoBehaviour {

    public PlayerCombatManager playerCombatManager;
    [SerializeField] int durationToExplode = 3;
    [SerializeField] float knockbackForce = 5f;
    
    private void Start() {
        StartCoroutine(Bomb());
    }
    public IEnumerator Bomb() {
        yield return new WaitForSeconds(durationToExplode);
        Explode();
        Destroy(transform.parent.gameObject);
    }
    void Explode() {
        RaycastHit[] ExplosionHits = Physics.SphereCastAll(transform.parent.position, 3f, Vector3.forward, 3f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Collide);
        
        foreach (RaycastHit hit in ExplosionHits) {
            if (hit.transform.CompareTag("BodyCollider")) {
                
                Transform target = FindParentWithTag(hit.transform.gameObject, "EnemyParent").transform;
                IDamagable damagable = target.GetComponent<IDamagable>();
                AINavigator aiNav = target.GetComponent<AINavigator>();
                if (damagable != null) {
                    damagable.doDamage(playerCombatManager.AbilityDamageCalculate(playerCombatManager.ability2Damage), true, playerCombatManager);
                    playerCombatManager.CreateNumberPopUp(hit.transform.position ,playerCombatManager.AbilityDamageCalculate(playerCombatManager.ability2Damage).ToString(), Color.cyan);
                    Vector3 dir = (target.position - transform.position).normalized;
                    dir = dir * knockbackForce / 2;
                    dir = new Vector3(dir.x, 0, dir.z);
                    aiNav.agent.Move(dir);
                }
            }
        }
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag) {
        Transform t = childObject.transform;
        while (t.parent != null) {
            if (t.parent.tag == tag) {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }
}
