using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectileReferenceAnim : MonoBehaviour
{
    //This script is only used as a reference for the animator keyevent
    AICombatManager aiManager;

    private void Start() {
        aiManager = GetComponentInParent<AICombatManager>();
    }

    public void SpawnProjectile() {
        aiManager.SpawnProjectile();
    }
}
