using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractChest : MonoBehaviour, IInteract
{
    [SerializeField] List<GameObject> crystals = new List<GameObject>();
    public bool canUse;
    public Transform spawnPos1, spawnPos2;
    Animator animator;
    void Awake() {
        animator = GetComponent<Animator>();
    }

    // Opens the chest for the player then sets it to used
    public void Interact(PlayerCombatManager player = null) {
        
        if (canUse) {
            canUse = false;
            StartCoroutine(OpenChest());
        }
    }

    // Animation timer until it automatically plays the close animation
    IEnumerator OpenChest() {
        animator.SetTrigger("Open");
        yield return new WaitForSeconds(10f);
        animator.SetTrigger("Close");
    }

    // Spawns loot randomly
    public void SpawnLoot() {
        Instantiate(crystals[Random.Range(0, crystals.Count)], spawnPos1.position, spawnPos1.rotation);
        Instantiate(crystals[Random.Range(0, crystals.Count)], spawnPos2.position, spawnPos2.rotation);
    }
}
