using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LootUI : MonoBehaviour
{
    [SerializeField] private GameObject container;

    public bool isShown;

    GameObject lootWindow;

    [SerializeField] private GameObject lootParent;

    [SerializeField] private GameObject lootSlot;

    private RectTransform window;

    public List<ScriptableItem> lootCopy;

    private void OnEnable()
    {
        Actions.OnLoot += UpdateUIAfterLooting;
    }

    private void OnDisable()
    {
        Actions.OnLoot -= UpdateUIAfterLooting;
    }

    // Start is called before the first frame update
    void Start()
    {
        lootWindow = container.gameObject;
        window = lootWindow.GetComponent<RectTransform>();
    }

    public void Show()
    {
        lootWindow.SetActive(true);
        isShown = true;
    }

    public void Hide()
    {
        container.SetActive(false);

        EmptyList();

        isShown = false;
    }

    public void createUI(LootableScript lootScript)
    {
        lootCopy = lootScript.loot;

        window.sizeDelta = new Vector2(300, 80 + lootCopy.Count * 100);

        foreach(ScriptableItem item in lootCopy)
        {
            GameObject button = Instantiate(lootSlot, lootParent.transform);
            LootSlot lootSlotScript = button.GetComponent<LootSlot>();
            lootSlotScript.AddLoot(item);
        }

        Show();
    }

    public void UpdateUIAfterLooting(ScriptableItem lootItem)
    {
        lootCopy.Remove(lootItem);

        window.sizeDelta = new Vector2(300, 80 + lootCopy.Count * 100);

        EmptyList();

        foreach (ScriptableItem item in lootCopy)
        {
            GameObject button = Instantiate(lootSlot, lootParent.transform);
            LootSlot lootSlotScript = button.GetComponent<LootSlot>();
            lootSlotScript.AddLoot(item);
        }

        if (lootCopy.Count <= 0) 
        {
            Hide();
        }

    }

    private void EmptyList()
    {
        foreach (Transform child in lootParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void LootAll()
    {
        foreach (ScriptableItem item in lootCopy)
        {
            Inventory.instance.Add(item);
        }

        Hide();
    }
}
