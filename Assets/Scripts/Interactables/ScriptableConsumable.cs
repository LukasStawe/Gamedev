using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Item/Consumable")]

/**
 * Scriptable Class of Consumables which inherit from Items.
 * 
 * Each Consumable has an amount of restored Health (can be negative).
 */
public class ScriptableConsumable : ScriptableItem
{
    public int restoredHealth = 0;

    [TextArea]
    public string usedDescription;

    /**
     * When the Consumable gets clicked on in the Inventory it restores (or removes) Health from the Player.
     */
    public override void Use()
    {
        base.Use();
        PlayerScript instance = PlayerScript.Instance;
        if (instance.currentHealth + restoredHealth > instance.maxHealth)
        {
            instance.currentHealth = instance.maxHealth;
            instance.healthBar.SetHealth(instance.maxHealth);
        }
        else
        {
            instance.currentHealth += restoredHealth;
            instance.healthBar.SetHealth(instance.currentHealth);
        }
        RemoveFromInventory();
        Debug.Log("Healed. Health now at " + instance.currentHealth);
    }


    /**
     * Drops the item when removed from Inventory by taking the player Position, direction and rotation and instantiating
     * the Prefab in front of the player. Also connects the playerScript with the ConsumableScript of the item.
     **/
    public override void Drop()
    {
        base.Drop();
        //PlayerScript player = PlayerScript.Instance;
        //itemPrefab.GetComponent<ConsumablePickup>().player = PlayerScript.Instance;
    }
}

