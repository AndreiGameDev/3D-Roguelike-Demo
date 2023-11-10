using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationPlayer : MonoBehaviour
{
    AICharacterManager character;

    private void Start() {
        character = GetComponent<AICharacterManager>();
    }
    

    // Plays the animation respective to the string given and tells the character manager it's performing an action and can move by defualt and plays the animation at x1
    public void PlayTargetActionAnimation(string targetAnimation, 
                                            bool isPerformingAction = true, 
                                            bool canMove = true, 
                                            float AnimatorSpeed = 1f ) {
        character.animator.speed = AnimatorSpeed;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
    }

    // Plays the animation respective to the string given and plays it at the animation x1 by default
    public void PlayTargetActionAnimation(string targetAnimation, float AnimatorSpeed = 1f) {
        character.animator.speed = AnimatorSpeed;
        character.animator.CrossFade(targetAnimation, 0.2f);
        character.isPerformingAction = true;
        character.canMove = true;
    }


}
