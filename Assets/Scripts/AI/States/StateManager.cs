using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class StateManager : MonoBehaviour {
    public AIAnimationPlayer oneshot;
    [HideInInspector] public AINavigator navigator;
    AICharacterManager character;
    public State currentState;
    AICombatManager combatManager;
    [SerializeField] bool wizzard;
    private void Awake() {
        currentState.enabled = true;
    }
    private void Start() {
        navigator = GetComponentInParent<AINavigator>();
        oneshot = GetComponentInParent<AIAnimationPlayer>();
        character = GetComponent<AICharacterManager>();
        combatManager = GetComponentInParent<AICombatManager>();
    }

    // If the character has to target the player, switch between the idle,chase and attack state
    void Update()
    {
        if (character.targetPlayer) {
            RunStateMachine();
        }
        switch(currentState) {
            case IdleState:
                Idle();
                break;
            case ChaseState:
                Chase();
                break;
            case AttackState:
                Attack();
                break;
            default:
                break;
        }
    }

    // Tells the navigator to stop and plays the idle animation
    void Idle() {
        navigator.Stop();
        character.animator.SetBool("isMoving", false);
    }

    // Tells the navigator to move and plays the chase animation
    void Chase() {
        navigator.Move();
        character.animator.SetBool("isMoving", true);
    }

    // Tells the navigator to stop and plays the attack animation, if it's a wizzard then it also looks at the target
    void Attack() {
        navigator.Stop();
        if (wizzard) {
            combatManager.LookAt();
        }
        if(!character.isPerformingAction) {
            oneshot.PlayTargetActionAnimation("Attack", 1.5f);
        }
          
    }

    // Swapping states logic
    void RunStateMachine() {
        State nextState = currentState?.RunCurrentState();

        if(nextState != null ) {
            SwitchToTheNextState( nextState );
        }
    }

    // Switches to next state logic
    void SwitchToTheNextState(State nextState) {
        currentState.enabled = false;
        nextState.enabled = true;
        currentState = nextState;
    }
}

// States
enum States {
    IdleState,
    ChaseState,
    AttackState
}
