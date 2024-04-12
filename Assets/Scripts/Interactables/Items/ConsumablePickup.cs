using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablePickup : Items
{
    public ScriptableConsumable consumable;

    public override string interactTextToDisplay => "Press E to pick up " + consumable.name + ". \n" +
                                                    "Press F to eat " + consumable.name + ".";

    private void Awake()
    {
    }

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {

        //Add Item to inventory
        bool wasPickedUp = Inventory.instance.Add(consumable);

        //Remove Object from Scene
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }


    }

    public void Eat()
    {
        PlayerScript player = PlayerScript.Instance;
        if (player.currentHealth + consumable.restoredHealth > player.maxHealth)
        {
            player.currentHealth = player.maxHealth;
            player.healthBar.SetHealth(player.maxHealth);
        } else
        {
            player.currentHealth += consumable.restoredHealth;
            player.healthBar.SetHealth(player.currentHealth);
        }

        Destroy(gameObject);
        Debug.Log("Healed. Health now at " + player.currentHealth);
    }
}
