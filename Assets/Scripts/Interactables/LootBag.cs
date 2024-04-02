using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class LootBag : LootableScript
{
    public override string interactTextToDisplay => "Press E to Loot.";

    Inventory playerInventory;

    //Reference to the LootUI in PlayerInventory Canvas
    LootUI lootUI;

    private void Start()
    {
        //Gets the Player Inventory
        playerInventory = Inventory.instance;

        lootUI = PlayerScript.Instance.lootUI;
    }

    public override void Interact()
    {

        base.Interact();

        lootUI.createUI(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Destroys the Bag if everything has been picked up
        if (loot.Count == 0)
        {
            Debug.Log("Loot destroyed");
            Destroy(this.gameObject);
        }
    }
}
