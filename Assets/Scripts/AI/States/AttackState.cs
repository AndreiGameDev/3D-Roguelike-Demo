using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState;

    public override State RunCurrentState() {
        if (!character.isPerformingAction && !navigator.isInAttackRange()) {
            return chaseState;
        } else {
            return this;
        }
    }
}
