using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLocomotionManager : MonoBehaviour {
    PlayerInputManager inputManager;
    CharacterManager characterManager;
    PlayerCombatManager combatManager;
    public float gravity = -9.81f;
    Vector3 velocity;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    public float jumpHeight = 3f;

    private void Start() {
        inputManager = PlayerInputManager.Instance;
        characterManager = GetComponent<CharacterManager>();
        combatManager = GetComponent<PlayerCombatManager>();
    }

    private void FixedUpdate() {
        isMoving();
        HandleGroundedMovement();
        Jump();
    }

    void HandleGroundedMovement() {
        if (characterManager.canMove) {
            if (isGrounded() && velocity.y < 0) {
                velocity.y = -2f;
            }
            Vector2 playerInput = inputManager.GetPlayerMovement();
            Vector3 Move = transform.right * playerInput.x + transform.forward * playerInput.y;
            characterManager.characterController.Move(Move * combatManager.movementSpeed * Time.deltaTime);
            // Handle gravity
            velocity.y += gravity + Time.deltaTime;
            characterManager.characterController.Move(velocity * Time.deltaTime);
        }
    }

    // Handles animations based on the player speed
    void isMoving() {
        float playerSpeedx = inputManager.GetPlayerMovement().x;
        float playerSpeedy = inputManager.GetPlayerMovement().y;
        float playerSpeed = playerSpeedx + playerSpeedy;
        float walkingThreshhold = 0f;
        if (playerSpeedx > walkingThreshhold | playerSpeedx < walkingThreshhold | playerSpeedy > walkingThreshhold | playerSpeedy < walkingThreshhold) {
            characterManager.armsAnimator.SetBool("isMoving", true);
            characterManager.weaponAnimator.SetBool("isMoving", true);
        } else if (playerSpeed == 0) {
            characterManager.armsAnimator.SetBool("isMoving", false);
            characterManager.weaponAnimator.SetBool("isMoving", false);
        }
    }

    //Checks if the sphere that got created at the players feet hits the ground or not
    public bool isGrounded() {
        return Physics.CheckSphere(groundCheck.position, groundDistance);
    }

    // Jumps
    void Jump() {

        if (inputManager.hasPlayerJumped() && isGrounded()) {
            Debug.Log(inputManager.hasPlayerJumped());
            Debug.Log(isGrounded());
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

}