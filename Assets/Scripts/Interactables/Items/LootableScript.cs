using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableScript : Interactable
{
    public List<ScriptableItem> loot = new List<ScriptableItem>();

    public override void Interact()    
    {
        Debug.Log(this.gameObject.name + " looted.");
    }

}
