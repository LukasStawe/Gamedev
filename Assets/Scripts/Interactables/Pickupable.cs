using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour, ParentInteractables
{
    public string interactTextToDisplay => "Press E to pick up this object.";

    public void Interact()
    {
        Debug.Log(this.gameObject.name + " picked up.");
    }
}