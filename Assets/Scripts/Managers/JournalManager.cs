using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that manages the entirety of the Journal
/// </summary>
public class JournalManager : MonoBehaviour
{
    #region Singleton

    public static JournalManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of JournalManager found!");
            return;
        }
        instance = this;
    }

    #endregion
    
    /// <summary>
    /// The structurce for a Journal Entry, consisting of an ID and the Entry itself.
    /// </summary>
    public struct JournalEntry
    {
        public int entryID;
        public string entryText;

        //TODO Add a type (Enum) to distinguish between important (Quest) and non-important (Lore) Entries.
    }

    //
    public List<JournalEntry> journalEntries = new();

    /// <summary>
    /// When the gameObject is enabled the AddJournalEntry is added to the Action OnJournalEntryEvent.
    /// </summary>
    private void OnEnable()
    {
        Actions.OnJournalEntryEvent += AddJournalEntry;
    }

    /// <summary>
    /// When the gameObject is disabled the AddJournalEntry is removed from the Action OnJournalEntryEvent.
    /// </summary>
    private void OnDisable()
    {
        Actions.OnJournalEntryEvent -= AddJournalEntry;
    }

    /// <summary>
    /// Goes through the entire Lists and checks if the Entry that is about to be added is already in the List. If yes nothing gets added. If not it gets the text assigned to the ID, creates a new journalEntry 
    /// with both and adds it to the List.
    /// </summary>
    /// <param name="journalID">The ID of the Journal Entry that is going to be added to the Journal List</param>
    public void AddJournalEntry(int journalID)
    {
        foreach (JournalEntry existingEntry in journalEntries) 
        {
            Debug.Log(existingEntry.entryID);
            if (existingEntry.entryID == journalID)
            {
                Debug.Log("Repeating entry found");
                return;
            }
        }

        string newJournalEntry = GetJournalEntry(journalID);

        if (newJournalEntry == null)
        {
            return;
        }

        JournalEntry entry;
        entry.entryID = journalID;
        entry.entryText = newJournalEntry;


        journalEntries.Add(entry);
           
    }

    /// <summary>
    /// Returns the Text assigned to the ID of a journal entry.
    /// </summary>
    /// <param name="journalID"> The ID of the new Journal Entry </param>
    /// <returns> The text assigned to the ID of the new Entry </returns>
    private string GetJournalEntry(int journalID)
    {
        switch (journalID)
        {
            case 0:
                return "I found this weird looking key. Wonder what it unlocks.";
            case 1:
                return "I talked to this weird looking cube. Maybe he can help me but first I have to get some Cheese for him.";
            case 2:
                return "I helped a Gnoblin from being killed by his friends. He said he could help me but first I should get him some cheese. What is it with this obsession over cheese? Anyways, I might find some in the main Kitchen Storage";
            case 3:
                return "I brought the Gnoblin his cheese. He seemed overjoyed, guess he really loves cheese. He said the Gnoblins dug a tunnel into the Castle long ago and used it to for mischevious activities but it collabsed after he and" + 
                    "his friends came here a few days ago. They are using it to stash their valuables until they can leave again. Apparently it's entrance is in the Prison. But he warned me that last time they went there one of these 'things'" +
                    "surprised them but they managed to lock it up, sadly in the cell the entrances is located.";
            case 4:
                return "Super Mario was based on an Italian man I saw die in an accident on a construction site. I thought to myself 'What if that man had been killed by a gorilla, rather than negligence?'";
            case 5:
                return "A game is eventually a game but a game is forever game.";
            case 6:
                return "Has anyone really been far even as decided to use even go want to do look more like?";
            case 7:
                return "Have you ever had a dream that you, um, you had, your, you- you could, you’ll do, you- you wants, you, you could do so, you- you’ll do, you could- you, you want, you want him to do you so much you could do anything?";
            case 8:
                return "AWDSMEAFOOTHIMAAAFOOTAFOOTWHSCUSEME.";
            case 11:
                return "What was that? I encountered something that looked like a human but was completely unresponsive. It attacked me on first sight. Thankfully it was quite slow so I was able to overcumb it. What is going on here? I need to be more careful, who knows what else lurks in the shadows.";
            case 12:
                return "Another Placeholder string";
            default:
                return null;
        }
    }
}
