using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectileReferenceAnim : MonoBehaviour
{
    AICombatManager aiManager;

    private void Start() {
        aiManager = GetComponentInParent<AICombatManager>();
    }

    public void SpawnProjectile() {
        aiManager.SpawnProjectile();
    }
}
