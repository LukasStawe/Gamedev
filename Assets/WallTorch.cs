using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallTorch : MonoBehaviour, ParentInteractables
{
    [SerializeField] private GameObject torchObject;

    [SerializeField] private TorchScript torchScript;

    [SerializeField] private bool hasTorch = true;

    public string interactTextToDisplay
    {
        get
        {
            if (hasTorch)
            {
                return "Press E to toggle the Torch." + "\n" +
                       "Press F to take the Torch.";
            }
            else
            {
                return "Press E to toggle the Torch." + "\n" +
                       "Press F to add a Torch (requires Torch).";
            }
        }
    }

    private void TakeTorch()
    {
        if (!Inventory.instance.Add(torchScript.torch)) return;
        torchObject.SetActive(false);
        hasTorch = false;
    }

    private void AddTorch()
    {
        Inventory inventory = Inventory.instance;
        if (inventory.SearchForSO(torchScript.torch) == -1) return;
        inventory.Remove(torchScript.torch);
        torchObject.SetActive(true);
        hasTorch = true;
    }

    public void SecondaryInteract()
    {
        if (hasTorch)
        {
            TakeTorch();
        } else
        {
            AddTorch();
        }
    }

    public void Interact()
    {
        if(!hasTorch) { return; }

        Toggle();
    }

    private void Toggle()
    {
        torchScript.Interact();
    }
}
