using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public override State RunCurrentState() {
        if(navigator.isInAttackRange()) {
            return attackState;
        } else {
            return this;
        }
        
    }
}
