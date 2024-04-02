using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI lootName;

    public ScriptableItem loot;

    public void AddLoot(ScriptableItem newItem)
    {

        icon.GetComponent<Image>().sprite = newItem.icon;
        lootName.text = newItem.name;
        loot = newItem;
    }

    public void Loot()
    {
        Inventory.instance.Add(loot);
        Actions.OnLoot(loot);
        Destroy(gameObject);

    }
}
