using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This Class manages the Inventory of the Player.
 * 
 * It has a space-limit and two Lists, one for regular Items and a special one just for Keys.
 */
public class Inventory : MonoBehaviour
{

    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 24;

    public List<ScriptableItem> items = new List<ScriptableItem>();

    public List<ScriptableItem> keys = new List<ScriptableItem>();

    /**
     * @param item: The item that was picked up and is now added to the Inventory.
     * 
     * Adds a newly picked up item (or unequipped Equipment) to the Inventory IF the inventory is not full. Keys get added to a seperate Inventory.
     * 
     * Returns true if the Item was successfully added. Only returns false when the Inventory was full and the Item could not be added.
     */
    public bool Add(ScriptableItem item)
    {
        if (items.Count >= space) 
        {
            Debug.Log("Inventory full.");
            return false;
        }
        if (item is ScriptableKey)
        {
            keys.Add(item);

        } else
        {
            items.Add(item);
        }        

        if (onItemChangedCallback!= null) { onItemChangedCallback.Invoke(); }        

        return true;
    }

    /**
     * @param item: The item that is going to be removed from the Inventory.
     * 
     * Removes an Item from Inventory/the items List, either on Use or on Drop/Destroy.
     */
    public void Remove(ScriptableItem item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null) { onItemChangedCallback.Invoke(); }
    }


}
