using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public ScriptableDestructable destructable;

    [SerializeField] GameObject bag;

    readonly int requiredHits = 3;

    int health;

    private void Start()
    {
        health = requiredHits;
    }

    /// <summary>
    /// Spawns the Lootbag with the Loot from the attached ScriptableEnemy
    /// </summary>
    public void SpawnLoot()
    {
        foreach (ScriptableItem item in destructable.Loot)
        {
            ItemManager.instance.SpawnObject(item.name, this.transform);
        }
    }

    /// <summary>
    /// Calculates the Damage the Enemy takes based on its armor
    /// </summary>
    public void TakeDamage()
    {
        health--;
                
        //Destroy
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Checks if the Container had Loot and spawns it and then destroys the Container Object
    /// </summary>
    void Die()
    {
        Debug.Log("Box destroyed");

        //Spawn Loot
        if (destructable.Loot.Count != 0)
        {
            SpawnLoot();
        }


        //Disable Enemy
        Destroy(this.gameObject);
    }
}
