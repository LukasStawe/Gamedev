using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropWindowUI : MonoBehaviour
{
    [SerializeField]
    private Button okayButton;
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private Slider slider;
    
    InventoryUI inventoryUI;
    InventorySlot currentSlot = null;
    int amount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUI(InventorySlot slot)
    {
        gameObject.SetActive(true);
        currentSlot = slot;
        amount = slot.amount;
        slider.maxValue = amount;
        slider.value = 1;
        okayButton.onClick.AddListener(() => slot.DropMultiple(int.Parse(inputText.text)));
        okayButton.onClick.AddListener(delegate { HideUI(); });
    }

    public void HideUI()
    {
        amount = 0;
        currentSlot = null;
        okayButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    public void OnValueChanged()
    {
        inputText.text = Math.Clamp(int.Parse(inputText.text), 1, amount).ToString();
        slider.value = int.Parse(inputText.text);
    }

    public void SliderChanged()
    {
        inputText.text = slider.value.ToString();
    }

    public void Up()
    {
        int value = int.Parse(inputText.text);
        if (value >= amount) return;
        value++;
        inputText.text = value.ToString();
    }

    public void Down()
    {        
        int value = int.Parse(inputText.text);
        if (value <= 1) return;
        value--;
        inputText.text = value.ToString();
    }
}
