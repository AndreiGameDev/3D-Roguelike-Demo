using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationPlayer : MonoBehaviour
{
    AICharacterManager character;

    private void Start() {
        character = GetComponent<AICharacterManager>();
    }

    public void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction = true, bool canMove = true, float AnimatorSpeed = 1f ) {
        /* Default settings when the command gets run >
        string targetAnimation = null,
        bool isPerformingAction = true;,
        bool canMove = false */
        character.animator.speed = AnimatorSpeed;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
    }

    public void PlayTargetActionAnimation(string targetAnimation, float AnimatorSpeed = 1f) {
        /* Default settings when the command gets run >
        string targetAnimation = null,
        bool isPerformingAction = true;,
        bool canMove = false */
        character.animator.speed = AnimatorSpeed;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = true;
        character.canMove = true;
    }


}
