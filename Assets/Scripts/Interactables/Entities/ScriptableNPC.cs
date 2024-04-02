using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC")]

/**
 * Scriptable Class of an NPC.
 * 
 * Each NPC has a name and a dialogue.
 */
public class ScriptableNPC : ScriptableObject
{

    new public string name = "New NPC";

    public List<ScriptableItem> Rewards = new List<ScriptableItem>();
    
}
