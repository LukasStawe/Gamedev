using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipmentField : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    ScriptableEquipment equipment;

    public void AddItem(ScriptableEquipment newItem)
    {
        equipment = newItem;

        icon.sprite = equipment.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        equipment = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(equipment);
    }

    public void Unequip()
    {
        if (equipment != null)
        {
            EquipmentManager.instance.Unequip((int)equipment.equipSlot);
        }
    }
}
