using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AINavigator : MonoBehaviour {
    public NavMeshAgent agent;
    GameObject target;
    public string TargetTag = "";
    Transform targetTransform;
    float OwnerAttackRange;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag(TargetTag);
        targetTransform = target.transform;
        OwnerAttackRange = GetComponent<AICombatManager>().AttackRange;
        agent.speed = GetComponent<AICombatManager>().movementSpeed;
    }

    // Tells the agent it's moving and sets the destination to the player position
    public void Move() {
        agent.isStopped = false;
        agent.SetDestination(targetTransform.position);
    }

    // If it's in attack range then it's true
    public bool isInAttackRange() {
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance <= OwnerAttackRange) {
            Debug.Log("In attack range");
            return true;
        } else {
            Debug.Log("Not in attack range");
            return false;
        }
    }

    // Tells the agent it's not moving
    public void Stop() {
        agent.isStopped = true;
    }
}
