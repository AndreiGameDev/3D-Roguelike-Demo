using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPickup : MonoBehaviour, IInteract, ITooltip {
    public Item item;
    public Items itemDrop;
    public ItemInteractionType itemType;
    private void Start() {
        // Depending on the item from the enumerator selected assigned the variable Item to values of the actual item
        item = AssignItem(itemDrop);
        
    }

    public void Tooltip(PlayerUIManager player) {
        player.SetTooltip(item.GiveName(), item.GiveDescription());
    }
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

public enum ItemInteractionType {
    Nothing,
    ChangesStats,
    CreatesVFX
    
}
