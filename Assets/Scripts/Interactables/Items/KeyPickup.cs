using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : Items
{
    public ScriptableKey key;

    public override string interactTextToDisplay => "Press E to pick up " + key.name + ".";

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    /// <summary>
    /// 
    /// </summary>
    void PickUp()
    {

        //Add Item to inventory
        bool wasPickedUp = Inventory.instance.Add(key);

        //Remove Object from Scene
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }


    }
}
