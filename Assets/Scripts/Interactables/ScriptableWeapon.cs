using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Item/Equipment/Weapon")]

/**
 * Scriptable Class of Weapons which inherits from Equipment.
 * 
 * Weapons have a damageModifier, attackRange, attackSpeed, as well as a Position and Rotation and a Weapon Type.
 */
public class ScriptableWeapon : ScriptableEquipment
{

    public int damageModifier;
    public float attackRange;
    public float attackSpeed;

    public WeaponType weapon;

    public bool isRanged;

    public bool isTwoHanded;

    /**
     * When the Weapon gets clicked on in the Inventory it gets equipped.
     */
    public override void Use()
    {
        //Equip the item
        EquipmentManager.instance.Equip(this);

        //Remove it from the Inventory
        base.Use();

        RemoveFromInventory();
    }    

    /**
     * The different Weapon Types.
     */
    public enum WeaponType
    {
        Sword, Axe, Spear, Crossbow, Bow, Club, Torch
    }

}
