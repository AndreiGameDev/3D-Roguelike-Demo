using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLocomotionManager : MonoBehaviour {
    // Grabs input manager so the script can know what the player pressed
    PlayerInputManager inputManager;
    // Grabs the charactermanager to check for certain conditions like if the player canmove or not
    CharacterManager characterManager;
    // Grabs the playercombatmanager script to grab base stats such as movement speed
    PlayerCombatManager combatManager;
    //Physics
    public float gravity = -9.81f;
    Vector3 velocity;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask; // Which layer to check for ground
    public float jumpHeight = 3f;
    Rigidbody rb;

    Vector2 playerAcceleration;


    private void Start() {
        rb = GetComponent<Rigidbody>();
        inputManager = PlayerInputManager.Instance;
        characterManager = GetComponent<CharacterManager>();
        combatManager = GetComponent<PlayerCombatManager>();


    }

    private void FixedUpdate() {
        isMoving();
        HandleGroundedMovement();
        Jump(); // Jump Function
    }

    void HandleGroundedMovement() {
        if (characterManager.canMove) {
            //Resets velocity.y when grounded
            if (isGrounded() && velocity.y < 0) {
                velocity.y = -2f;
            }
            // grabs imput from player, then moves
            Vector2 playerInput = inputManager.GetPlayerMovement();
            //   Vector3 Move = transform.right * playerInput.x + transform.forward * playerInput.y;


            playerAcceleration = playerAcceleration + (playerInput * Time.deltaTime);

            //if (playerInput.magnitude < 0.01f) {
            //    playerAcceleration = playerAcceleration * .95f;
            //}


            //if (playerAcceleration.magnitude > 1) {
            //    playerAcceleration = playerAcceleration.normalized;
            //}

            // https://forum.unity.com/threads/make-character-accelerates-using-character-controller-component.848044/ acceleration / deceleration

            Vector3 Move = transform.right * playerInput.x + transform.forward * playerInput.y;
            characterManager.characterController.Move(Move * combatManager.movementSpeed * Time.deltaTime);
            // Handle gravity
            velocity.y += gravity + Time.deltaTime;
            characterManager.characterController.Move(velocity * Time.deltaTime);
        }
        if (inputManager.GetPlayerMovement() == Vector2.zero) {

        }
    }

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

    void Jump() {

        if (inputManager.hasPlayerJumped() && isGrounded()) {
            Debug.Log(inputManager.hasPlayerJumped());
            Debug.Log(isGrounded());
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

}