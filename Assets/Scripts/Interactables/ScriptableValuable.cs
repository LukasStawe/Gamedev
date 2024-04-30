using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Valuable", menuName = "Inventory/Item/Valuable")]
public class ScriptableValuable : ScriptableObject
{
    new public string name = "New Item";
    public int value;
}
