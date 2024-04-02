using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LootChest : LootableScript
{
    //Script that includes the Loot and the required Key to unlock the Door
    public ScriptableChest chest;

    //Bool to check if the Chest is locked
    public bool isLocked = true;

    //Reference to the PlayerInventory in GameManager
    [field: SerializeField]  private Inventory playerInventory;

    //Reference to the LootUI in PlayerInventory Canvas
    [SerializeField] private LootUI lootUI;

    //Text to display when looking at the Chest
    public override string interactTextToDisplay
    {
        get
        {
            if (isLocked) return "Press E to Unlock. (Requires " + chest.requiredKey.name + ")";
            else return "Press E to Loot.";
        }
    }

    /*
     * Copys the Loot from the ScriptableChest to the actual Item (to not delete it from the SO) 
     */
    private void Start()
    {
        foreach(ScriptableItem item in chest.Loot)
        {
            loot.Add(item);
        }
    }


    /*
     * Loots the Chest one Item at a time if its unlocked 
     */
    public override void Interact()
    {
        if (isLocked)
        {
            Unlock();
            return;
        }
        if(!isLocked)
        {
            base.Interact();

            lootUI.createUI(this);
        }
    }

    /*
     * Unlocks the Chest if it was locked and the player has the Required Key (from ScriptableChest)
     */
    public void Unlock()
    {
        if (playerInventory.keys.Contains(chest.requiredKey))
        {
            isLocked = false;
            Debug.Log("Chest Unlocked!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO Marks the Chest as fully looted when its empty
        if (loot.Count == 0)
        {
            //TODO Mark Chest as empty -> Open
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private IEnumerator MoveChest(float degree)
    {

    }
}
