using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AINavigator : MonoBehaviour
{
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
    public void Move() {
            agent.isStopped = false;
            agent.SetDestination(targetTransform.position);
    }
    public bool isInAttackRange() {
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance <= OwnerAttackRange) {
            return true;
        } else {
            return false;
        }
    }

    public void Stop() {
            agent.isStopped = true;
    }

    private void Update() {
    }
}
