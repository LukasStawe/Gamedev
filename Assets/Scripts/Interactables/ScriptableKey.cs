using UnityEngine;

/**
 * A Scriptable Object for keys. Used to determine in Inventory if the Item picked up is a normal Item or a Key to put it into a seperate Inventory
 * Doesn't add any variables, inherits everything from ScriptableItem.
 */
[CreateAssetMenu(fileName = "New Key", menuName = "Inventory/Item/Key")]
public class ScriptableKey : ScriptableItem
{

    //new public string name = "New Item";
    //public Sprite icon = null;

}
