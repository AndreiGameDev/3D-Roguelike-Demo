using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour, IInteract
{
    Animator animator;
    bool isOpen;
    private void Start() {
        animator = GetComponent<Animator>();
    }
    public void Interact(PlayerCombatManager player = null) {
        if(!isOpen) {
            isOpen = true;
            animator.SetTrigger("Open");
        }
    }
}
