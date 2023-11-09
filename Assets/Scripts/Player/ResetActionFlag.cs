using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager character;

    //When action ends, the override actions return to "Empty" - this toggles the correct flags for the player to move.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(character == null) {
            character = animator.GetComponentInParent<CharacterManager>();
        }

        character.isPerformingAction = false;
        character.canMove = true;
        character.canRotate = true;
        character.applyRootMotion = false;
    }
}
