using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterAnimationManager : MonoBehaviour {
    CharacterManager character;

    private void Start() {
        character = GetComponent<CharacterManager>();
    }

    // This function plays the animation for arms and the weapon, then sets variables and sets the animation speed.
    public void PlayTargetActionAnimation(string targetArmsAnimation,
                                            string targetWeaponAnimation,
                                            bool isPerformingAction, 
                                            float AnimatorSpeed = 1f,
                                            bool applyRootMotion = true,
                                            bool canRotate = true, 
                                            bool canMove = true,
                                            bool canLook = true) {
        character.armsAnimator.speed = 1;
        character.weaponAnimator.speed = 1;
        character.armsAnimator.applyRootMotion = applyRootMotion;
        character.armsAnimator.CrossFade(targetArmsAnimation, 0.2f);
        character.weaponAnimator.applyRootMotion = applyRootMotion;
        character.weaponAnimator.CrossFade(targetWeaponAnimation, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
        character.canRotate = canRotate;
        character.canLook = canLook;
    }
}