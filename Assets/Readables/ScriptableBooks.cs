using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Book", menuName = "Readables/Books")]
public class ScriptableBooks : ScriptableObject
{

    new public string name = "New Book";
    [TextArea] public string text = "This is a test.";

    public bool addsJournalEntry;
    public int journalID;
}


