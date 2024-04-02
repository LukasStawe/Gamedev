using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject container;

    public bool isShown;

    Inventory inventory;

    EquipmentManager equipment;

    public Transform itemsParent;

    public Transform equipmentParent;

    InventorySlot[] slots;

    EquipmentField[] equipmentFields;

    [SerializeField]
    private GameObject tooltip;

    [SerializeField]
    private GameObject tooltipName;
    [SerializeField]
    private GameObject tooltipDescription;
    [SerializeField]
    private GameObject tooltipStats;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        equipment = EquipmentManager.instance;
        equipment.onEquipmentChanged += UpdateUI;

        equipmentFields = equipmentParent.GetComponentsInChildren<EquipmentField>();

        Hide();
    }

    public void Show()
    {
        container.SetActive(true);
        isShown = true;
    }

    public void Hide()
    {
        container.SetActive(false);
        isShown = false;
    }


    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if ( i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            } else
            {
                slots[i].ClearSlot();
            }
        }
    }

    void UpdateUI(ScriptableEquipment newItem, ScriptableEquipment oldItem)
    {
        if (newItem != null)
        {
            equipmentFields[(int)newItem.equipSlot].AddItem(newItem);
        } else
        {
            equipmentFields[(int)oldItem.equipSlot].ClearSlot();
        }
    }

    public void DisplayTooltip(ScriptableItem item)
    {
        tooltip.SetActive(true);

        tooltipName.GetComponent<TextMeshProUGUI>().text = item.name;
        tooltipDescription.GetComponent<TextMeshProUGUI>().text = item.description;

        if (item is ScriptableEquipment)
        {

        }
        else
        {
            tooltipStats.SetActive(false);
        }
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
