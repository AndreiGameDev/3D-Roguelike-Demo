using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPickup : MonoBehaviour, IInteract, ITooltip {
    public Item item;
    public Items itemDrop;
    public ItemInteractionType itemType;
    private void Start() {
        item = AssignItem(itemDrop);
    }

    // Grabs tooltip information for the player to see
    public void Tooltip(PlayerUIManager player) {
        player.SetTooltip(item.GiveName(), item.GiveDescription());
    }
    // Adds the item to the player and destroys the gameobject after
    public void Interact(PlayerCombatManager player) {
        Debug.Log("Crystal Interaction");
        if (player != null) {
            AddItem(player);
            switch (itemType) {
                case ItemInteractionType.ChangesStats:
                    player.CallStatUpdateOnItemPickup();
                    break;
                case ItemInteractionType.CreatesVFX:
                    player.CallItemVFX();
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
    }

    // Adds the item to the player logic
    void AddItem(PlayerCombatManager player) {
        foreach(ItemList i in player.items) {
            if (i.name == item.GiveName()) {
                i.stacks += 1;
                return;
            }
        }
        player.items.Add(new ItemList(item, item.GiveName(), 1));
    }

    //Assigns item depending on the value of the enumerator in the inspector.
    public Item AssignItem(Items itemToAssign) {
        switch (itemToAssign) {
            case Items.HealingCrystal:
                return new HealingCrystal();
            case Items.PoisonCloudCrystal: 
                return new PoisonCloudCrystal();
            case Items.SpeedCrystal:
                return new SpeedCrystal();
            case Items.DeffenseCrystal: 
                return new DeffenseCrystal();
            case Items.PowerCrystal: 
                return new PowerCrystal();
            case Items.RejuvenateCrystal: 
                return new RejuvenateCrystal();
            default:
                return new HealingCrystal();
        }
    }
    
}
//List of items each pickup will be assigned to.
public enum Items {
    HealingCrystal,
    PoisonCloudCrystal,
    SpeedCrystal,
    DeffenseCrystal,
    PowerCrystal,
    RejuvenateCrystal
}

// Interaction type that the crystal does related to the player
public enum ItemInteractionType {
    Nothing,
    ChangesStats,
    CreatesVFX
    
}
