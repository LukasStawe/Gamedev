using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class of Readable Object which inherits from the Interface ParentInteractables.
 * 
 * Each Readable has a Text as that is shown when looking at it (Which button to press) as well as a Scriptable Book which has the actual Text displayed when reading it.
 */
public class Readables : MonoBehaviour,  ParentInteractables
{
    [field:SerializeField] public virtual string interactTextToDisplay { get; }
    public ScriptableBooks book;

    public static Action<Readables> OnRead, OnStopReading;

    /**
     * Function that gets activated when interacting with the Object. So far it just gives out a Log message.
     */
    public virtual void Interact()
    {
        Debug.Log(this.gameObject.name + " read.");
    }
}
