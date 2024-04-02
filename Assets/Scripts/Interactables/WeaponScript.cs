using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{

    public ScriptableWeapon weapon;
    
    float attackRange;

    int damage;

    float attackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        attackRange = weapon.attackRange;
        damage = weapon.damageModifier;
        attackSpeed = weapon.attackSpeed;
    }

    public int getDamage()
    {
        return damage;
    }

    public float getRange()
    {
        return attackRange;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
}
