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

    [SerializeField] private Transform rotationPoint;

    [SerializeField] private Transform upperChest;

    //Text to display when looking at the Chest
    public override string interactTextToDisplay
    {
        get
        {
            if (isLocked) return "Press E to Unlock. (Requires " + chest.requiredKey.name + ")";
            else return "Press E to Loot.";
        }
    }

    /// <summary>
    /// Copies the Loot from the ScriptableChest to the actual Item (to not delete it from the SO) 
    /// </summary>
    private void Start()
    {
        foreach(ScriptableItem item in chest.Loot)
        {
            loot.Add(item);
        }
    }

    /// <summary>
    /// Checkes if the chest is Locked first. If yes, tries to unlock it and returns. If not, tells the UI for looting to be created.
    /// </summary>
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

            StartCoroutine(MoveChest(30f));

            StartCoroutine(WaitFor(1f));
        }
    }

    public void CloseChest(float degree = -30f)
    {
        StartCoroutine(MoveChest(degree));
    }

    /// <summary>
    /// Unlocks the Chest if it was locked and the player has the Required Key (from ScriptableChest)
    /// </summary>
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
            Destroy(this);
        }
    }

    /// <summary>
    /// Visualizes the opening and closing of the chest by rotating the upper part of the Chest around a point on the edge.
    /// </summary>
    /// <param name="degree"> The degree by which the upper part of the chest should rotate </param>
    /// <returns> Waits a frame after a rotating-step </returns>
    private IEnumerator MoveChest(float degree)
    {
        //TODO
        for (var t = 0f; t < 1; t += Time.deltaTime / 1f)
        {
            upperChest.RotateAround(rotationPoint.position, -Vector3.forward, degree * Time.deltaTime);
            yield return null;
        }        
        yield break;
    }

    private IEnumerator WaitFor(float time)
    {
        yield return new WaitForSeconds(time);
        lootUI.createUI(this);
        yield break;
    }
}
