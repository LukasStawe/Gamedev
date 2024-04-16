using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

/**
 * Manager for the Equipment of the Player. Has a list of currently Equiped Items, an Event triggered when Equipment is changed, references to the Inventory and Player
 * as well as to the Players Hand and all equippable Weapons.
 */
public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this; 
    }
    #endregion

    ScriptableEquipment[] currentEquipment;

    public delegate void OnEquipmentChanged(ScriptableEquipment newItem, ScriptableEquipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;
    PlayerScript player;

    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject axe;
    [SerializeField]
    private GameObject bow;

    /**
     * Instantiates Inventory and Player Singletons.
     * Gets the Number of possible EquipmentSlots the player has and creates an Array with that size.
     * Also instantiates all Weapons and Deactivates them in the Players Hand.
     */
    void Start()
    {
        inventory = Inventory.instance;
        player = PlayerScript.Instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new ScriptableEquipment[numSlots];
    }

    /**
     * @param newEquipment: The newly equipped Armor.
     * 
     * Equips the new Armor by making sure that if an Item was already equipped in that slot it gets added back to the Inventory.
     */
    public void Equip (ScriptableArmor newArmor)
    {
        int slotIndex = (int)newArmor.equipSlot;

        ScriptableEquipment oldArmor = null;

        if (currentEquipment[slotIndex] != null )
        {
            oldArmor = currentEquipment[slotIndex];
            inventory.Add(oldArmor);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newArmor, oldArmor);
        }

        currentEquipment[slotIndex] = newArmor;
    }

    /**
     * @param newEquipment: The newly equipped Weapon.
     * 
     * Equipts the new Weapon by making sure that if a Weapon was already equipped it gets added back to the Inventory and then Activates that weapon in the Players Hand.
     */
    public void Equip(ScriptableWeapon newWeapon)
    {
        int slotIndex = (int)newWeapon.equipSlot;

        ScriptableEquipment oldWeapon = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldWeapon = currentEquipment[slotIndex];
            DisableWeapon((ScriptableWeapon) oldWeapon);            
            inventory.Add(oldWeapon);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newWeapon, oldWeapon);
        }

        currentEquipment[slotIndex] = newWeapon;

        EnableWeapon(newWeapon);

        player.weaponEquipped = true;
        player.weaponScript = newWeapon;
    }

    /**
     * @param slotIndex: The Index for the Slot of the Item that is to be unequipped. 0 = Helmet, 1 = Chest, 2 = Legs, 3 = Weapon, 4 = Secondary.
     * 
     * Unequips the Item of the given Slot by adding it back to the Inventory and removing it from the Slot and also deacitvating the Weapon if the unequipped Item is a Weapon.
     */
    public void Unequip (int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            ScriptableEquipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }

            if (slotIndex == 3)
            {
                sword.gameObject.SetActive(false);
                axe.gameObject.SetActive(false);
                bow.gameObject.SetActive(false);

                player.weaponEquipped = false;
            }
        }

        
    }

    /**
     * @param weapon: The equipped weapon.
     * 
     * Activates the Equipped Weapon in your hand.
     */
    public void EnableWeapon(ScriptableWeapon weapon) 
    {
        switch ((int) weapon.weapon) {
            case 0: sword.gameObject.SetActive(true);
                return; 
            case 1: axe.gameObject.SetActive(true); 
                return; 
            case 2: break; 
            case 3: break; 
            case 4: bow.gameObject.SetActive(true);
                break;
                }
    }

    public void DisableWeapon(ScriptableWeapon weapon)
    {
        switch ((int)weapon.weapon)
        {
            case 0:
                sword.gameObject.SetActive(false);
                return;
            case 1:
                axe.gameObject.SetActive(false);
                return;
            case 2: break;
            case 3: break;
            case 4: bow.gameObject.SetActive(false); 
                break;
        }
    }

}
