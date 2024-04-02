using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScriptableEquipment : ScriptableItem
{
    public EquipmentSlot equipSlot;

}



public enum EquipmentSlot
{
    Head, Chest, Legs, Weapon, Secondary
}
