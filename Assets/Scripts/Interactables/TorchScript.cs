using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : Interactable
{

    public Light flame;
    public bool isLit;
    public ScriptableWeapon torch;

    public override string interactTextToDisplay
    {
        get
        {
            if (isLit) return "Press E to extinguish the Torch.";
            else return "Press E to light the Torch.";
        }
    }

    void Start()
    {
        flame.enabled = isLit;
    }

    public override void Interact()
    {
        flame.enabled = !isLit;
        flame.gameObject.SetActive(!isLit);
        isLit = !isLit;
    }
}
