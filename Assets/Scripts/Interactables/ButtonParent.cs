using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The abstract class for all Levers/Buttons that the player can interact with and that are connected to a target object.
/// </summary>
public abstract class ButtonParent : MonoBehaviour,  ParentInteractables
{
    [SerializeField]
    protected Gate connectedObject;

    [SerializeField] public virtual string interactTextToDisplay { get; }

    public abstract void Interact();
}
