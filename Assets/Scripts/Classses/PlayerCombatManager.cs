using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerCombatManager : CombatManager {
    public Camera fpsCamera;
    CharacterManager characterManager;
    PlayerInputManager inputManager;
    [HideInInspector] public CharacterAnimationManager animator;
    public GameObject damageNumberPrefab;
    public List<ItemList> items = new List<ItemList>();
    public Transform VFXHolder;
    public PlayerUIManager UIManager;
    public float ability1Damage;
    public float ability2Damage;
    public float difficultyDamage;

    private void Awake() {
        difficultyDamage = GameManager.Instance.difficultyAmplify;
        inputManager = PlayerInputManager.Instance;
        characterManager = GetComponent<CharacterManager>();
        animator = GetComponent<CharacterAnimationManager>();
        UIManager = GetComponent<PlayerUIManager>();
        damageNumberPrefab = (GameObject)Resources.Load("Prefabs/DamageNumber", typeof(GameObject));
        GameManager.Instance.isInWave = false;
    }
    public new void Start() {
        base.Start();
        GameManager.Instance.Score = 0;
        StartCoroutine(CallItemUpdate());
        StartCoroutine(CallDamageOvertimeItem());
    }
    public void CallStatUpdateOnItemPickup() {
        foreach (ItemList i in items) {
            i.item.OnStatChange(this, i.stacks);
        }
    }

    private void Update() {
        HandleAllAttacks();
        Interact();
        ItemTabToggle();
        Die();
    }

    // Toggles crystal tab ui (tab key)
    private void ItemTabToggle() {
        if (inputManager.hasPlayerCheckedItemsTab()) {
            UIManager.CrystalTabUi();
        }
    }

    // Handles input resposes
    void HandleAllAttacks() { 
        //Primary Attack
        if (inputManager.hasPrimaryFireTriggered()) {
            PrimaryAttack();
        }
        //Secondary Attack
        if (inputManager.hasSecondaryFireTriggered()) {
            SecondaryAttack();
        }
        //Ability1
        if (inputManager.hasAbility1Triggered()) {
            Abillity1();
        }
        //Ability2
        if (inputManager.hasAbility2Triggered()) {
            Abillity2();
        }

    }

    // All the functions 80-117 lines are just the functions for each ability named and checked whether the player is already doing an input and the attack logic being defined
    public void PrimaryAttack() {
        if (!characterManager.isPerformingAction) {
            PrimaryAttackLogic();
        }
    }
    public virtual void PrimaryAttackLogic() {
        // Primary Attack Logic
    }
    public void SecondaryAttack() {
        if (!characterManager.isPerformingAction) {
            SecondaryAttackLogic();
        }
    }

    public virtual void SecondaryAttackLogic() {
        // Secondary Attack Logic
    }

    public void Abillity1() {
        if (!characterManager.isPerformingAction) {
            Abillity1Logic();
        }
    }

    public virtual void Abillity1Logic() {
        // First Abilitty Logic
    }

    public void Abillity2() {
        if (!characterManager.isPerformingAction) {
            Abillity2Logic();
        }
    }

    public virtual void Abillity2Logic() {
        // Second Abillity logic
    }


    // Create damage number popup when hitting enemies
    public void CreateNumberPopUp(Vector3 position, string text, Color color) {
        var popup = Instantiate(damageNumberPrefab, position + Vector3.up, quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;
        Destroy(popup, 1f);
    }

    // Shoots a raycast to check for objects that are interactable
    void Interact() {
        if (inputManager.hasPlayerInteracted()) {
            RaycastHit[] hits = Physics.RaycastAll(fpsCamera.transform.position, fpsCamera.transform.forward, 20f, LayerMask.GetMask("Tooltip") | LayerMask.GetMask("Default"), QueryTriggerInteraction.Collide);
            Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

            foreach (RaycastHit hit in hits) {
                IInteract interactObject = hit.collider.GetComponent<IInteract>();
                Debug.Log("Looking for IInteractScript");
                if (interactObject != null) {
                    Debug.Log("Interacting");
                    interactObject.Interact(this);
                    break;
                } else {
                    Debug.Log("Not interactable");
                }
            }
        }
    }

    // Item related code from line 149-181

    // Calls Item Updates every 1 second
    IEnumerator CallItemUpdate() {
        foreach (ItemList i in items) {
            i.item.Update(this, i.stacks);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemUpdate());
    }

    // Call DOT items every second
    IEnumerator CallDamageOvertimeItem() {
        foreach (ItemList i in items) {
            i.item.OnPeriodicItemEvent(this, i.stacks);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallDamageOvertimeItem());
    }

    // Calls On kill item effects function
    public void OnKillItemEffect() {
        foreach (ItemList i in items) {
            i.item.OnKill(this, i.stacks);
        }
    }

    // Creates gameobjects(VFX) under vfx holder
    public void CallItemVFX() {
        foreach (ItemList i in items) {
            i.item.CreateVFX(VFXHolder);
        }
    }

    // When the player dies load the death scene
    void Die() {
        if(health <= 0) {
            SceneManager.LoadScene("DeathScene");
        }
    }
}
