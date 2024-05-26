using UnityEngine;
using UnityEditor;
using System;

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
        [HideInInspector] public BranchPoint branchPoint;
        public bool activatesQuest;
        [HideInInspector] public int questID;
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

//[CustomEditor(typeof(DialogueTreeSO))]
//public class DialogueTreeEditor : Editor
//{
//    override public void OnInspectorGUI()
//    {
//        var script = target as DialogueTreeSO;

//        script.hasBranchPoint = EditorGUILayout.Toggle("Has Branch Point", script.hasBranchPoint);

//        script.activatesQUest = EditorGUILayout.Toggle("Activates Quest", script.activatesQuest);

//        if (script.hasBranchPoint == true) script.branchPoint = EditorGUILayout.PropertyField(serializedObject.FindProperty("branchPoint", true);

//        if (script.activatesQuest == true) script.questID = EditorGUILayout.IntField("Quest ID:", script.questID);
//    }
//}