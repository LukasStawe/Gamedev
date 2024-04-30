using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
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

    public delegate void OnMoneyChanged(int value, int change);
    public event OnMoneyChanged onMoneyChangedCallback;

    public int space = 24;

    //public List<ScriptableItem> items = new List<ScriptableItem>();
    public List<Item> items = new List<Item>();
    
    public List<ScriptableItem> keys = new List<ScriptableItem>();

    public int money = 0;

    public Ammo equippedAmmo;


    /// <summary>
    /// When the gameObject is enabled the AddMoney is added to the Action OnValuableEvent.
    /// </summary>
    private void OnEnable()
    {
        Actions.OnMoneyChangedEvent += AddMoney;
    }

    /// <summary>
    /// When the gameObject is disabled the AddMoney is removed from the Action OnValuableEvent.
    /// </summary>
    private void OnDisable()
    {
        Actions.OnMoneyChangedEvent -= AddMoney;
    }

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
            Item newItem;
            int index = SearchForSO(item);
            if (item.isStackable && index != -1)
            {
                newItem.item = item;
                newItem.amount = items[index].amount + 1;
                items[index] = newItem;
                Debug.Log(items[index].amount);
               
            } else
            {
                newItem.item = item;
                newItem.amount = 1;
                items.Add(newItem);
            }
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
        int index = SearchForSO(item);
        if (items[index].amount == 1)
        {
            items.Remove(items[index]);
        } else
        {
            Item newItem;
            newItem.item = item;
            newItem.amount = items[index].amount - 1;
            items[index] = newItem;
            Debug.Log(items[index].amount);
        }

        if (onItemChangedCallback != null) { onItemChangedCallback.Invoke(); }
    }

    /// <summary>
    /// Adds an amount (positive or negative) to the players money.
    /// </summary>
    /// <param name="amount">The amount to be added to the money</param>
    public void AddMoney(int amount)
    {
        money += amount;
        if (onMoneyChangedCallback != null) { onMoneyChangedCallback.Invoke(money, amount); }
    }

    public int SearchForSO(ScriptableItem item)
    {
        foreach (Item itemStruct in items)
        {
            if (itemStruct.item == item)
            {
                return items.IndexOf(itemStruct);
            }
        }
        return -1;
    }
}
public struct Ammo
{
    public AmmoType type;
    public int amount;
}

public enum AmmoType
{
    Arrow, FireArrow, RopeArrow, PoisonArrow, Bolt
}

public struct Item
{
    public ScriptableItem item;
    public int amount;
}

