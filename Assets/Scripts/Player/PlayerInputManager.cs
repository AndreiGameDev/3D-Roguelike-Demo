using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour {
    private static PlayerInputManager _instance;
    public static PlayerInputManager Instance {
        get {
            return _instance;
        }
    }

    PlayerControls playerControls;
    private void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
        }
        if (playerControls == null) {
            playerControls = new PlayerControls();
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChange;
        _instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene) {
        // When we are loading into any of the playable levels, enable our player controls
        foreach (int i in WorldSaveGameManager.instance.GetWorldSceneIndex()) {
            if (newScene.buildIndex == i) {
                _instance.enabled = true;
            }
        // Otherwise we must be at the main menu, disable player controls
        else {
                _instance.enabled = false;
            }
        }

    }

    private void OnEnable() {
        
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    private void OnDestroy() {
        // If we destroy this object, unsubscribe from the event
        //SceneManager.activeSceneChanged -= OnSceneChange;
    }

    public Vector2 GetPlayerMovement() {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta() {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool hasPlayerJumped() {
        return playerControls.Player.Jump.triggered;
    }
    
    public bool hasPrimaryFireTriggered() {
        return playerControls.Player.PrimaryAttack.triggered;
    }

    public bool hasSecondaryFireTriggered() { 
        return playerControls.Player.SecondaryAttack.triggered;
    }

    public bool hasAbility1Triggered() { 
        return playerControls.Player.Ability1.triggered;
    }
    public bool hasAbility2Triggered() {
        return playerControls.Player.Ability2.triggered;
    }

    public bool hasUltimateAbilityTriggered() {
        return playerControls.Player.UltimateAbility.triggered;
    }
    
    public bool hasReloadTriggered() {
        return playerControls.Player.Reload.triggered;
    }

    public bool hasPlayerInteracted() {
        return playerControls.Player.Interact.triggered;
    }

    public bool hasPlayerCheckedItemsTab() {
        return playerControls.Player.ItemTab.triggered;
    }
}