using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    InventorySlot[] slots;

    EquipmentField[] equipmentFields;

    [Header("Tooltip Objects")]
    [SerializeField]
    private GameObject tooltip;    
    [SerializeField]
    private GameObject tooltipName;
    [SerializeField]
    private GameObject tooltipDescription;
    [SerializeField]
    private GameObject tooltipStats;

    [Header("Context Menu Objects")]
    [SerializeField]
    private GameObject contextMenu;
    [SerializeField]
    private GameObject useButtonPrefab;
    [SerializeField]
    private GameObject dropButtonPrefab;
    [SerializeField]
    private GameObject dropAllButtonPrefab;
    [SerializeField]
    private GameObject inspectButtonPrefab;
    [SerializeField]
    private GameObject combineButtonPrefab;
    private List<GameObject> buttonList = new();

    [Header("Drop Window Objects")]
    [SerializeField]
    private GameObject dropWindow;
    [SerializeField]
    private DropWindowUI dropUI;
    private Button dropButton;
    private Text dropAmount;

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

    public void CreateContextMenu(InventorySlot slot)
    {
        if (contextMenu.activeSelf)
        {
            HideContextMenu();
        }

        contextMenu.SetActive(true);
        
        int buttons = 3; //Use, Inspect and Drop are for all Items

        //TODO Instantiate Use, inspect and drop button (with input on how many to drop)
        InstantiateButton(useButtonPrefab).GetComponent<Button>().onClick.AddListener(() => slot.UseItem());
        InstantiateButton(inspectButtonPrefab).GetComponent<Button>().onClick.AddListener(() => slot.UseItem());
        InstantiateButton(dropButtonPrefab).GetComponent<Button>().onClick.AddListener(() => slot.OnRemoveButton());

        if (slot.item.isStackable && slot.amount > 1)
        {
            buttons++;
            //TODO instantiate Drop all button
            InstantiateButton(dropAllButtonPrefab).GetComponent<Button>().onClick.AddListener(() => dropUI.ShowUI(slot));
        }

        if (slot.item.isCombinable)
        {
            buttons++;
            //TODO instantiate combine button
            InstantiateButton(combineButtonPrefab).GetComponent<Button>().onClick.AddListener(() => slot.UseItem());
            
        }

        RectTransform rect = contextMenu.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(250, 20 + buttons * 50 + (buttons - 1) * 5);
        Vector2 position;
        position = Input.mousePosition;
        position.y += rect.sizeDelta.y / 2;
        position.x += Input.mousePosition.x < Screen.width/2 ? rect.sizeDelta.x / 2 : -rect.sizeDelta.x / 2;
        rect.anchoredPosition = position;
    }

    public void HideContextMenu()
    {
        foreach (GameObject button in buttonList)
        {
            Destroy(button);
        }
        buttonList.Clear();
        contextMenu.SetActive(false);
    }

    private GameObject InstantiateButton(GameObject prefab)
    {
        GameObject button = Instantiate(prefab);
        button.transform.SetParent(contextMenu.transform);
        button.GetComponent<Button>().onClick.AddListener(() => HideContextMenu());
        buttonList.Add(button);
        return button;
    }
}
