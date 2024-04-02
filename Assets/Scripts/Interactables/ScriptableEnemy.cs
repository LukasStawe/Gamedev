using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class ScriptableEnemy : ScriptableObject
{

    new public string name = "New Enemy";

    public int attackDamage;
    public float attackRange;
    public float attackSpeed;

    public int maxHealth;

    public float movementSpeed;

    public float detectionRadius;

    public float chaseRange;

    public int armor;

    public bool addsJournalEntry;
    public int journalID;

    public List<ScriptableItem> Loot = new List<ScriptableItem>();
}
