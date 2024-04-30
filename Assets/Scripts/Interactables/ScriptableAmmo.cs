using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo", menuName = "Inventory/Item/Ammo")]
public class ScriptableAmmo : ScriptableItem
{
    public AmmoType Type;

    public float damage;

    public bool isAflame = false;


    public override void Use()
    {
        base.Use();
        
        //TODO Equip Ammo in AmmoSlot
    }
}
