using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : Readables
{
    //public ScriptableBooks book;

    public bool wasRead = false;

    public override string interactTextToDisplay => "Press E to read " + book.name + ".";

    
    public override void Interact()
    {
        Actions.OnRead?.Invoke(this);
        base.Interact();
        wasRead = true;

        if (book.addsJournalEntry)
        {
            Actions.OnJournalEntryEvent?.Invoke(book.journalID);
        }
    }
}
