using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Destructable", menuName = "Destructable")]
public class ScriptableDestructable : ScriptableObject
{
    public List<ScriptableItem> Loot = new();
}
