using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Items
{
    public ScriptableEquipment equipment;
    public override string interactTextToDisplay => "Press E to pick up " + equipment.name + ".";

    public override void Interact()
    {

        Debug.Log(this.gameObject.name + " picked up.");

    }
}
