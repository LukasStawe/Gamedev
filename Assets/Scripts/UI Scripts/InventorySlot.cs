using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
//If Reenabling remember to re-add IPointerEnterHandler and IPointerExitHandler next to MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI amountUI;
    public int amount;
    
    ScriptableItem item;    

    private void Awake()
    {
        PlayerScript.Instance.actions.FindActionMap("UI").FindAction("DoubleClick").performed += OnDoubleClick;
        PlayerScript.Instance.actions.FindActionMap("UI").FindAction("RightClick").performed += OnRightClick;
    }

    public void AddItem(Item newItem)
    {
        item = newItem.item;
        amount = newItem.amount;
        amountUI.text = amount.ToString();

        if (amount > 1)
        {
            amountUI.enabled = true;
        } else
        {
            amountUI.enabled = false;
        }

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

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
        amount = 0;
        icon.sprite = null;
        icon.enabled = false;
        amountUI.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        item.Drop();
        Inventory.instance.Remove(item);

        transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>().HideTooltip();
    }

    private void OnDoubleClick(InputAction.CallbackContext context)
    {
        Debug.Log("Double Clicked!");
        UseItem();
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }

        transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>().HideTooltip();

        amountUI.text = amount.ToString();
        if (amount > 1)
        {
            amountUI.enabled = true;
        }
        else
        {
            amountUI.enabled = false;
        }
    }
    public void OnRightClick(InputAction.CallbackContext context)
    {
        //TODO Implement Context Menu!
        Debug.Log("IMPLEMENT CONTEXT MENU");
        UseItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered!");
        if (icon == null || item == null) return;

        transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>().DisplayTooltip(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (icon == null || item == null) return;

        transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>().HideTooltip();
    }

}
