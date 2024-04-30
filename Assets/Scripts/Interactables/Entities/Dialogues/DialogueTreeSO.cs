using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueTree", menuName = "Dialogue/DialogueTree")]

public class DialogueTreeSO : ScriptableObject
{

    public DialogueSection[] sections;

    /**
     * 
     */
    [System.Serializable]
    public struct DialogueSection
    {
        [TextArea]
        public string[] dialogue;
        public bool endAfterDialogue;
        public bool hasBranchPoint;
        public BranchPoint branchPoint;
    }

    /**
     * 
     */
    [System.Serializable]
    public struct BranchPoint
    {
        [TextArea]
        public string question;
        public Answer[] answers;
    }

    /**
     * 
     */
    [System.Serializable]
    public struct Answer
    {
        public string answerLabel;
        public int nextElement;
        public bool hasBeenClicked;
    }

}