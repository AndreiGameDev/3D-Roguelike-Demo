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
    void Update()
    {
        if (character.hasObjective) {
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
    void Idle() {
        navigator.Stop();
        character.animator.SetBool("isMoving", false);
    }
    void Chase() {
        navigator.Move();
        character.animator.SetBool("isMoving", true);
    }
    void Attack() {
        navigator.Stop();
        if (wizzard) {
            combatManager.LookAt();
        }
        if(!character.isPerformingAction) {
            oneshot.PlayTargetActionAnimation("Attack", 1.5f);
        }
          
    }
    void RunStateMachine() {
        State nextState = currentState?.RunCurrentState();

        if(nextState != null ) {
            SwitchToTheNextState( nextState );
        }
    }

    void SwitchToTheNextState(State nextState) {
        currentState.enabled = false;
        nextState.enabled = true;
        currentState = nextState;
        
    }
}

enum States {
    IdleState,
    ChaseState,
    AttackState
}
