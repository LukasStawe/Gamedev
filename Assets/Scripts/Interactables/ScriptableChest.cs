using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Interactable/Chest")]
public class ScriptableChest : ScriptableObject
{
    new public string name = "New Item";

    public List<ScriptableItem> Loot = new List<ScriptableItem>();

    public ScriptableKey requiredKey;

}
