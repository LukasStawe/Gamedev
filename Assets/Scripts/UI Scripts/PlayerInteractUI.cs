using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractUI : MonoBehaviour
{

    [SerializeField] private GameObject container;
    [SerializeField] private PlayerScript playerInteract;

    [SerializeField] private TextMeshProUGUI InteractableUIText;

    ParentInteractables interactable;

    public void Show(ParentInteractables interactable)
    {
        InteractableUIText.text = interactable.interactTextToDisplay;
        container.SetActive(true);
    }

    public void Hide()
    {
        container.SetActive(false);
    }    
}
