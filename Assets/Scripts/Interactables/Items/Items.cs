using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Items : MonoBehaviour, ParentInteractables
{
    [field:SerializeField] public virtual string interactTextToDisplay { get; }

    public virtual void Interact()
    {

        Debug.Log(this.gameObject.name + " picked up.");

    }
}
