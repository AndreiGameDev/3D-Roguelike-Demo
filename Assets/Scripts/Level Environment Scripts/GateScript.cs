using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour, IInteract {
    GameManager gameManager;
    public bool isOpen;
    Collider gateCollider;
    Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        gateCollider = GetComponent<Collider>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        StartCoroutine(DoorManager());
    }

    // Checks if the player is in a wave, if it is then close the door if it was open, if the player is no longer in the wave then don't open it.
    IEnumerator DoorManager() {
        yield return new WaitForSeconds(.5f);
        if (gameManager.isInWave && isOpen) {
            DoorClose();
        } else if (!gameManager.isInWave && isOpen) {
            doorOpen();
        }
        StartCoroutine(DoorManager());
    }

    // Opens door
    public void doorOpen() {
        animator.SetBool("DoorOpen", true);
        gateCollider.enabled = false;
    }

    // Closes door
    public void DoorClose() {
        animator.SetBool("DoorOpen", false);
        gateCollider.enabled = true;
    }

    // Interact function the player interacts with
    public void Interact(PlayerCombatManager player = null) {
        if (!isOpen && gameManager.isInWave == false) {
            isOpen = true;
            animator.SetBool("Interacted", true);
            doorOpen();
        }

    }
}
