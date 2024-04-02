using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendlyNPC : Interactable
{
    // The dialogue Tree for the NPC.
    public DialogueTreeSO dialogue;

    public ScriptableNPC npc;

    public bool hasBeenTalkedTo = false;

    [SerializeField]
    private DIalogueUI dialogueUI;

    /**
     * Function that gets activated when interacting with the Object. So far it just gives out a Log message.
     */
    public override string interactTextToDisplay
    {
        get
        {
            return "Press E to speak to " + npc.name + ".";
        }
    }
    public override void Interact()
    {
        base.Interact();

        dialogueUI.createUI(this);
        
    }
}
