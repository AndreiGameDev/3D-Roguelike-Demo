using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : State {
    public ChaseState chaseState;
    public AttackState attackState;
    public override State RunCurrentState() {
        if (navigator.isInAttackRange()) {
            return attackState;
        } else if(!navigator.isInAttackRange()) {
            return chaseState;
        } else {
            return this;
        }
        

    }
}
