using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
//If Reenabling remember to re-add IPointerEnterHandler and IPointerExitHandler next to MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    
    ScriptableItem item;

    public void AddItem(ScriptableItem newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {        
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        item.Drop();
        Inventory.instance.Remove(item);

        transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>().HideTooltip();
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }

        transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>().HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (icon == null || item == null) return;

        transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>().DisplayTooltip(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (icon == null || item == null) return;

        transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>().HideTooltip();
    }

}
