using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Items
{
    public ScriptableItem item;

    public override string interactTextToDisplay => "Press E to pick up " + item.name + ".";

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp()
    {

        //Add Item to inventory
        bool wasPickedUp = Inventory.instance.Add(item);

        //Remove Object from Scene
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }


    }
}
