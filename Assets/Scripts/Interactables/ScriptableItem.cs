using UnityEngine;


public class ScriptableItem : ScriptableObject
{
    new public string name = "New Item";
    [TextArea]
    public string description;
    public Sprite icon = null;
    public GameObject itemPrefab;

    public bool isStackable = false;
    public int maxStackSize = 1;

    public bool isCombinable = false;

    public virtual void Use()
    {
        //Use item

        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this); 
    }

    /**
     * To be overriden in 
     * Drops the item when removed from Inventory by taking the player Position, direction and rotation and instantiating
     * the Prefab in front of the player.
     */
    public virtual void Drop() 
    {
        Transform dropPoint = PlayerScript.Instance.dropPoint;

        ItemManager.instance.SpawnObject(name, dropPoint);

        Debug.Log(name + " dropped.");
    }

    public void Drop(int amount)
    {
        Transform dropPoint = PlayerScript.Instance.dropPoint;
        for (int i = 0; i < amount; i++)
        {
            ItemManager.instance.SpawnObject(name, dropPoint);
        }
    }
}
