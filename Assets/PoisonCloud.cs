using Unity.VisualScripting;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    [SerializeField] LayerMask enemy;
    
    // When this function gets called, apply damage to targets within a radius of 3 from the player.
    public void CallItem( int damage, PlayerCombatManager player) {
        RaycastHit[] EnemiesAfflicted = Physics.SphereCastAll(transform.position, 3f, Vector3.forward, 3f, enemy, QueryTriggerInteraction.Collide);
        
        foreach (RaycastHit enemy in EnemiesAfflicted) {
            if (enemy.transform.CompareTag("BodyCollider")){
                IDamagable damagable = enemy.transform.GetComponentInParent<IDamagable>();
                if (damagable != null) {
                    damagable.doDamage(damage, true, player);
                    player.CreateNumberPopUp(enemy.transform.position, damage.ToString(), Color.magenta);
                }
            }
                
        }
    }
}
