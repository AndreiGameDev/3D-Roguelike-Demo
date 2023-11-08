using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterAnimationManager : MonoBehaviour {
    CharacterManager character;

    private void Start() {
        character = GetComponent<CharacterManager>();
    }
    public void PlayTargetActionAnimation(string targetArmsAnimation, string targetWeaponAnimation, bool isPerformingAction, float AnimatorSpeed = 1f,bool applyRootMotion = true, bool canRotate = true, bool canMove = true, bool canLook = true) {
        /* Default settings when the command gets run >
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false
        canLook = true */
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