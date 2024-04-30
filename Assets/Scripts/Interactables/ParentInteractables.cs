using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ParentInteractables
{
    [property: SerializeField] public abstract string interactTextToDisplay { get; }
    public abstract void Interact();
}
