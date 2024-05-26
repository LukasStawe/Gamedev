using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler //, IPointerClickHandler
//If Reenabling remember to re-add IPointerEnterHandler and IPointerExitHandler next to MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI amountUI;
    public int amount = 0;

    private InventoryUI inventoryUI;
    
    public ScriptableItem item = null;    

    private void Awake()
    {
        
    }

    private void Start()
    {
        inventoryUI = transform.parent.parent.parent.gameObject.GetComponent<InventoryUI>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem.item;
        amount = newItem.amount;
        amountUI.text = amount.ToString();

        if (amount > 1)
        {
            amountUI.gameObject.SetActive(true);
        } else
        {
            amountUI.gameObject.SetActive(false);
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
        amountUI.gameObject.SetActive(false);
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        item.Drop();
        Inventory.instance.Remove(item);
    }

    public void DropMultiple(int amount)
    {
        item.Drop(amount);
        Inventory.instance.RemoveMultiple(item, amount);
    }

    public void OnDoubleClick()
    {
        if (item == null) return;
        UseItem();
    }

    public void UseItem()
    {
        if (item == null) return;
        Debug.Log(item.name + " USED!");

        item.Use();        

        if (amount > 1)
        {
            amountUI.gameObject.SetActive(true);
            amountUI.text = amount.ToString();
        }
        else
        {
            amountUI.gameObject.SetActive(false);
            inventoryUI.HideTooltip();
        }
    }

    public void OnRightClick()
    {
        Debug.Log("IMPLEMENT CONTEXT MENU");
        if (item == null) return;
        inventoryUI.CreateContextMenu(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (icon == null || item == null) return;

        inventoryUI.DisplayTooltip(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (icon == null || item == null) return;

        inventoryUI.HideTooltip();
    }
}
