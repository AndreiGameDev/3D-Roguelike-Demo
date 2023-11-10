using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterManager : MonoBehaviour {
    public AIAnimationPlayer oneshot;
    public AICombatManager combatManager;
    public Animator animator;
    public bool isPerformingAction;
    public bool canMove;
    public bool isInAttackRange;
    public bool targetPlayer;

    void Awake() {
        oneshot = GetComponent<AIAnimationPlayer>();
        combatManager = GetComponent<AICombatManager>();
        animator = GetComponent<Animator>();
        if (animator == null) {
            animator = GetComponentInChildren<Animator>();
        }
    }
}
