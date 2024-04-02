using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Item/Equipment/Armor")]

public class ScriptableArmor : ScriptableEquipment
{
    /**
    * Scriptable Class of Armors which inherits from Equipment.
     * 
    * Armors have a armorModifier, healthModifier as well as a speedModifier.
     */
    public float armorModifier;
    public float healthModifier;
    public float speedModifier;

    /**
     * When the Armor gets clicked on in the Inventory it gets equipped.
     */
    public override void Use()
    {
        //Equip the item
        EquipmentManager.instance.Equip(this);

        //Remove it from the Inventory
        base.Use();

        RemoveFromInventory();
    }

}
